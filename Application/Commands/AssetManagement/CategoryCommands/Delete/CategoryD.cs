using Application.Dtos.AssetDtos;
using MediatR;

namespace Application.Commands.AssetManagement.CategoryCommands.Delete;

public record CategoryD(long Id) : IRequest<CategoryDto>;