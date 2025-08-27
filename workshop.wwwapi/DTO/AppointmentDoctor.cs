using workshop.wwwapi.Models;

namespace workshop.wwwapi.DTO
{
    public class AppointmentDoctor
    {
        public int DoctorId { get; set; }
        public string? DoctorFullName { get; set; }
        public AppointmentType Type { get; set; }
        public DateTime Booktime { get; set; }
    }
}
