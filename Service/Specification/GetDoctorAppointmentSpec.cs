using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specification
{
    public class GetDoctorAppointmentSpec : Specification<Appointment, int>
    {
        public GetDoctorAppointmentSpec(int doctorId) : base(e=>e.DoctorId==doctorId)
        {
            
        }
    }
}
