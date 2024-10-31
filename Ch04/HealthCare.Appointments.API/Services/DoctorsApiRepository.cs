using HealthCare.Appointments.API.Models;

namespace HealthCare.Appointments.API.Services
{
    public class DoctorsApiRepository : HttpRepository<Doctor>, IDoctorsApiRepository
    {
        public DoctorsApiRepository(HttpClient client)
            : base(client) { }
    }
}
