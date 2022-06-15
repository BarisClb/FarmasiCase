using AutoMapper;
using FarmasiCase.Domain.Entities;
using FarmasiCase.Service.Dtos.Create;
using FarmasiCase.Service.Dtos.Read;
using FarmasiCase.Service.Dtos.Update;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmasiCase.Infrastructure.Automapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            // When Creating
            CreateMap<UserCreateDto, User>().ReverseMap();

            // When Updating
            CreateMap<UserUpdateDto, User>().ReverseMap();

            // When Listing/Reading
            CreateMap<User, UserReadDto>().ReverseMap();
        }
    }
}
