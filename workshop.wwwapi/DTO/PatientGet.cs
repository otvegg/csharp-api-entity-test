namespace workshop.wwwapi.DTO
{
    public class PatientGet
    {
        public string Name { get; set; }
        public ICollection<AppointmentDoctor> Appointments { get; set; } = new List<AppointmentDoctor>();
    }
}
