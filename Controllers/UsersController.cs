using Authenticator.DTO;
using Authenticator.Interfaces;
using Authenticator.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Authenticator.Controllers
{
    [ApiController]
    [Route("api/[controller]/AuthPerson")]
    public class UsersController : ControllerBase
    {
        private readonly IAuthRepository _repo;

        private readonly IConfiguration _config;
        private readonly ILogger<UsersController> _logger;
        public UsersController(IAuthRepository repo, ILogger<UsersController> logger, IConfiguration config)
        {
            _repo = repo;
            _logger = logger;
            _config = config;
        }

        // GET: api/Users
        [HttpGet]
        [Authorize(Policy = "AdminPolicy")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            return await _repo.GetAllUsers();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        [Authorize(Policy = "UserPolicy")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _repo.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            try
            {
                await _repo.UpdateUser(user);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repo.UserExistsById(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto userDto)
        {
            var userFromRepo = await _repo.Login(userDto.Username.ToLower(), userDto.Password);
            if (userFromRepo == null) //User login failed
                return Unauthorized();


            // generate jwt token with claims
            var claims = new Claim[]{
                    new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                    new Claim(ClaimTypes.Name, userFromRepo.Username)
                };

            var expires = DateTime.UtcNow.Add(TimeSpan.FromMinutes(2));


            var token = new JwtSecurityToken(
                _config.GetSection("Jwt")["Issuer"],
                _config.GetSection("Jwt")["Audience"],
                claims,
                expires: expires,
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_config.GetSection("Jwt")["Key"])),
                    SecurityAlgorithms.HmacSha256)
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            return Ok(new { tokenString });
        }

        // POST: api/Users
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> PostUser([FromBody] UserDto userDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                userDto.Username = userDto.Username.ToLower(); //Convert username to lower case before storing in database.

                if (await _repo.UserExists(userDto.Username))
                    return BadRequest("Username is already taken");

                var createUser = await _repo.Register(userDto, userDto.Password);

                userDto.UserId = createUser.Id;

                return CreatedAtAction("GetUser", new { id = createUser.Id }, userDto);

            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _repo.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            await _repo.DeleteUser(user);

            return NoContent();
        }
    }
}
