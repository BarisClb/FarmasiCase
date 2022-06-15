using AutoMapper;
using FarmasiCase.Domain.Entities;
using FarmasiCase.Persistence.Models;
using FarmasiCase.Service.Dtos.Create;
using FarmasiCase.Service.Dtos.Update;
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
    public class ProductService
    {
        private readonly IMongoCollection<Product> _productsCollection;
        private readonly IMapper _mapper;

        public ProductService(IOptions<FarmasiCaseSettings> farmasiCaseSettings, IMapper mapper)
        {
            var mongoClient = new MongoClient(
                farmasiCaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                farmasiCaseSettings.Value.DatabaseName);

            _productsCollection = mongoDatabase.GetCollection<Product>(
                farmasiCaseSettings.Value.ProductsCollectionName);

            _mapper = mapper;
        }


        public async Task<List<Product>> Get()
        {
            return await _productsCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<Product> GetById(string productId)
        {
            Product product = await _productsCollection.Find(product => product.Id == productId).FirstOrDefaultAsync();

            if (product == null)
                throw new Exception("Product not found.");

            return product;
        }

        public async Task<ProductCreateDto> Create(ProductCreateDto newProductDto)
        {
            await _productsCollection.InsertOneAsync(_mapper.Map<ProductCreateDto, Product>(newProductDto));
            return newProductDto;
        }

        public async Task<Product> Update(ProductUpdateDto updatedProductDto)
        {
            Product product = await GetById(updatedProductDto.Id);

            if (updatedProductDto.Name != null)
                product.Name = updatedProductDto.Name;

            if (updatedProductDto.Description != null)
                product.Description = updatedProductDto.Description;

            if (updatedProductDto.Price != null)
                product.Price = (int)updatedProductDto.Price;

            await _productsCollection.ReplaceOneAsync(product => product.Id == updatedProductDto.Id, product);
            return product;
        }

        public async Task Delete(string productId)
        {
            Product product = await GetById(productId);

            await _productsCollection.DeleteOneAsync(product => product.Id == productId);
            return;
        }
    }
}
