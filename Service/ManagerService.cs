using ServiceAbestraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ManagerService(Func<IAuthenticationService> AuthFunc,Func<IDoctorService> Doctorfunc,
        Func<ISpecialtyService> Specialtyfunc,Func<IAppointmentService> AppointmentFunc, Func<IPaymobService> PaymentFunc) : IManagerService
    {
        public IAuthenticationService authentication => AuthFunc.Invoke();

        public IDoctorService doctor => Doctorfunc.Invoke();

        public ISpecialtyService specialty => Specialtyfunc.Invoke();

        public IAppointmentService appointment => AppointmentFunc.Invoke();

        public IPaymobService payment => PaymentFunc.Invoke();
    }
}
