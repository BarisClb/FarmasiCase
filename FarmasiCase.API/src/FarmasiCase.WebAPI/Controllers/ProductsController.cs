using AutoMapper;
using FarmasiCase.Domain.Entities;
using FarmasiCase.Service.Dtos.Create;
using FarmasiCase.Service.Dtos.Update;
using FarmasiCase.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarmasiCase.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(ProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _productService.Get());
        }

        [HttpPost]
        public async Task<IActionResult> Post(ProductCreateDto newProductDto)
        {
            return Ok(await _productService.Create(newProductDto));
        }

        [HttpPut("{productId}")]
        public async Task<IActionResult> Put(ProductUpdateDto productUpdateDto)
        {
            return Ok(await _productService.Update(productUpdateDto));
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(string productId)
        {
            await _productService.Delete(productId);

            return Ok("Product deleted.");
        }
    }
}
