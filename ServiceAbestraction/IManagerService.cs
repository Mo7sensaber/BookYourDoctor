using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbestraction
{
    public interface IManagerService
    {
        IAuthenticationService authentication { get; }
        IDoctorService doctor { get; }
        ISpecialtyService specialty { get; }
        IAppointmentService appointment { get; }
        IPaymobService payment { get; } 
    }
}
