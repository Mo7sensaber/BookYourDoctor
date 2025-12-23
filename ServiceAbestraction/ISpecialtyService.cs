using Shared.SpecialtyDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceAbestraction
{
    public interface ISpecialtyService
    {
        Task<IEnumerable<ReturnSpecialtyDto>> GetAllSpecialties();
        Task<ReturnSpecialtyDto> GetSpecialtyById(int id);

        Task<ReturnSpecialtyDto> AddSpecialty(AddSpecialtyDto specialtyDto);
        Task<AddSpecialtyDto> UpdateSpecialty(int id, AddSpecialtyDto specialtyDto);
        Task DeleteSpecialty(int id);
    }
}
