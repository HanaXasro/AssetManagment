using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories.UserRepositores
{
    public interface IUserReference
    {
        public Guid UserId { get; }
    }
}
