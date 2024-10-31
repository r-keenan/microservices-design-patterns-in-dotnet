namespace HealthCare.SharedAssets.Messages
{
    public class AppointmentMessage
    {
        public Guid Id { get; set; }
        public Guid CustomerId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime Date { get; set; }
    }
}
