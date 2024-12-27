using Application.Commands.AssetManagement.CategoryCommands.Create;
using Application.Commands.AssetManagement.CategoryCommands.Update;
using AutoMapper;
using Application.Commands.UserCommands.Register;
using Application.Dtos;
using Application.Dtos.AccountDtos;
using Application.Dtos.AssetDtos;
using Domain.Entities;
using Domain.Entities.UserEntity;
using Domain.Entities.Inventory;

namespace Application.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
           
            CreateMap(typeof(Pagination<>), typeof(PaginationDto<>));
            
            CreateMap<RegisterUserCommand, User>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
            
            CreateMap<CategoryC, Category>().ReverseMap();
            CreateMap<CategoryU, Category>().ReverseMap();
            CreateMap< Category , CategoryDto>().ReverseMap();
            CreateMap< Category , CategoryListDto>().ReverseMap();
        }
    }
}
