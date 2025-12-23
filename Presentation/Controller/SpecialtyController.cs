using Microsoft.AspNetCore.Mvc;
using ServiceAbestraction;
using Shared.SpecialtyDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class SpecialtyController: ControllerBase
    {
        private readonly IManagerService manager;

        public SpecialtyController(IManagerService manager) 
        {
            this.manager = manager;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnSpecialtyDto>>> GetAllSpecialties()
        {
            var result = await manager.specialty.GetAllSpecialties();
            return Ok(result);
        }
        [HttpGet("GetSpecialtyById")]
        public async Task<ActionResult<ReturnSpecialtyDto>> GetSpecialty(int id)
        {
            var specialty= await manager.specialty.GetSpecialtyById(id);
            return Ok(specialty);
        }
        [HttpPost]
        public async Task<ActionResult<AddSpecialtyDto>> AddSpecialty(AddSpecialtyDto specialtyDto)
        {
            var specialty = await manager.specialty.AddSpecialty(specialtyDto);
            return Ok(specialty);
        }
        [HttpPut]
        public async Task<ActionResult<AddSpecialtyDto>> UpdateSpecialty(int id, AddSpecialtyDto specialtyDto)
        {
            var specialty = await manager.specialty.UpdateSpecialty(id, specialtyDto);
            return Ok(specialty);
        }
        [HttpDelete]
        public async Task<ActionResult> DeleteSpecialty(int id)
        {
            await manager.specialty.DeleteSpecialty(id);
            return Content("Deleted Successfully");
        }

    }
}
