using Application.Dtos.AccountDtos;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Commands.UserCommands.RefreshToken;

public record RefreshTokenUserCommand : IRequest<LoginReqponseDto>
{
    [Required] public string Token { get; set; } = string.Empty;
}