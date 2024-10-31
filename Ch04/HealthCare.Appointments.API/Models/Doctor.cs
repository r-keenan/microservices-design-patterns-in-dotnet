namespace HealthCare.Appointments.API.Models
{
    public class Doctor : BaseEntity<Guid>, IAggregateRoot
    {
        public Doctor(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public string Name { get; private set; }

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}
