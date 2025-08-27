using AutoMapper;
using System;
using workshop.wwwapi.DTO;
using workshop.wwwapi.Models;

namespace workshop.wwwapi.Tools
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Patient, PatientGet>();
            CreateMap<Doctor, DoctorGet>();

            CreateMap<Appointment, AppointmentDoctor>()
                .ForMember(dest => dest.DoctorFullName, opt => opt.MapFrom(src => src.Doctor.FullName))
                .ForMember(dest => dest.Booktime, opt => opt.MapFrom(src => src.Booktime));

            CreateMap<Appointment, AppointmentGet>()
                .ForMember(dest => dest.PatientFullName, opt => opt.MapFrom(src => src.Patient.FullName))
                .ForMember(dest => dest.DoctorFullName, opt => opt.MapFrom(src => src.Doctor.FullName))
                .ForMember(dest => dest.Booktime, opt => opt.MapFrom(src => src.Booktime));

            CreateMap<Appointment, AppointmentPatient>()
                .ForMember(dest => dest.PatientFullName, opt => opt.MapFrom(src => src.Patient.FullName))
                .ForMember(dest => dest.Booktime, opt => opt.MapFrom(src => src.Booktime));

            CreateMap<PatientPost, Patient>();
            CreateMap<DoctorPost, Doctor>();
            CreateMap<AppointmentPost, Appointment>();

        }
    }
}
