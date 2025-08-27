namespace workshop.wwwapi.DTO
{
    public class PatientGet
    {
        public string? FullName { get; set; }
        public ICollection<AppointmentDoctor> Appointments { get; set; } = new List<AppointmentDoctor>();
    }
}
