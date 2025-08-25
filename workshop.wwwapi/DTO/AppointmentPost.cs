namespace workshop.wwwapi.DTO
{
    public class AppointmentPost
    {
        public int patientId {  get; set; }
        public int doctorId { get; set; }
        public DateTime booking { get; set; }
    }
}
