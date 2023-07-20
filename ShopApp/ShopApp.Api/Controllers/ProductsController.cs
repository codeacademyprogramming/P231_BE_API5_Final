using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopApi.Data;
using ShopApp.Service.Dtos.ProductDtos;
using ShopApp.Core.Entities;
using ShopApp.Core.Repositories;
using ShopApp.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using ShopApp.Service.Dtos.Common;

namespace ShopApp.Api.Controllers
{
    //[Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

      
        [HttpPost("")]
        public IActionResult Create([FromForm] ProductCreateDto productDto)
        {
            return StatusCode(201,_productService.Create(productDto));
        }

        /// <summary>
        /// get a product by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public ActionResult<ProductGetDto> Get(int id)
        {
            return Ok(_productService.GetById(id));
        }
        [HttpGet("all")]
        public ActionResult<List<ProductGetAllItemDto>> GetAll()
        {
           return Ok(_productService.GetAll());
        }

        [HttpGet("")]
        public ActionResult<PaginatedListDto<ProductGetPaginatedListItemDto>> GetAll(int page = 1)
        {
            return Ok(_productService.GetAllPaginated(page));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="productDto"></param>
        /// <returns></returns>
        /// <response code="204">The product has updated successfuly</response>
        /// <response code="400">Model is not correct format</response>
        /// <response code="404">Product not found by id</response>
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult Edit(int id,[FromForm] ProductEditDto productDto)
        {
            _productService.Edit(id, productDto);

            return NoContent(); 
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _productService.Delete(id);
            return NoContent();
        }
    }
}
