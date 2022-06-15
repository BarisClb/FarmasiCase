using AutoMapper;
using FarmasiCase.Domain.Entities;
using FarmasiCase.Persistence.Models;
using FarmasiCase.Service.Dtos.Create;
using FarmasiCase.Service.Dtos.Update;
using FarmasiCase.Service.RabbitMQ;
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
            var list = await _productsCollection.Find(new BsonDocument()).ToListAsync();
            await GenericActionMethod.SendMessageViaRabbitMQ("GetProductList successful.", "GetProductListExchange", "ProductQueue");
            return list;
        }

        public async Task<Product> GetById(string productId)
        {
            Product product = await _productsCollection.Find(product => product.Id == productId).FirstOrDefaultAsync();

            if (product == null)
                throw new Exception("Product not found.");

            await GenericActionMethod.SendMessageViaRabbitMQ("GetProduct successful.", "GetProductByIdExchange", "ProductQueue");
            return product;
        }

        public async Task<ProductCreateDto> Create(ProductCreateDto newProductDto)
        {
            await _productsCollection.InsertOneAsync(_mapper.Map<ProductCreateDto, Product>(newProductDto));

            await GenericActionMethod.SendMessageViaRabbitMQ("CreateProduct successful.", "CreateProductExchange", "ProductQueue");
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

            await GenericActionMethod.SendMessageViaRabbitMQ("UpdateProduct successful.", "UpdateProductExchange", "ProductQueue");
            return product;
        }

        public async Task Delete(string productId)
        {
            Product product = await GetById(productId);

            await _productsCollection.DeleteOneAsync(product => product.Id == productId);
            await GenericActionMethod.SendMessageViaRabbitMQ("DeleteProduct successful.", "DeleteProductExchange","ProductQueue");
            return;
        }
    }
}
