using AutoMapper;
using FarmasiCase.Domain.Entities;
using FarmasiCase.Persistence.Models;
using FarmasiCase.Service.Contracts;
using FarmasiCase.Service.Dtos.Create;
using FarmasiCase.Service.Dtos.Read;
using FarmasiCase.Service.Dtos.Redis;
using FarmasiCase.Service.RabbitMQ;
using FarmasiCase.Service.Redis;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmasiCase.Service.Services
{
    public class OrderService
    {
        private readonly IMongoCollection<Order<ProductRedisDto>> _ordersCollection;
        private readonly IJwtService _jwtService;
        private readonly UserService _userService;
        private readonly IDistributedCache _cache;
        private readonly IMapper _mapper;
        private readonly CartService _cartService;

        public OrderService(
            IOptions<FarmasiCaseSettings> farmasiCaseSettings, IJwtService jwtService, UserService userService, IDistributedCache cache, IMapper mapper, CartService cartService)
        {
            var mongoClient = new MongoClient(
                farmasiCaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                farmasiCaseSettings.Value.DatabaseName);

            _ordersCollection = mongoDatabase.GetCollection<Order<ProductRedisDto>>(
                farmasiCaseSettings.Value.OrdersCollectionName);

            _jwtService = jwtService;
            _userService = userService;
            _cache = cache;
            _mapper = mapper;
            _cartService = cartService;
        }


        public async Task<List<Order<ProductRedisDto>>> Get()
        {
            // Interrupting return to SendMessage
            var list = await _ordersCollection.Find(new BsonDocument()).ToListAsync();
            await GenericActionMethod.SendMessageViaRabbitMQ("GetOrderList successful.", "GetOrderListExchange", "OrderQueue");
            return list;
        }

        public async Task<Order<ProductRedisDto>> GetById(string orderId)
        {
            var order = await _ordersCollection.Find(order => order.Id == orderId).FirstOrDefaultAsync();
            await GenericActionMethod.SendMessageViaRabbitMQ("GetOrderById successful.", "GetOrderByIdExchange", "OrderQueue");
            return order;
        }


        public async Task Create(string jwt)
        {
            User user = await IdentifyJwt(jwt);

            // Bring the Cache
            string recordKey = "cartItems_";
            var cache = await _cache.GetRecordAsync<List<ProductRedisDto>>(recordKey);

            // If the Cache doesn't exist, create one with the Product
            if (cache == null)
                throw new Exception("Cache does not exist.");

            OrderCreateDto<ProductRedisDto> newOrder = new()
            {
                User = user.Name,
                Status = "Created",
                Items = cache
            };


            await _ordersCollection.InsertOneAsync(_mapper.Map<OrderCreateDto<ProductRedisDto>, Order<ProductRedisDto>>(newOrder));

            // Clear the cart
            await _cartService.ClearCart();

            await GenericActionMethod.SendMessageViaRabbitMQ("CreateOrder successful.", "CreateOrderExchange", "OrderQueue");
            return;
        }

        public async Task UpdateAsync(string orderId, string status)
        {
            Order<ProductRedisDto> order = await _ordersCollection.Find(order => order.Id == orderId).FirstOrDefaultAsync();
            order.Status = status;
            await _ordersCollection.ReplaceOneAsync(order => order.Id == orderId, order);

            await GenericActionMethod.SendMessageViaRabbitMQ("UpdateOrder successful.", "UpdateOrderExchange","OrderQueue");
            return;
        }

        public async Task DeleteAsync(string orderId)
        {
            await _ordersCollection.DeleteOneAsync(order => order.Id == orderId);

            await GenericActionMethod.SendMessageViaRabbitMQ("DeleteOrder successful.", "DeleteOrderExchange", "OrderQueue");
            return;
        }


        // Helpers

        public async Task<User> IdentifyJwt(string? jwt)
        {
            if (jwt == null)
                throw new Exception("Jwt does not exist.");

            JwtSecurityToken token = await _jwtService.Verify(jwt);
            User user = await _userService.GetById(token.Issuer);

            if (user == null)
                throw new Exception("User does not exist.");


            return user;
        }
    }
}
