using HealthCare.Appointments.API.Models;

namespace HealthCare.Appointments.API.Services
{
    public class PatientsApiRepository : HttpRepository<Patient>, IPatientsApiRepository
    {
        public PatientsApiRepository(HttpClient client)
            : base(client) { }
    }
}
