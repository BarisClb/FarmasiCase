using AutoMapper;
using FarmasiCase.Domain.Entities;
using FarmasiCase.Service.Contracts;
using FarmasiCase.Service.Dtos.Account;
using FarmasiCase.Service.Dtos.Read;
using FarmasiCase.Service.RabbitMQ;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FarmasiCase.Service.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserService _userService;
        private readonly IJwtService _jwtService;
        private readonly IMapper _mapper;

        public AccountService(UserService userService, IJwtService jwtService, IMapper mapper)
        {
            _userService = userService;
            _jwtService = jwtService;
            _mapper = mapper;
        }


        public async Task<string> Login(AccountLoginDto accountLoginDto)
        {
            // We use the Helper method to bring the User with Login info, then use mapper to turn it into UserReadDto
            UserReadDto userReadDto = _mapper.Map<User, UserReadDto>(await LoginHelper(accountLoginDto.UserId, accountLoginDto.Password));

            // Generate Jwt and add it to response
            string jwt = await _jwtService.GenerateJwt(userReadDto.Id);


            await GenericActionMethod.SendMessageViaRabbitMQ("Login successful.", "AccountLoginExchange", "AccountQueue");
            return jwt;
        }

        public async Task Logout()
        {
            // We do the Logout actions inside controller but I added this here as a method, in case we change the process in the future.
            throw new Exception("You should not be able to see this.");
        }

        public async Task<UserReadDto> Verify(string jwt)
        {
            User user = await IdentifyJwt(jwt);
            return _mapper.Map<User, UserReadDto>(user);
        }


        // Helpers

        public async Task<User> LoginHelper(string accountId, string password)
        {
            User user = await _userService.GetById(accountId);

            if (user == null)
                throw new Exception("User does not exist.");

            // Check if Password is Incorrect
            if (password != user.Password)
                throw new Exception("Incorrect password.");


            return user;
        }

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
