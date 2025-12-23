using Shared.DoctorDto;
using Shared.SlotDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbestraction
{
    public interface IDoctorService
    {
        Task<IEnumerable<ReturnDoctorDto>> GetAllDoctorsAsync();
        Task<DoctorDetailsDto?> GetDoctorByIdAsync(int id);
        Task<ReturnDoctorDto> AddDoctorAsync(DoctorProfileDto doctorDto,string userId);
        Task<ReturnDoctorDto> EditDoctorAsync(int id, DoctorProfileDto doctorDto);
        Task DeleteDoctorAsync(int id);
        Task<IEnumerable<DoctorSlotDto>> ReturnDoctorSlotsAsync(int doctorId);
        Task<AddSlotDto> AddDoctorSlotsAsync(int doctorId, AddSlotDto slotDto);
        Task DeleteDoctorSlotsAsync(int doctorId, int slotId);
        Task<DoctorSlotDto> EditDoctorSlotsAsync(int doctorId, AddSlotDto slotDto);



    }
}
