namespace workshop.wwwapi.DTO
{
    public class AppointmentGet
    {
        public DateTime booktime {  get; set; }
        public int DoctorId { get; set; }
        public string DoctorName { get; set; }
        public int PatientId { get; set; }
        public string PatientName { get; set; }
    }
}
