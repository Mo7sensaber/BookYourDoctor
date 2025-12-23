using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DoctorDto
{
    public class DoctorProfileDto
    {
        public string UserId { get; set; }
        public int SpecialtyId { get; set; }
        public string Bio { get; set; }
        public decimal SessionPrice { get; set; }
    }
}
