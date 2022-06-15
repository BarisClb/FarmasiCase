using AutoMapper;
using FarmasiCase.Domain.Entities;
using FarmasiCase.Service.Dtos.Create;
using FarmasiCase.Service.Dtos.Read;
using FarmasiCase.Service.Dtos.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmasiCase.Infrastructure.Automapper
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // When Creating
            CreateMap<OrderCreateDto<ProductRedisDto>, Order<ProductRedisDto>>().ReverseMap();

            // When Listing/Reading
            CreateMap<Order<ProductRedisDto>, OrderReadDto<ProductRedisDto>>().ReverseMap();
        }
    }
}
