using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.AppointmentDto
{
    public class BookAppontmentDto
    {
        public int DoctorId { get; set; }
        public int SlotId { get; set; }
        public string PatientId { get; set; }
    }
}
