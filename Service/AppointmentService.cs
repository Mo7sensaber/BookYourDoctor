using AutoMapper;
using Domain.Model;
using Domain.RepoInterface;
using Service.Specification;
using ServiceAbestraction;
using Shared.AppointmentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public AppointmentService(IUnitOfWork unitOfWork,IMapper mapper) 
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<ChengeStatusDto> ChangeAppointmentStatus(int appointId, ChengeStatusDto statusDto)
        {
            var appointment = await unitOfWork.Repository<Appointment, int>().GetByIdAsync(appointId);

            if (appointment == null)
                throw new Exception($"NotFound Appointment With Id {appointId}");

            // تحويل الـ Enum من نوع الـ DTO لنوع الـ Domain
            // بنعمل Cast لـ int وبعدين للـ Target Enum
            appointment.Status = (Status)(int)statusDto;

            await unitOfWork.Repository<Appointment, int>().Edit(appointment);
            await unitOfWork.SaveChanges();

            return statusDto;
        }

        public async Task<ReturnAppointmentDto> CreateAppointment(BookAppontmentDto appointmentDto)
        {
            var appointment = mapper.Map<Appointment>(appointmentDto);
            try
            {
                var slot = await unitOfWork.Repository<Slot, int>().GetByIdAsync(appointmentDto.SlotId);
                if (slot == null)
                    throw new Exception($"NotFound Slot With Id {appointmentDto.SlotId}");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            await unitOfWork.Repository<Appointment, int>().AddAsync(appointment);
            await unitOfWork.SaveChanges();
            return mapper.Map<ReturnAppointmentDto>(appointment);
        }

        public async Task DeleteAppointment(int appointId)
        {
            var appointmentRepo =await unitOfWork.Repository<Appointment, int>().GetByIdAsync(appointId);
            if (appointmentRepo == null)
                throw new Exception($"NotFound Appointment With Id {appointId}");
            await unitOfWork.Repository<Appointment, int>().Delete(appointId);
            await unitOfWork.SaveChanges();
        }

        public async Task<IEnumerable<ReturnAppointmentDto>> GetAllDoctorAppointment(int doctorId)
        {
            var appointmenSpec = new GetDoctorAppointmentSpec(doctorId);
            var appointments =await unitOfWork.Repository<Appointment, int>().GetAllSpecAsync(appointmenSpec);
            return mapper.Map<IEnumerable<ReturnAppointmentDto>>(appointments);
        }

        public async Task<IEnumerable<ReturnAppointmentDto>> GetAllPatientAppointments(string patientId)
        {
            var appointmenSpec = new GetPatientAppointmentSpec(patientId);
            var appointments =await unitOfWork.Repository<Appointment, int>().GetAllSpecAsync(appointmenSpec);
            return mapper.Map<IEnumerable<ReturnAppointmentDto>>(appointments);

        }
    }
}
