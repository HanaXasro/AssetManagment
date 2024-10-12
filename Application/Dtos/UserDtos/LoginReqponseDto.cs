using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dtos.AccountDtos
{
    public class LoginReqponseDto
    {
        public UserDto? user { get; set; }
        public string? Token { get; set; }
        public string? RefreshToken { get; set; }

        public DateTime Expired { get; set; }
    }
}
