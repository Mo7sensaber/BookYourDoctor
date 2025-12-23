using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specification
{
    public class AppointmentWithDoctorSpecification : Specification<Appointment, int>
    {
        public AppointmentWithDoctorSpecification(int id) : base(a=>a.Id==id)
        {
            AddInclude(a => a.Doctor);
        }
    }
}
