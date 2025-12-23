using Domain.Model;
using Domain.RepoInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specification
{
    public class DoctorSpecialtySpecification : Specification<Doctor, int>
    {
        public DoctorSpecialtySpecification(int id) : base(o=>o.Id==id)
        {
            AddInclude(o => o.Specialty);
            AddInclude(o=>o.Slots);
        }
        public DoctorSpecialtySpecification() : base(null)
        {
            AddInclude(o => o.Specialty);
            AddInclude(o => o.Slots);
        }
    }
}
