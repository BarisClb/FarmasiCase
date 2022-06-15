using FarmasiCase.Service.Dtos.Create;
using FarmasiCase.Service.Dtos.Update;
using FarmasiCase.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarmasiCase.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _userService.Get());
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserCreateDto newUserDto)
        {
            return Ok(await _userService.Create(newUserDto));
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> Put(UserUpdateDto userUpdateDto)
        {
            return Ok(await _userService.Update(userUpdateDto));
        }

        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            await _userService.Delete(userId);

            return Ok("User deleted.");
        }
    }
}
