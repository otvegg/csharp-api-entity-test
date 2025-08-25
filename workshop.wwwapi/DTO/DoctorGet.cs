using workshop.wwwapi.Models;

namespace workshop.wwwapi.DTO
{
    public class DoctorGet
    {
        public string Name { get; set; }
        public ICollection<AppointmentPatient> Appointments { get; set; } = new List<AppointmentPatient>();
    }
}
