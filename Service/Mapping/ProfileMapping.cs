using AutoMapper;
using Domain.Model;
using Shared.AppointmentDto;
using Shared.DoctorDto;
using Shared.SlotDto;
using Shared.SpecialtyDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Mapping
{
    public class ProfileMapping:Profile
    {
        public ProfileMapping()
        {
            //CreateMap<Source, Destination>();
            CreateMap<Specialty,ReturnSpecialtyDto>().ReverseMap();
            CreateMap<Specialty,AddSpecialtyDto>().ReverseMap();
            CreateMap<DoctorProfileDto, Doctor>().ReverseMap();
            CreateMap<Doctor, ReturnDoctorDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.User.DisplayName))
                .ForMember(dest => dest.Specialty, opt => opt.MapFrom(src => src.Specialty.SpecialtyName)).ReverseMap();
            CreateMap<AddSlotDto, Slot>().ReverseMap();
            CreateMap<Slot,AddSlotDto>()
                .ForMember(dest=>dest.SlotId, opt => opt.MapFrom(src => src.Id)).ReverseMap();

            CreateMap<Slot,DoctorSlotDto>().
                ForMember(dest=>dest.UserId, opt => opt.MapFrom(src => src.Doctor.UserId))
                .ReverseMap();
            CreateMap<Doctor, DoctorDetailsDto>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.User.DisplayName)).
                ForMember(d => d.DoctorSlot, o => o.MapFrom(s => s.Slots))
                .ReverseMap();
            CreateMap<Doctor,DoctorSlotDto>().ReverseMap();
            CreateMap<BookAppontmentDto, Appointment>().
                ForMember(dest=>dest.PatientId, opt => opt.MapFrom(src => src.PatientId))
                .ReverseMap();
            CreateMap<Appointment,ReturnAppointmentDto>().
                ForMember(dest=>dest.BookingDate, opt => opt.MapFrom(src => src.BookingDate))
                .ReverseMap();
        }
    }
}
