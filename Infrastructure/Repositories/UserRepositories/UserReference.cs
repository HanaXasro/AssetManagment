using Domain.Entities.UserEntity;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Repositories.UserRepositories;

namespace Infrastructure.Repositories.UserRepositories
{
    public class UserReference(IHttpContextAccessor httpContextAccessor) : IUserReference
    {
        public Guid UserId => ((User)httpContextAccessor.HttpContext!.Items["User"]!).UserId;
    }
}
