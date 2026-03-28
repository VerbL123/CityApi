//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Rendering;
//using Microsoft.EntityFrameworkCore;
//using CityApi.Data;
//using FirtsApi.Models;
//using CityApi.Dtos;
//using Nest;
//using AutoMapper;
//using System.Data.Entity;

//namespace CityApi.Controllers
//{
//    [Route("app/[controller]")]
//    [ApiController]
//    public class UsersController : ControllerBase
//    {
//        private readonly CityDBContext _context;
//        private readonly IMapper _mapper;

//        public UsersController(CityDBContext context, IMapper mapper)
//        {
//            _context = context;
//            _mapper = mapper;
//        }

//        // GET: Users

//        [HttpGet]
//        public async Task<ActionResult<List<UserDtos>>> GetUser()
//        {
//            var userg = await _context.Users.ToListAsync();
//            var userdq = _mapper.Map<List<UserDtos>>(userg);
//            return userdq;
//        }


//        [HttpPost]
//        public async Task<ActionResult<User>> PostUser(UserDtos UserCreateDto)
//        {
//            var userp = _mapper.Map<User>(UserCreateDto);
//            _context.Users.Add(userp);

//            await _context.SaveChangesAsync();

//            var userDtos = _mapper.Map<UserDtos>(userp);

//            return CreatedAtAction(nameof(PostUser), new { id = userDtos.Id }, userDtos);
//        }

//        PUT
//        [HttpPut]
//        [Route("{id:long}")]
//        public async Task<IActionResult> PutUser(long id, [FromBody] UserDtos userUpdateDTO)
//        {
//            if (id != userUpdateDTO.Id)
//            {
//                return BadRequest();
//            }

//            var user = _mapper.Map<User>(userUpdateDTO);
//            _context.Entry(user).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!UserExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }
//        [HttpPut]
//        [Route("{id:guid}")]
//        public async Task<IActionResult> PutResident(Guid id, [FromBody] ResidentDto residentUpdateDTO)
//        {
//            if (id != residentUpdateDTO.Id)
//            {
//                return BadRequest();
//            }

//            var resident = _mapper.Map<Resident>(residentUpdateDTO);
//            _context.Entry(resident).State = EntityState.Modified;

//            try
//            {
//                await _context.SaveChangesAsync();
//            }
//            catch (DbUpdateConcurrencyException)
//            {
//                if (!ResidentExists(id))
//                {
//                    return NotFound();
//                }
//                else
//                {
//                    throw;
//                }
//            }

//            return NoContent();
//        }
//        [HttpDelete]
//        [HttpDelete("{id}")]
//        public async Task<IActionResult> DeleteUSer(long id)
//        {
//            var user = await _context.Users.FindAsync(id);
//            if (user == null)
//            {
//                return NotFound();
//            }

//            _context.Users.Remove(user);
//            await _context.SaveChangesAsync();

//            return NoContent();
//        }




//        private bool UserExists(long id)
//        {
//            return _context.Users.Any(e => e.Id == id);
//        }




//    }
//}

//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using CityApi.Data;
//using FirtsApi.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;

//[Route("api/[controller]")]
//[ApiController]
//public class UsersController : ControllerBase
//{
//    private readonly CityDBContext _context;
//    private readonly IConfiguration _configuration;

//    public UsersController(CityDBContext context, IConfiguration configuration)
//    {
//        _context = context;
//        _configuration = configuration;
//    }

//    [AllowAnonymous]
//    [HttpPost("register")]
//    public async Task<ActionResult<User>> Register(User user)
//    {
//        _context.Users.Add(user);
//        await _context.SaveChangesAsync();

//        return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
//    }

//    GET api/users/5
//    [HttpGet("{id}")]
//    public async Task<ActionResult<User>> GetUser(int id)
//    {
//        var user = await _context.Users.FindAsync(id);

//        if (user == null)
//        {
//            return NotFound();
//        }

//        return user;
//    }

//    [AllowAnonymous]
//    [HttpPost("login")]
//    public async Task<ActionResult<string>> Login([FromBody] LoginRequest request)
//    {
//        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username && u.Password == request.Password);

//        if (user == null)
//        {
//            return Unauthorized();
//        }

//        var token = GenerateJwtToken(user);
//        return Ok(token);
//    }


//    [HttpGet("logout")]
//    public IActionResult Logout()
//    {
//        Нет необходимости в реализации этого метода, так как JWT является без состояния и не содержит информации о сеансе.
//        return Ok("User logged out.");
//    }

//    private string GenerateJwtToken(User user)
//    {
//        var tokenHandler = new JwtSecurityTokenHandler();
//        var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:SecretKey"]);
//        var tokenDescriptor = new SecurityTokenDescriptor
//        {
//            Subject = new ClaimsIdentity(new[]
//            {
//            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
//            new Claim(ClaimTypes.Name, user.Username), // Замените UserName на соответствующее свойство
//            new Claim(ClaimTypes.Role, user.Role)
//        }),
//            Issuer = _configuration["JwtSettings:Issuer"],
//            Expires = DateTime.UtcNow.AddDays(7),
//            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//        };
//        var token = tokenHandler.CreateToken(tokenDescriptor);
//        return tokenHandler.WriteToken(token);
//    }

//    [HttpGet("message")]
//    [Authorize]
//    public ActionResult<string> GetMessage()
//    {
//        return "Защищенное сообщение, доступное только аутентифицированным пользователям";
//    }
//}

//public class LoginRequest
//{
//    public string Username { get; set; }
//    public string Password { get; set; }
//}

