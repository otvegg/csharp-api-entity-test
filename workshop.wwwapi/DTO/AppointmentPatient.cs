using workshop.wwwapi.Models;

namespace workshop.wwwapi.DTO
{
    public class AppointmentPatient
    {
        public int PatientId { get; set; }
        public string? PatientFullName { get; set; }
        public DateTime Booktime { get; set; }
        public AppointmentType Type { get; set; }
    }
}
