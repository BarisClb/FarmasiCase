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
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // When Creating
            CreateMap<ProductCreateDto, Product>().ReverseMap();

            // When Updating
            CreateMap<ProductUpdateDto, Product>().ReverseMap();

            // When Listing/Reading
            CreateMap<Product, ProductReadDto>().ReverseMap();
        }
    }
}
