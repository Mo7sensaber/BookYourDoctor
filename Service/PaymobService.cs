using Domain.Model;
using Domain.RepoInterface;
using Microsoft.Extensions.Configuration;
using ServiceAbestraction;
using System.Net.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Service.Specification;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Identity;
using System.Net.Http.Headers;
using Shared.PaymobDto;

namespace Service
{
    // تعديل الكلاس لضمان استقبال الـ ID كـ string لتجنب إيرور التحويل
    public class PaymobIntentionResponse
    {
        public string client_secret { get; set; } = default!;
        public object id { get; set; } = default!; // استخدمنا object أو string عشان الـ JSON بيبعته أرقام كبيرة
    }

    public class PaymobService : IPaymobService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<ApplicationUser> _userManager;

        public PaymobService(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IUnitOfWork unitOfWork,
            UserManager<ApplicationUser> userManager)
        {
            _httpClientFactory = httpClientFactory;
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        // أضف الـ Usings المطلوبة
        public async Task<bool> UpdatePaymentStatusAsync(string transactionId, string orderIdFromUrl, bool success)
        {
            // البحث المرن: يطابق 439166999 مع pi_test_439166999
            var allTransactions = await _unitOfWork.Repository<PaymentTransaction, int>().GetAllAsync();

            var transaction = allTransactions.FirstOrDefault(t =>
                t.PaymobOrderId != null && t.PaymobOrderId.Contains(orderIdFromUrl));

            if (transaction == null)
            {
                // إذا فشل البحث، نبحث بأحدث معاملة للموعد (كخطة بديلة)
                transaction = allTransactions.OrderByDescending(t => t.CreatedDate).FirstOrDefault();
            }

            if (transaction != null)
            {
                transaction.PaymentStatus = success ? "Success" : "Failed";
                transaction.TransactionId = transactionId; // حفظ رقم العملية الحقيقي

                if (success)
                {
                    var appointment = await _unitOfWork.Repository<Appointment, int>().GetByIdAsync(transaction.AppointmentId);
                    if (appointment != null)
                    {
                        appointment.Status = Status.Confirmed; // التحديث لـ Confirmed
                    }
                }
                return await _unitOfWork.SaveChanges() > 0;
            }
            return false;
        }
        public async Task<string> GetPaymentLinkAsync(int appointmentId)
        {
            // 1. جلب بيانات الموعد والدكتور
            var spec = new AppointmentWithDoctorSpecification(appointmentId);
            var appointment = await _unitOfWork.Repository<Appointment, int>().GetByIdSpecAsync(spec);
            if (appointment == null) throw new Exception("الموعد غير موجود");

            // 2. جلب بيانات المريض من الـ Identity
            var patientUser = await _userManager.FindByIdAsync(appointment.PatientId);
            if (patientUser == null) throw new Exception("بيانات المريض غير موجودة");

            // 3. تجهيز بيانات الـ Intention
            var names = patientUser.DisplayName.Split(' ');
            var intentionData = new
            {
                amount = (int)(appointment.Doctor.SessionPrice * 100),
                currency = "EGP",
                payment_methods = new int[] { int.Parse(_configuration["PaymobSettings:IntegrationId"]) },
                billing_data = new
                {
                    first_name = names[0],
                    last_name = names.Length > 1 ? string.Join(" ", names.Skip(1)) : "Patient",
                    email = patientUser.Email,
                    phone_number = patientUser.PhoneNumber ?? "01000000000",
                    apartment = "NA",
                    floor = "NA",
                    street = "NA",
                    building = "NA",
                    city = "Cairo",
                    country = "EG",
                    state = "Cairo"
                }
            };

            // 4. إرسال الطلب باستخدام الـ Secret Key
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", _configuration["PaymobSettings:SecretKey"]);

            var response = await client.PostAsJsonAsync("https://accept.paymob.com/v1/intention/", intentionData);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Paymob Intention Failed: {error}");
            }

            // 5. قراءة الرد (تم استخدام object للـ id لضمان عدم حدوث Exception)
            var result = await response.Content.ReadFromJsonAsync<PaymobIntentionResponse>();
            if (result == null || string.IsNullOrEmpty(result.client_secret))
                throw new Exception("فشل الحصول على الـ Client Secret");

            // 6. تسجيل المعاملة
            var transaction = new PaymentTransaction
            {
                AppointmentId = appointmentId,
                PaymobOrderId = result.id.ToString()!, // نحوله لـ string قبل التخزين
                Amount = appointment.Doctor.SessionPrice,
                PaymentStatus = "Pending",
                CreatedDate = DateTime.Now
            };
            await _unitOfWork.Repository<PaymentTransaction, int>().AddAsync(transaction);
            await _unitOfWork.SaveChanges();

            // 7. بناء الرابط النهائي
            var publicKey = _configuration["PaymobSettings:PublicKey"];
            return $"https://accept.paymob.com/unifiedcheckout/?publicKey={publicKey}&clientSecret={result.client_secret}";
        }
    }
}