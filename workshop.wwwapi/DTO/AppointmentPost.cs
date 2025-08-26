using workshop.wwwapi.Models;

namespace workshop.wwwapi.DTO
{
    public class AppointmentPost
    {
        public int patientId {  get; set; }
        public int doctorId { get; set; }
        public AppointmentType Type {  get; set; }
        public DateTime booking { get; set; }
    }
}
