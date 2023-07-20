using AutoMapper;
using Microsoft.AspNetCore.Http;
using ShopApp.Core.Entities;
using ShopApp.Service.Dtos.BrandDtos;
using ShopApp.Service.Dtos.ProductDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Service.Profiles
{
    public class MappingProfile:Profile
    {
        public MappingProfile(IHttpContextAccessor _httpContextAccessor)
        {
            var baseUrl = new UriBuilder(_httpContextAccessor.HttpContext.Request.Scheme, _httpContextAccessor.HttpContext.Request.Host.Host, _httpContextAccessor.HttpContext.Request.Host.Port ?? -1);

            CreateMap<BrandCreateDto, Brand>();
            CreateMap<Brand, BrandGetDto>();
            CreateMap<Brand,BrandInProductGetDto>();
            CreateMap<Brand,BrandGetAllItemDto>();

            CreateMap<ProductCreateDto, Product>();
            CreateMap<Product, ProductGetDto>()
                .ForMember(d => d.Profit, s => s.MapFrom(m => m.SalePrice - m.CostPrice))
                .ForMember(d => d.ImageUrl, s => s.MapFrom(m => baseUrl + "uploads/products/" + m.ImageName));

            CreateMap<Product, ProductGetAllItemDto>()
                .ForMember(d => d.ImageUrl, s => s.MapFrom(m => string.IsNullOrWhiteSpace(m.ImageName)?null:( baseUrl + "uploads/products/" + m.ImageName)));

            CreateMap<Product, ProductGetPaginatedListItemDto>();
        }
    }
}
