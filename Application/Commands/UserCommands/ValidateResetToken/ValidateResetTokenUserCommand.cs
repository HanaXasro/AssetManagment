using Application.Helper;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Commands.UserCommands.ValidateResetToken
{
    public record ValidateResetTokenUserCommand(string token) : IRequest<string>;
}
