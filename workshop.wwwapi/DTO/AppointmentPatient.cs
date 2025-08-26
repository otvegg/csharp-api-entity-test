using workshop.wwwapi.Models;

namespace workshop.wwwapi.DTO
{
    public class AppointmentPatient
    {
        public int PatientId { get; set; }
        public string PatientName { get; set; }
        public DateTime booking { get; set; }
        public AppointmentType Type { get; set; }
    }
}
