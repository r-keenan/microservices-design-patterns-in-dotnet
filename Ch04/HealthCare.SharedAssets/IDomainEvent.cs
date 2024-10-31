using MediatR;

namespace HealthCare.Appointments.API.Models
{
    public interface IDomainEvent : INotification
    {
        public interface IDomainEvent : INotification
        {
            DateTime ActionDate { get; }
        }
    }
}
