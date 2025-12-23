using AutoMapper;
using Domain.Model;
using Domain.RepoInterface;
using Microsoft.AspNetCore.Identity;
using Service.Specification;
using ServiceAbestraction;
using Shared.DoctorDto;
using Shared.SlotDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork unit;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> manager;

        public DoctorService(IUnitOfWork unit,IMapper mapper,UserManager<ApplicationUser> manager)
        {
            this.unit = unit;
            this.mapper = mapper;
            this.manager = manager;
        }
        public async Task<ReturnDoctorDto> AddDoctorAsync(DoctorProfileDto doctorDto, string userId)
        {
            // 1. التأكد من المستخدم
            var user = await manager.FindByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            // 2. المابينج والحفظ
            var doctorMap = mapper.Map<Doctor>(doctorDto);
            doctorMap.UserId = userId;

            await unit.Repository<Doctor, int>().AddAsync(doctorMap);
            await unit.SaveChanges(); // هنا الدكتور أخد ID في الداتابيز

            // 3. الخطوة السحرية: إعادة تحميل الدكتور بالبيانات المرتبطة
            // هنستخدم Specification تجيب الدكتور بالـ ID بتاعه وتعمل Include لاسم التخصص
            var spec = new DoctorSpecialtySpecification(doctorMap.Id);
            var fullDoctorInfo = await unit.Repository<Doctor, int>().GetByIdSpecAsync(spec);

            // 4. يدويًا: نربط الـ User بالدكتور لأن الـ User في داتابيز تانية
            // الـ EF مش هيعرف يعمل Include لليوزر لأنه في داتابيز تانية، فبنحطه يدوي
            fullDoctorInfo.User = user;

            // 5. المابينج للـ Dto اللي هيرجع
            var returnDoctor = mapper.Map<ReturnDoctorDto>(fullDoctorInfo);

            // تأكد أن الـ Name في الـ DTO بياخد قيمته من fullDoctorInfo.User.DisplayName
            return returnDoctor;
        }

        public async Task<AddSlotDto> AddDoctorSlotsAsync(int doctorId, AddSlotDto slotDto)
        {
            var Doctor =await unit.Repository<Doctor, int>().GetByIdAsync(doctorId);
            if (Doctor == null)
            {
                throw new Exception("Doctor not found");
            }
            var slotMap = mapper.Map<Slot>(slotDto);
            slotMap.DoctorId = doctorId;
            await unit.Repository<Slot, int>().AddAsync(slotMap);
            await unit.SaveChanges();
            var returnSlot = mapper.Map<AddSlotDto>(slotMap);
            return returnSlot;

        }

        public async Task DeleteDoctorAsync(int id)
        {
            var doctor =await unit.Repository<Doctor, int>().GetByIdAsync(id);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }
            await unit.Repository<Doctor, int>().Delete(id);
            await unit.SaveChanges();

        }

        public async Task DeleteDoctorSlotsAsync(int doctorId, int slotId)
        {
            var doctor =await unit.Repository<Doctor, int>().GetByIdAsync(doctorId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }
            var slot = await unit.Repository<Slot, int>().GetByIdAsync(slotId);
            if (slot == null || slot.DoctorId != doctorId)
            {
                throw new Exception("Slot not found for this doctor");
            }
            await unit.Repository<Slot, int>().Delete(slotId);
            await unit.SaveChanges();
        }

        public async Task<ReturnDoctorDto> EditDoctorAsync(int id, DoctorProfileDto doctorDto)
        {
            var doctor =await unit.Repository<Doctor, int>().GetByIdAsync(id);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }
            mapper.Map(doctorDto, doctor);
            await unit.Repository<Doctor, int>().Edit(doctor);
            await unit.SaveChanges();
            var returnDoctor = mapper.Map<ReturnDoctorDto>(doctor);
            return returnDoctor;
        }

        public async Task<DoctorSlotDto> EditDoctorSlotsAsync(int doctorId,  AddSlotDto slotDto)
        {
            var doctor =await unit.Repository<Doctor, int>().GetByIdAsync(doctorId);
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }
            var slot = await unit.Repository<Slot, int>().GetByIdAsync(slotDto.SlotId);
            if (slot == null || slot.DoctorId != doctorId)
            {
                throw new Exception("Slot not found for this doctor");
            }
            mapper.Map(slotDto, slot);
            await unit.Repository<Slot, int>().Edit(slot);
            await unit.SaveChanges();
            var returnSlots = mapper.Map<DoctorSlotDto>(slot);
            return returnSlots;
        }

        public async Task<IEnumerable<ReturnDoctorDto>> GetAllDoctorsAsync()
        {
            // 1. نجيب كل الدكاترة بالتخصصات بتاعتهم من داتابيز الـ Booking
            var doctorSpec = new DoctorSpecialtySpecification();
            var doctors = await unit.Repository<Doctor, int>().GetAllSpecAsync(doctorSpec);

            // 2. نستخرج كل الـ UserIds اللي موجودة عند الدكاترة دول (عشان مجيبش كل اليوزرز اللي في السيستم)
            var userIds = doctors.Select(d => d.UserId).Distinct().ToList();

            // 3. نروح داتابيز الـ Identity نجيب اليوزرز دول بس مرة واحدة (Performance)
            var users = manager.Users.Where(u => userIds.Contains(u.Id)).ToList();

            // 4. نربط كل دكتور باليوزر بتاعه في الميموري عشان المابينج يشتغل
            foreach (var doctor in doctors)
            {
                doctor.User = users.FirstOrDefault(u => u.Id == doctor.UserId);
            }

            // 5. دلوقتي المابينج هيلاقي doctor.User.DisplayName شغال وهيملاه في الـ Name
            var returnDoctors = mapper.Map<IEnumerable<ReturnDoctorDto>>(doctors);

            return returnDoctors;
        }

        public async Task<DoctorDetailsDto?> GetDoctorByIdAsync(int id)
        {
            // 1. نجيب الدكتور بالتخصص بتاعه (من داتابيز الـ Booking)
            // تأكد إن الـ Spec هنا بتعمل filter بـ Id الدكتور
            var spec = new DoctorSpecialtySpecification(id);
            var doctor = await unit.Repository<Doctor, int>().GetByIdSpecAsync(spec);

            if (doctor == null) return null; // أو ارمي Exception لو ده نظامك في المشروع

            // 2. نجيب بيانات اليوزر (من داتابيز الـ Identity)
            var user = await manager.FindByIdAsync(doctor.UserId);

            // 3. الربط اليدوي عشان الـ Mapping يملأ الاسم
            doctor.User = user;

            // 4. تحويل للـ DTO
            return mapper.Map<DoctorDetailsDto>(doctor);
        }

        public async Task<IEnumerable<DoctorSlotDto>> ReturnDoctorSlotsAsync(int doctorId)
        {
            var DoctorSpec = new DoctorSpecialtySpecification(doctorId);
            var doctor =await unit.Repository<Doctor, int>().GetByIdSpecAsync(DoctorSpec);
            
            if (doctor == null)
            {
                throw new Exception("Doctor not found");
            }
            if (doctor.Slots == null || !doctor.Slots.Any())
            {
                throw new Exception("No slots found for this doctor");
            }
            var returnSlots = mapper.Map<IEnumerable<DoctorSlotDto>>(doctor.Slots);
            return returnSlots;

        }
    }
}
