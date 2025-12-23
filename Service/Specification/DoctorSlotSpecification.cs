using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Service.Specification
{
    public class DoctorSlotSpecification : Specification<Doctor, int>
    {
        public DoctorSlotSpecification(Expression<Func<Doctor, bool>>? expression) : base(expression)
        {
        }
    }
}
