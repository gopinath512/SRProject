using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SRIndia_Repository;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SRIndia_Models.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;

namespace SRIndia.Controllers
{

    public class JwtPacket
    {
        public string Token { get; set; }
        public string FirstName { get; set; }
    }

    public class LoginData
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    [Produces("application/json")]
    [Route("Auth")]
    [EnableCors("Cors")]
    public class AuthController : Controller
    {
        readonly SRIndiaContext context;

        public AuthController(SRIndiaContext context)
        {
            this.context = context;
        }

        [HttpPost("login")]

        public ActionResult Login([FromBody] LoginData loginData)
        {
            var user = context.Users.SingleOrDefault(u => u.Email == loginData.Email && u.PasswordHash == loginData.Password);
            var newUser = Mapper.Map<User>(user);
            if (user == null)
                return NotFound("email or password incorrect");

            return Ok(CreateJwtPacket(newUser));
        }

        [HttpPost("register")]
        public JwtPacket Register([FromBody]User user)
        {
            user.Id = Guid.NewGuid().ToString();
            var newUser = Mapper.Map<AppUser>(user);
            context.Users.Add(newUser);
            context.SaveChanges();
            return CreateJwtPacket(user);
        }

        //[HttpPost("aspregister")]
        //public async Task<JwtPacket> AspRegister([FromBody]Models.User user)
        //{
        //    var newuser = Mapper.Map<AppUser>(user);
        //    // context.users.add(newuser);
        //    // context.savechanges();
        //    var result = await _userManager.CreateAsync(newuser);
        //    return CreateJwtPacket(user);
        //}

        JwtPacket CreateJwtPacket(User user)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("this is the secret phrase"));

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id)
            };

            var jwt = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new JwtPacket() { Token = encodedJwt, FirstName = user.FirstName };
        }
    }


}