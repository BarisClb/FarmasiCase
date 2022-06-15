using FarmasiCase.Service.Dtos.Account;
using FarmasiCase.Service.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FarmasiCase.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly AccountService _accountService;

        public AccountsController(AccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpPost("Login")]
        public async Task<IActionResult> Login(AccountLoginDto accountLoginDto)
        {
            string jwt = await _accountService.Login(accountLoginDto);

            // We need to add options if we want to Append the Cookie while working with LocalHost. If we don't, it doesn't hold the Cookie.
            // HttpOnly prevents Client side scripts from accessing the data. 
            Response.Cookies.Append($"jwtUser", jwt, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
            });


            return Ok(new { success = true, message = $"User login successful." });
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            string? jwt = Request.Cookies[$"jwtUser"];
            if (jwt == null)
                return Ok(new { success = false, message = "You are not logged in." });

            Response.Cookies.Delete($"jwtUser", new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.None,
                Secure = true,
            });


            return Ok(new { success = true, message = $"User logout successful." });
        }

        [HttpGet("Verify")]
        public async Task<IActionResult> Verify()
        {
            string? jwt = Request.Cookies[$"jwtUser"];
            if (jwt == null)
                return Ok(new { success = false, message = "You are not logged in." });

            return Ok(await _accountService.Verify(jwt));
        }
    }
}
