using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.UserCommands.ForgotPassword;

public record ForgotPasswordUserCommand : IRequest<string>
{
    [Required] public string Email { get; set; } = string.Empty;
}