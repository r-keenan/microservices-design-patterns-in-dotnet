namespace HealthCare.Appointments.API.Dtos
{
    public class AppointmentDetailsDto
    {
        public PatientDto Patient { get; set; }
        public Guid PatientId { get; set; }
        public DoctorDto Doctor { get; set; }
        public Guid DoctorId { get; set; }
        public int AppointmentTypeId { get; set; }

        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string Title { get; set; }
        public DateTime? DateTimeConfirmed { get; set; }
    }
}
