using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DoctorDto
{
    public class ReturnDoctorDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string Specialty { get; set; } = default!;
        public decimal SessionPrice { get; set; } 

    }
}
