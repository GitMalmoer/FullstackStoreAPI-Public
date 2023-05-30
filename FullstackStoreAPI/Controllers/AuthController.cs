using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using FullstackStoreAPI.Data;
using FullstackStoreAPI.Models;
using FullstackStoreAPI.Models.DTO;
using FullstackStoreAPI.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Exception = System.Exception;


namespace FullstackStoreAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _dbContext;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private string _secretKey;
        private ApiResponse _apiResponse;

        public AuthController(AppDbContext dbContext, IConfiguration configuration, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
            _apiResponse = new ApiResponse();
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO model)
        {
            ApplicationUser userFromDb = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName.ToLower() == model.UserName.ToLower());

            if (userFromDb != null)
            {
                _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                _apiResponse.isSuccess = false;
                _apiResponse.ErrorMessages.Add("User name allready exists");
                return BadRequest(_apiResponse);
            }

            ApplicationUser newUser = new ApplicationUser()
            {
                UserName = model.UserName,
                Email = model.UserName,
                Name = model.Name,
                NormalizedEmail = model.UserName.ToUpper(),
            };
            try
            {
                var result = await _userManager.CreateAsync(newUser, model.Password);

                if (result.Succeeded)
                {
                    if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
                    {
                        await _roleManager.CreateAsync(new(SD.Role_Admin));
                        await _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer));
                    }

                    if (model.Role.ToLower() == SD.Role_Admin)
                    {
                        await _userManager.AddToRoleAsync(newUser, SD.Role_Admin);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(newUser, SD.Role_Customer);
                    }

                    _apiResponse.HttpStatusCode = HttpStatusCode.OK;
                    _apiResponse.isSuccess = true;
                    return Ok(_apiResponse);
                }
            }
            catch (Exception ex)
            {
                _apiResponse.ErrorMessages.Add("Error while registering");
                _apiResponse.ErrorMessages.Add(ex.ToString());
                _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                _apiResponse.isSuccess = false;
                return BadRequest(_apiResponse);
            }

            _apiResponse.ErrorMessages.Add("Error while registering");
            _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
            _apiResponse.isSuccess = false;
            return BadRequest(_apiResponse);
        }

        [HttpPost("Login")]

        public async Task<IActionResult> Login([FromBody]LoginRequestDTO model )
        {
            ApplicationUser user = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(u => u.UserName == model.UserName);

            if (user == null)
            {
                _apiResponse.isSuccess = false;
                _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add("Password or username is invalid");
                return BadRequest(_apiResponse);
            }

            var isValid = await _userManager.CheckPasswordAsync(user, model.Password);

            if (isValid == false)
            {
                _apiResponse.Result = new LoginResponseDTO();
                _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                _apiResponse.isSuccess = false;
                _apiResponse.ErrorMessages.Add("Password or username is invalid");
                return BadRequest(_apiResponse);
            }

            // GENERATE JWT
            var roles = await _userManager.GetRolesAsync(user);
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            byte[] key = Encoding.ASCII.GetBytes(_secretKey);

            SecurityTokenDescriptor tokenDescriptor = new()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("id",user.Id.ToString()),
                    new Claim("fullName",user.Name),
                    new Claim(ClaimTypes.Email, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role,roles.FirstOrDefault())
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken securityToken = tokenHandler.CreateToken(tokenDescriptor);


            LoginResponseDTO loginResponse = new LoginResponseDTO()
            {
                Email = model.UserName,
                Token = tokenHandler.WriteToken(securityToken),
            };

            if (loginResponse.Email == null || string.IsNullOrEmpty(loginResponse.Token))
            {
                _apiResponse.HttpStatusCode = HttpStatusCode.BadRequest;
                _apiResponse.ErrorMessages.Add("username or password is incorrect");
                _apiResponse.isSuccess = false;
                return BadRequest(_apiResponse);
            }

            _apiResponse.isSuccess = true;
            _apiResponse.HttpStatusCode = HttpStatusCode.OK;
            _apiResponse.Result = loginResponse;
            return Ok(_apiResponse);



        }

    }
}
