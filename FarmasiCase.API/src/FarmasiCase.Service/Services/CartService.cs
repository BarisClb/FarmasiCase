using FarmasiCase.Domain.Entities;
using FarmasiCase.Persistence.Models;
using FarmasiCase.Service.Dtos.Redis;
using FarmasiCase.Service.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmasiCase.Service.Services
{
    public class CartService
    {
        private readonly IDistributedCache _cache;
        private readonly ProductService _productService;
        public CartService(IDistributedCache cache, ProductService productService)
        {
            _cache = cache;
            _productService = productService;
        }

        private readonly string recordKey = "cartItems_";

        public async Task<List<ProductRedisDto>> GetCart()
        {
            // Bring the Cache
            var cache = await _cache.GetRecordAsync<List<ProductRedisDto>>(recordKey);

            // If the Cache doesn't exist, create one with the Product
            if (cache == null)
                throw new Exception("Cart is empty.");

            return cache;
        }

        public async Task AddProductToCart(string productId)
        {
            // Bring the Cache
            var cache = await _cache.GetRecordAsync<List<ProductRedisDto>>(recordKey);

            // If the Cache doesn't exist, create one with the Product
            if (cache == null)
            {
                var product = await _productService.GetById(productId);
                // Need mapping here
                ProductRedisDto newProductRedisDto = new()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1
                };
                List<ProductRedisDto> newCartList = new()
                {
                    newProductRedisDto
                };
                await _cache.SetRecordAsync(recordKey, newCartList);
                return;
            }

            // Find the item inside Cache list
            var productToAdd = cache.Find(product => product.Id == productId);

            // If the item doesn't exist, Add it
            if (productToAdd == null)
            {
                var product = await _productService.GetById(productId);
                // Need mapping here
                ProductRedisDto newProductRedisDto = new()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    Quantity = 1
                };
                cache.Add(newProductRedisDto);
                await _cache.SetRecordAsync(recordKey, cache);
                return;
            }

            // If it exist, add 1 to quantity
            productToAdd.Quantity += 1;
            await _cache.SetRecordAsync(recordKey, cache);
            return;
        }

        public async Task ReduceProductFromCart(string productId)
        {
            // Bring the Cache
            var cache = await _cache.GetRecordAsync<List<ProductRedisDto>>(recordKey);

            // If cache doesn't exist
            if (cache == null)
                throw new Exception("Cart is Empty");

            // Find the item inside Cache list
            var productToAdd = cache.Find(product => product.Id == productId);
            if (productToAdd == null)
                throw new Exception("Product is not in the cart.");


            // If it exist, reduce 1 from quantity, if it hits 0, remove from the list
            if (productToAdd.Quantity == 1)
                cache.RemoveAll(product => product.Id == productId);
            else
                productToAdd.Quantity -= 1;


            await _cache.SetRecordAsync(recordKey, cache);
            return;
        }

        public async Task RemoveProductFromCart(string productId)
        {
            // Bring the Cache
            var cache = await _cache.GetRecordAsync<List<ProductRedisDto>>(recordKey);

            // If cache doesn't exist
            if (cache == null)
                throw new Exception("Cart is Empty");

            // Find the item inside Cache list
            var productToAdd = cache.Find(product => product.Id == productId);
            if (productToAdd == null)
                throw new Exception("Product is not in the cart.");

            // If the item is in the cart, remove it
            cache.RemoveAll(product => product.Id == productId);


            await _cache.SetRecordAsync(recordKey, cache);
            return;
        }

        public async Task ClearCart()
        {
            await _cache.SetRecordAsync<ProductRedisDto>(recordKey, null);
            return;
        }
    }
}
