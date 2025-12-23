using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class PaymentTransaction:BaseEntity<int>
    {
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        // البيانات الخاصة بـ Paymob
        public string? PaymobOrderId { get; set; } // يحصل عليه في المرحلة الثانية من الدفع
        public string? TransactionId { get; set; } // يحصل عليه بعد إتمام الدفع بنجاح

        public decimal Amount { get; set; }
        public string Currency { get; set; } = "EGP";

        // حالات الدفع: Pending, Success, Failed, Refunded
        public string PaymentStatus { get; set; } = "Pending";

        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime? UpdatedDate { get; set; }
    }
}
