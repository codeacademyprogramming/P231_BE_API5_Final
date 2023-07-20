using AutoMapper;
using Microsoft.AspNetCore.Http;
using ShopApp.Core.Entities;
using ShopApp.Core.Repositories;
using ShopApp.Service.Dtos.Common;
using ShopApp.Service.Dtos.ProductDtos;
using ShopApp.Service.Exceptions;
using ShopApp.Service.Helpers;
using ShopApp.Service.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopApp.Service.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IBrandRepository _brandRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private string _rootPath;

        public ProductService(IBrandRepository brandRepository,IProductRepository productRepository,IMapper mapper)
        {
            _brandRepository = brandRepository;
            _productRepository = productRepository;
            _mapper = mapper;
            _rootPath = Directory.GetCurrentDirectory()+"/wwwroot";
        }
        public CreatedResultDto Create(ProductCreateDto dto)
        {
            if (!_brandRepository.IsExist(x => x.Id == dto.BrandId))
                throw new RestException(System.Net.HttpStatusCode.BadRequest, "BrandId", $"Brand not found by id: {dto.BrandId}");

            if(_productRepository.IsExist(x=>x.Name == dto.Name))
                throw new RestException(System.Net.HttpStatusCode.BadRequest, "Name", $"Name already taken");

            var entity = _mapper.Map<Product>(dto);

            entity.ImageName = FileManager.Save(dto.ImageFile, _rootPath, "uploads/products");

            _productRepository.Add(entity);
            _productRepository.Commit();

            return new CreatedResultDto { Id = entity.Id };
        }

        public void Delete(int id)
        {
            var entity = _productRepository.Get(x => x.Id == id);

            if (entity == null)
                throw new RestException(System.Net.HttpStatusCode.NotFound, $"Product not found by id: {id}");
            
            _productRepository.Remove(entity);
            _productRepository.Commit();
        }

        public void Edit(int id, ProductEditDto dto)
        {
            var entity = _productRepository.Get(x=>x.Id == id);

            if (entity == null)
                throw new RestException(System.Net.HttpStatusCode.NotFound, $"Product not found by id: {id}");

            if (entity.BrandId != dto.BrandId && !_brandRepository.IsExist(x => x.Id == dto.BrandId))
                throw new RestException(System.Net.HttpStatusCode.BadRequest, "BrandId", "BrandId not found");

            if (entity.Name != dto.Name && _productRepository.IsExist(x => x.Name == dto.Name))
                throw new RestException(System.Net.HttpStatusCode.BadRequest, "Name", "Name already taken");

            entity.Name = dto.Name;
            entity.CostPrice = dto.CostPrice;
            entity.SalePrice= dto.SalePrice;
            entity.BrandId = dto.BrandId;
            entity.ModifiedAt = DateTime.UtcNow;
            string existImageName = null;

            if (dto.ImageFile != null)
            {
                existImageName = entity.ImageName;
                entity.ImageName = FileManager.Save(dto.ImageFile, _rootPath, "uploads/products");
            }

            _productRepository.Commit();

            if(existImageName!=null)
                FileManager.Delete(_rootPath, "uploads/products", existImageName);
        }

        public List<ProductGetAllItemDto> GetAll()
        {
            var entities = _productRepository.GetQueryable(x => true, "Brand").ToList();

            return _mapper.Map<List<ProductGetAllItemDto>>(entities);
        }

        public PaginatedListDto<ProductGetPaginatedListItemDto> GetAllPaginated(int page)
        {
            var query = _productRepository.GetQueryable(x => true, "Brand");
            var entities = query.Skip((page - 1) * 4).Take(4).ToList();
            var items = _mapper.Map<List<ProductGetPaginatedListItemDto>>(entities);
            return new PaginatedListDto<ProductGetPaginatedListItemDto>(items,page,4,query.Count());
        }

        public ProductGetDto GetById(int id)
        {
            var entity = _productRepository.Get(x => x.Id == id, "Brand");

            if (entity == null)
                throw new RestException(System.Net.HttpStatusCode.NotFound, $"Product not found by id: {id}");

            return _mapper.Map<ProductGetDto>(entity); ;
        }

    }
}
