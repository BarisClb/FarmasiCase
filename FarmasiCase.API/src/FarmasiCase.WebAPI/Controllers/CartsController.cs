﻿using FarmasiCase.Domain.Entities;
using FarmasiCase.Service.Dtos.Redis;
using FarmasiCase.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarmasiCase.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartsController : ControllerBase
    {
        private readonly CartService _cartService;

        public CartsController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet]
        public async Task<List<ProductRedisDto>> Get()
        {
            return await _cartService.GetCart();
        }

        [HttpPost("AddProductToCart/{productId}")]
        public async Task<IActionResult> Post(string productId)
        {
            await _cartService.AddProductToCart(productId);
            return Ok("Product added to cart.");
        }

        [HttpPost("ReduceProductToCart/{productId}")]
        public async Task<IActionResult> Put(string productId)
        {
            await _cartService.ReduceProductFromCart(productId);
            return Ok("Cart updated.");
        }

        [HttpDelete("RemoveProductFromCart/{productId}")]
        public async Task<IActionResult> Delete(string productId)
        {
            await _cartService.RemoveProductFromCart(productId);
            return Ok("Product removed from Cart.");
        }
    }
}
