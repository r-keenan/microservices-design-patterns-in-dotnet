using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HealthCare.Appointments.API.Models
{
    public abstract class BaseEntity<TId>
    {
        public TId Id { get; set; }
    }
}
