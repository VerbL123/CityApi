//using CityApi.Models;
//using FirtsApi.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;
//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;

//namespace FirtsApi.Controllers
//{
//    [ApiController]
//    [Route("api/[controller]")]
//    public class AuthController : ControllerBase
//    {
//        private readonly UserManager<UserAuth> _userManager;
//        private readonly SignInManager<UserAuth> _signInManager;
//        private readonly IConfiguration _configuration;

//        public AuthController(UserManager<UserAuth> userManager, SignInManager<UserAuth> signInManager, IConfiguration configuration)
//        {
//            _userManager = userManager;
//            _signInManager = signInManager;
//            _configuration = configuration;
//        }

//        [HttpPost("login")]
//        public async Task<IActionResult> Login([FromBody] LoginModel model)
//        {
//            var user = await _userManager.FindByNameAsync(model.Username);
//            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
//            {
//                var authClaims = new List<Claim>
//                {
//                    new Claim(ClaimTypes.Name, user.UserName),
//                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                };

//                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

//                var token = new JwtSecurityToken(
//                    issuer: _configuration["Jwt:Issuer"],
//                    audience: _configuration["Jwt:Audience"],
//                    expires: DateTime.Now.AddHours(3),
//                    claims: authClaims,
//                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
//                );

//                return Ok(new
//                {
//                    token = new JwtSecurityTokenHandler().WriteToken(token),
//                    expiration = token.ValidTo
//                });
//            }

//            return Unauthorized();
//        }
//        [HttpPost("register")]
//        public async Task<IActionResult> Register([FromBody] LoginModel model)
//        {
//            if (ModelState.IsValid)
//            {
//                var user = new UserAuth { UserName = model.Username, Role = "user" };
//                var result = await _userManager.CreateAsync(user, model.Password);

//                if (result.Succeeded)
//                {
//                    // Sign in the registered user
//                    await _signInManager.SignInAsync(user, isPersistent: false);
//                    return Ok("User registered and signed in successfully.");
//                }

//                // Add errors to the ModelState
//                foreach (var error in result.Errors)
//                {
//                    ModelState.AddModelError(string.Empty, error.Description);
//                }
//            }

//            return BadRequest(ModelState);
//        }
//    }
//    public class LoginModel
//    {
//        public string Username { get; set; }
//        public string Password { get; set; }
//    }
//}

using CityApi.Models;
using FirtsApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Nodes;

namespace CityApi.Controllers
{
    [Controller]
    public class AuthController : Controller
    {
        private static List<User> UserList = new List<User>();
        private readonly AppSettings _applicationSettings;
        private readonly HttpClient _httpClient;

        public AuthController(IOptions<AppSettings> _applicationSettings, HttpClient httpClient)
        {
            this._applicationSettings = _applicationSettings.Value;
            _httpClient = httpClient;
        }


        [HttpPost("Login")]
        public IActionResult Login([FromBody] Login model)
        {
            var user = UserList.Where(x => x.UserName == model.UserName).FirstOrDefault();

            if (user == null)
            {
                return BadRequest("Username Or Password Was Invalid");
            }

            var match = CheckPassword(model.Password, user);

            if (!match)
            {
                return BadRequest("Username Or Password Was Invalid");
            }

            JWTGenerator(user);

            return Ok();

        }

        public dynamic JWTGenerator(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var secretKey = "your_secret_key_here";
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("id", user.UserName), new Claim(ClaimTypes.Role, user.Role) }),

                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encrypterToken = tokenHandler.WriteToken(token);

            SetJWT(encrypterToken);

            //var refreshToken = GenerateRefreshToken();

            //SetRefreshToken(refreshToken, user);

            return new { token = encrypterToken, username = user.UserName };
        }

        //private RefreshToken GenerateRefreshToken()
        //{
        //    var refreshToken = new RefreshToken()
        //    {
        //        Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
        //        Expires = DateTime.Now.AddDays(7),
        //        Created = DateTime.Now
        //    };

        //    return refreshToken;

        //}

        //[HttpGet("RefreshToken")]
        //public async Task<ActionResult<string>> RefreshToken()
        //{
        //    var refreshToken = Request.Cookies["X-Refresh-Token"];

        //    var user = UserList.Where(x => x.Token == refreshToken).FirstOrDefault();

        //    if (user == null || user.TokenExpires < DateTime.Now)
        //    {
        //        return Unauthorized("Token has expired");
        //    }

        //    JWTGenerator(user);

        //    return Ok();
        //}

        //public void SetRefreshToken(RefreshToken refreshToken, User user)
        //{

        //    HttpContext.Response.Cookies.Append("X-Refresh-Token", refreshToken.Token,
        //         new CookieOptions
        //         {
        //             Expires = refreshToken.Expires,
        //             HttpOnly = true,
        //             Secure = true,
        //             IsEssential = true,
        //             SameSite = SameSiteMode.None
        //         });

        //    UserList.Where(x => x.UserName == user.UserName).First().Token = refreshToken.Token;
        //    UserList.Where(x => x.UserName == user.UserName).First().TokenCreated = refreshToken.Created;
        //    UserList.Where(x => x.UserName == user.UserName).First().TokenExpires = refreshToken.Expires;
        //}

        public void SetJWT(string encrypterToken)
        {

            HttpContext.Response.Cookies.Append("X-Access-Token", encrypterToken,
                  new CookieOptions
                  {
                      Expires = DateTime.Now.AddMinutes(15),
                      HttpOnly = true,
                      Secure = true,
                      IsEssential = true,
                      SameSite = SameSiteMode.None
                  });
        }

        [HttpDelete("RevokeToken/{username}")]
        public async Task<IActionResult> RevokeToken(string username)
        {
            UserList.Where(x => x.UserName == username).First().Token = "";

            return Ok();
        }



        private bool CheckPassword(string password, User user)
        {
            bool result;

            using (HMACSHA512? hmac = new HMACSHA512(user.PasswordSalt))
            {
                var compute = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                result = compute.SequenceEqual(user.PasswordHash);
            }

            return result;
        }

        [HttpPost("Register")]
        public IActionResult Register([FromBody] Register model)
        {
            var user = new User { UserName = model.UserName, Role = model.Role };

            //if(model.ConfirmPassword == model.Password)
            //{
            using (HMACSHA512? hmac = new HMACSHA512())
            {
                user.PasswordSalt = hmac.Key;
                user.PasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(model.Password));
            }
            //}

            UserList.Add(user);

            return Ok(user);
        }
    }
}