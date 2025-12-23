using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specification
{
    public class GetPatientAppointmentSpec : Specification<Appointment, int>
    {
        public GetPatientAppointmentSpec(string patientId) : base(e=>e.PatientId==patientId)
        {
        }
    }
}
