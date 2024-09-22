using Application.Helper;
using MediatR;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UserCommands.VerifyEmail
{
    public record VerifyEmailUserCommand(string Token) : IRequest<string>;
}
