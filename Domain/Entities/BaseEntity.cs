using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BaseEntity
    {
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime? Updated { get; set; } 
    }
}
