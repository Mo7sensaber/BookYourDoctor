using Microsoft.AspNetCore.Mvc;
using ServiceAbestraction;
using Shared.AppointmentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentController:ControllerBase
    {
        private readonly IManagerService manager;

        public AppointmentController(IManagerService manager)
        {
            this.manager = manager;
        }
        [HttpGet("GetAllAppointPerDoctor")]
        public async Task<ActionResult<IEnumerable<ReturnAppointmentDto>>> GetAllAppointPerDoctor(int doctorId)
        {
            var result = await manager.appointment.GetAllDoctorAppointment(doctorId);
            return Ok(result);
        }
        [HttpPost("CreateAppointment")]
        public async Task<ActionResult<ReturnAppointmentDto>> CreateAppointment(BookAppontmentDto createAppointment)
        {
            var result = await manager.appointment.CreateAppointment(createAppointment);
            return Ok(result);
        }

        [HttpGet("GetAllAppointPerPatient")]
        public async Task<ActionResult<IEnumerable<ReturnAppointmentDto>>> GetAllAppointPerPatient(string patientId)
        {
            var result = await manager.appointment.GetAllPatientAppointments(patientId);
            return Ok(result);
        }
        [HttpDelete("CancelAppointment")]
        public async Task<ActionResult> CancelAppointment(int appointmentId)
        {
            await manager.appointment.DeleteAppointment(appointmentId);
            return Ok("Appointment Canceled");
        }
        [HttpPut("ChangeStatus")]
        public async Task<ActionResult> ChangeStatus(int appointmentId, ChengeStatusDto status)
        {
            await manager.appointment.ChangeAppointmentStatus(appointmentId, status);
            return Ok("Status Changed");
        }

    }
}
