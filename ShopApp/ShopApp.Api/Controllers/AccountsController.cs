using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ShopApp.Api.Services;
using ShopApp.Core.Entities;
using ShopApp.Service.Dtos.AccountDtos;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace ShopApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly JwtService _jwtService;

        public AccountsController(RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager, IConfiguration configuration, JwtService jwtService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
            _jwtService = jwtService;
        }
        //[HttpGet("createrole")]
        //public async Task<IActionResult> CreateRole()
        //{
        //    await _roleManager.CreateAsync(new IdentityRole("Admin"));
        //    await _roleManager.CreateAsync(new IdentityRole("Member"));

        //    return Ok();
        //}

        //[HttpGet("createadmin")]
        //public async Task<IActionResult> CreateAdmin()
        //{
        //    AppUser admin = new AppUser
        //    {
        //        Email = "admin@gmail.com",
        //        FullName = "Hikmet Abbasov",
        //        UserName = "hikmet_admin"
        //    };

        //    var result = await _userManager.CreateAsync(admin, "Admin123");
        //    await _userManager.AddToRoleAsync(admin, "Admin");

        //    return Ok(result);
        //}


        [HttpPost("login")]
        public async Task<IActionResult> Login(AdminLoginDto loginDto)
        {
            AppUser admin = await _userManager.FindByNameAsync(loginDto.UserName);

            if(admin == null)
                return BadRequest();

            if (!await _userManager.CheckPasswordAsync(admin, loginDto.Password))
                return BadRequest();

          
            return Ok(new {token=await _jwtService.GenerateToken(admin)});
        }

        [Authorize]
        [HttpGet("profile")]
        public async Task<IActionResult> Profile()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            return Ok(new
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName,
                FullName = user.FullName
            });
        }
    }
}
