using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.AppointmentDto
{
    public class ReturnAppointmentDto
    {
        public int Id { get; set; }
        public string status { get; set; } = default!;
        public DateTime BookingDate { get; set; }
    }
}
