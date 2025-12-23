using Shared.AppointmentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbestraction
{
    public interface IAppointmentService
    {
        Task<ReturnAppointmentDto> CreateAppointment(BookAppontmentDto appointmentDto);
        Task<IEnumerable<ReturnAppointmentDto>> GetAllPatientAppointments(string patientId);
        Task<IEnumerable<ReturnAppointmentDto>> GetAllDoctorAppointment(int doctorId);
        Task<ChengeStatusDto> ChangeAppointmentStatus(int appointId,ChengeStatusDto status);
        Task DeleteAppointment(int appointId);
    }
}
