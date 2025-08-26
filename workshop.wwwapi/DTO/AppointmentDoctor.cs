using workshop.wwwapi.Models;

namespace workshop.wwwapi.DTO
{
    public class AppointmentDoctor
    {
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public AppointmentType Type { get; set; }
        public DateTime booking { get; set; }
    }
}
