﻿using workshop.wwwapi.Models;

namespace workshop.wwwapi.DTO
{
    public class AppointmentGet
    {
        public DateTime Booktime {  get; set; }
        public AppointmentType Type { get; set; }
        public int DoctorId { get; set; }
        public string? DoctorFullName { get; set; }
        public int PatientId { get; set; }
        public string? PatientFullName { get; set; }
    }
}
