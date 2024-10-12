using AutoMapper;
using Application.Commands.UserCommands.Register;
using Application.Dtos.AccountDtos;
using Domain.Entities.UserEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegisterUserCommand, User>().ReverseMap();
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
