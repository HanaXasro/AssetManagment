using MediatR;

namespace Application.Commands.UserCommands.ResendCodeToRest;

public record ResendCodeToRestPasswordC(string Email) : IRequest<string>;