using Shared.SlotDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.DoctorDto
{
    public class DoctorDetailsDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public decimal SessionPrice { get; set; }
        public ICollection<DoctorSlotDto> DoctorSlot { get; set; }
    }
}
