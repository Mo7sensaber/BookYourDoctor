using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Doctor: BaseEntity<int>
    {
        public string UserId { get; set; } = default!;

        [NotMapped] // هذا السطر يمنع EF من محاولة ربط الجدول بقاعدة بيانات أخرى
        public ApplicationUser User { get; set; }

        [ForeignKey("Specialty")]
        public int SpecialtyId { get; set; } = default!;
        public Specialty Specialty { get; set; }

        public string Bio { get; set; } = default!;
        public decimal SessionPrice { get; set; }
        public ICollection<Slot> Slots { get; set; }
        public ICollection<Appointment> Appointments { get; set; }
    }
}
