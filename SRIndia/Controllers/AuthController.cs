using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using SRIndia_Models;
using SRIndia_Repository;
using SRIndiaInfo_Services;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        private IUserInfoRepository _userInfoRepository;

        public AuthController(IUserInfoRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }

        [HttpPost("login")]

        public ActionResult Login([FromBody] LoginData loginData)
        {
            var userEntity = _userInfoRepository.GetUserWithLogin(loginData.Email , loginData.Password);
            var newUser = Mapper.Map<UserDto>(userEntity);
            if (userEntity == null)
                return NotFound("email or password incorrect");

            return Ok(CreateJwtPacket(newUser));
        }

        [HttpPost("register")]
        public ActionResult Register([FromBody]UserDto user)
        {
            user.Id = Guid.NewGuid().ToString();
            var newUserEntity = Mapper.Map<AppUser>(user);
            _userInfoRepository.AddUser(newUserEntity);

            if (!_userInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request. Not able to register.");
            }
            return Ok(CreateJwtPacket(user));
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

        JwtPacket CreateJwtPacket(UserDto user)
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