using Application.Helper;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Application.Commands.UserCommands.RevokeToken
{
    public record RevokeTokenUserCommand : IRequest<string>
    {
        [Required]
        public string Token { get; set; } = string.Empty;
    }
}
