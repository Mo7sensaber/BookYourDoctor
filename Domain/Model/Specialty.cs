using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Model
{
    public class Specialty : BaseEntity<int>
    {
        public string SpecialtyName { get; set; } = default!;
        public string Description { get; set; } = default!;
        public ICollection<Doctor> Doctors { get; set; }

    }
}
