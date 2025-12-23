using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Appointment : BaseEntity<int>
    {
        [ForeignKey("Slot")]
        public int SlotId { get; set; }
        public Slot Slot { get; set; }
        [ForeignKey("Doctor")]
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public string PatientId { get; set; }

        public DateTime BookingDate { get; set; }= DateTime.Now;
        public Status Status { get; set; } 
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public ICollection<PaymentTransaction> PaymentTransactions { get; set; } = new HashSet<PaymentTransaction>();
    }
}
