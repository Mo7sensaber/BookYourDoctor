using Microsoft.AspNetCore.Mvc;
using ServiceAbestraction;
using Shared.DoctorDto;
using Shared.SlotDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorController : ControllerBase
    {
        private readonly IManagerService manager;

        public DoctorController(IManagerService manager)
        {
            this.manager = manager;
        }
        [HttpGet("GetAllDoctors")]
        public async Task<ActionResult<ReturnDoctorDto>> GetDoctors()
        {
            var result = await manager.doctor.GetAllDoctorsAsync();
            return Ok(result);
        }
        [HttpPost("AddDoctor")]
        public async Task<ActionResult<ReturnDoctorDto>> AddDoctor(DoctorProfileDto doctorDto, string userId)
        {
            var result = await manager.doctor.AddDoctorAsync(doctorDto,userId);
            return Ok(result);
        }
        [HttpGet("GetDoctorByID")]
        public async Task<ActionResult<ReturnDoctorDto>> GetDoctorById( int id)
        {
            var result = await manager.doctor.GetDoctorByIdAsync(id);
            return Ok(result);
        }
        [HttpPost("EditDoctor")]
        public async Task<ActionResult<ReturnDoctorDto>> EditDoctor(int id,DoctorProfileDto doctorDto)
        {
            var result = await manager.doctor.EditDoctorAsync(id,doctorDto);
            return Ok(result);
        }
        [HttpPost("EditDoctorSlot")]
        public async Task<ActionResult<DoctorSlotDto>> EditDoctorSlot(int id, AddSlotDto slotDto)
        {
            var result = await manager.doctor.EditDoctorSlotsAsync(id, slotDto);
            return Ok(result);
        }
        [HttpDelete("DeleteDoctor")]
        public async Task<ActionResult> DeleteDoctor( int id)
        {
            await manager.doctor.DeleteDoctorAsync(id);
            return Ok("Deleted Successfully");
        }
        [HttpPost("AddDoctorSlot")]
        public async Task<ActionResult<DoctorSlotDto>> AddDoctorSlot(int id, AddSlotDto slotDto)
        {
            var result = await manager.doctor.AddDoctorSlotsAsync(id, slotDto);
            return Ok(result);
        }
        [HttpDelete("DeleteDoctorSlot")]
        public async Task<ActionResult> DeleteDoctorSlot(int id, int slotId)
        {
             await manager.doctor.DeleteDoctorSlotsAsync(id, slotId);
            return Ok("Deleted Successfully");

        }
        [HttpGet("GetDoctorSlots")]
        public async Task<ActionResult<IEnumerable<DoctorSlotDto>>> GetDoctorSlots(int id)
        {
            var result = await manager.doctor.ReturnDoctorSlotsAsync(id);
            return Ok(result);
        }

    }
}
