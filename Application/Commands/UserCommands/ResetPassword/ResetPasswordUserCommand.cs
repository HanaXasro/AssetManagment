using Application.Helper;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UserCommands.ResetPassword
{
    public record ResetPasswordUserCommand : IRequest<string>
    {
        [Required]
        public string Token { get; set; } = string.Empty;
        [Required]
        [MinLength(6),MaxLength(32)]
        public string Password { get; set; } = string.Empty;
        [Required]
        [Compare("Password")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
