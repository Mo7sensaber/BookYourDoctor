using AutoMapper;
using Domain.Model;
using Domain.RepoInterface;
using ServiceAbestraction;
using Shared.SpecialtyDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class SpecialtyService : ISpecialtyService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public SpecialtyService(IUnitOfWork unitOfWork,IMapper mapper) 
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }
        public async Task<ReturnSpecialtyDto> AddSpecialty(AddSpecialtyDto specialtyDto)
        {
            var specialty = mapper.Map<Specialty>(specialtyDto);
            await unitOfWork.Repository<Specialty, int>().AddAsync(specialty);
            await unitOfWork.SaveChanges();
            var returnSpecialtyDto = mapper.Map<ReturnSpecialtyDto>(specialty);
            return returnSpecialtyDto;
        }

        public async Task DeleteSpecialty(int id)
        {
            var specialtyRepo =await unitOfWork.Repository<Specialty, int>().GetByIdAsync(id);
            if (specialtyRepo != null)
            {
                await unitOfWork.Repository<Specialty, int>().Delete(specialtyRepo.Id);
                await unitOfWork.SaveChanges();
            }
        }

        public async Task<IEnumerable<ReturnSpecialtyDto>> GetAllSpecialties()
        {
            var specialties =await unitOfWork.Repository<Specialty,int>().GetAllAsync();
            var specialtiesDto = mapper.Map<IEnumerable<ReturnSpecialtyDto>>(specialties);
            return specialtiesDto;
        }

        public async Task<ReturnSpecialtyDto> GetSpecialtyById(int id)
        {
            var specialty =await unitOfWork.Repository<Specialty, int>().GetByIdAsync(id);
            if (specialty == null)
            {
                throw new Exception("NotFound This Specialty");
            }
            var specialtyDto = mapper.Map<ReturnSpecialtyDto>(specialty);
            return specialtyDto;
        }

        public async Task<AddSpecialtyDto> UpdateSpecialty(int id, AddSpecialtyDto specialtyDto)
        {
            var specialty = mapper.Map<Specialty>(specialtyDto);
            var existingSpecialty = await unitOfWork.Repository<Specialty, int>().GetByIdAsync(id);
            if (existingSpecialty == null)
            {
                return null;
            }
            existingSpecialty.SpecialtyName = specialty.SpecialtyName;
            existingSpecialty.Description = specialty.Description;
            await unitOfWork.Repository<Specialty, int>().Edit(existingSpecialty);
            await unitOfWork.SaveChanges();
            var updatedSpecialtyDto = mapper.Map<AddSpecialtyDto>(existingSpecialty);
            return updatedSpecialtyDto;
        }
    }
}
