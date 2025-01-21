using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class BaseEntity
    {
        public bool IsEnable { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
