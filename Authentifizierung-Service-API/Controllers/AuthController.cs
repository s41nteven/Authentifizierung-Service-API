using Authentifizierung_Service_API.Data;
using Authentifizierung_Service_API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics.Eventing.Reader;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Authentifizierung_Service_API.Controllers
{
    [Route("/account")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        public static User user = new User();
        private readonly IConfiguration _configuration;

        private readonly AuthContext _context;

        public AuthController(AuthContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("register")]
        public IActionResult RegisterUser(UserDto request, string username, string password, int fahrtenbuchWrite, int adressenWrite)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            string userPasswordHash = System.Text.Encoding.UTF8.GetString(passwordHash);

            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            var userReg = new User { Username = username, PasswordHash = passwordHash, PasswordSalt = passwordSalt, FahrtenbuchWrite = fahrtenbuchWrite, AdressenWrite = adressenWrite };

            Console.WriteLine("User Hash " + userPasswordHash);

            _context.Add(userReg);
            _context.SaveChanges();

            return Ok(user);
        }

        [HttpPost("login")]
        public IActionResult Login(UserRequest userRequest)
        {
            var dbUser = _context.Users.Find(userRequest.Username);

            byte[] userRequestHash = System.Text.Encoding.UTF8.GetBytes(userRequest.PasswordHash);

            if (dbUser == null)
            {
                return BadRequest("No DB User found");
            }

            if (dbUser.Username != userRequest.Username)
            {
                return BadRequest("User not found.");
            }

            if (!VerifyPasswordHash(userRequestHash, dbUser.PasswordHash))
            {
                return BadRequest("Wrong password.");
            }

            //string token = CreateToken(user);

            return Ok(dbUser);
        }

        [HttpPut("update")]
        public IActionResult UpdateUser(User userRequest)
        {
            var dbEntryUser = _context.Users.FirstOrDefault(u => u.Username == userRequest.Username);

            if (dbEntryUser == null)
            {
                NotFound();
            }

            dbEntryUser.Username = userRequest.Username;
            dbEntryUser.PasswordHash = userRequest.PasswordHash;

            return Ok("Updated user {$username}");
        }

        [HttpDelete("delete")]
        public IActionResult DeleteUser(User userRequest)
        {
            var dbEntryUser = _context.Users.FirstOrDefault(u => u.Username == userRequest.Username);

            if (dbEntryUser == null)
            {
                NotFound();
            }

            _context.Remove(dbEntryUser);
            _context.SaveChanges();

            return Ok("Deleted user {$username}");
        }

        //private string CreateToken(User user)
        //{
        //    List<Claim> claims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, user.Username)
        //    };

        //    var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
        //    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        //    var token = new JwtSecurityToken(
        //        claims: claims,
        //        expires: DateTime.Now.AddDays(1),
        //        signingCredentials: creds);

        //    var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        //    return jwt;
        //}

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(byte[] passwordHashRequest, byte[] passwordHashUser)
        {
            using (var hmac = new HMACSHA512(passwordHashUser))
            {
                var computeHash = hmac.ComputeHash(passwordHashRequest);
                return computeHash.SequenceEqual(passwordHashUser);
            }

            //return passwordHashRequest == passwordHashUser ? true : false;
        }
    }
}
