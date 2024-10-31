using HealthCare.Appointments.API.Models;

namespace HealthCare.Appointments.API.Repositories
{
    public interface IAppointmentRepository
    {
        Task<Appointment> Get(string id);
        Task<List<Appointment>> GetAll();
        Task<Appointment> Add(Appointment appointment);
        Task<bool> Delete(string id);
        Task Update(Appointment appointment);
        Task<bool> Exists(string id);
    }
}
