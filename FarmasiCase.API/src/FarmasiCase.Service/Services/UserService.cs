﻿using AutoMapper;
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
    public class UserService
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IMapper _mapper;

        public UserService(IOptions<FarmasiCaseSettings> farmasiCaseSettings, IMapper mapper)
        {
            var mongoClient = new MongoClient(
                farmasiCaseSettings.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(
                farmasiCaseSettings.Value.DatabaseName);

            _usersCollection = mongoDatabase.GetCollection<User>(
                farmasiCaseSettings.Value.UsersCollectionName);

            _mapper = mapper;
        }


        public async Task<List<User>> Get()
        {
            return await _usersCollection.Find(new BsonDocument()).ToListAsync();
        }

        public async Task<User> GetById(string userId)
        {
            User user = await _usersCollection.Find(user => user.Id == userId).FirstOrDefaultAsync();

            if (user == null)
                throw new Exception("User not found.");

            return user;
        }

        public async Task<UserCreateDto> Create(UserCreateDto newUserDto)
        {
            await _usersCollection.InsertOneAsync(_mapper.Map<UserCreateDto, User>(newUserDto));
            return newUserDto;
        }

        public async Task<User> Update(UserUpdateDto updatedUserDto)
        {
            User user = await GetById(updatedUserDto.Id);

            if (updatedUserDto.Name != null)
                user.Name = updatedUserDto.Name;

            if (updatedUserDto.Username != null)
                user.Username = updatedUserDto.Username;

            if (updatedUserDto.Password != null)
                user.Password = updatedUserDto.Password;

            await _usersCollection.ReplaceOneAsync(user => user.Id == updatedUserDto.Id, user);
            return user;
        }

        public async Task Delete(string userId)
        {
            User user = await GetById(userId);

            await _usersCollection.DeleteOneAsync(user => user.Id == userId);
            return;
        }
    }
}