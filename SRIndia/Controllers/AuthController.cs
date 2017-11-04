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

    [Produces("application/json")]
    [Route("Auth")]
    [EnableCors("Cors")]
    public class AuthController : Controller
    {

        private readonly IUserInfoRepository _userInfoRepository;

        public AuthController(IUserInfoRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }

        [HttpPost("login")]

        public ActionResult Login([FromBody] UserLoginDto loginData)
        {
            var userEntity = _userInfoRepository.GetUserWithLogin(loginData.Email , loginData.Password);
            if (userEntity == null)
                return NotFound("email or password incorrect");

            var newUser = Mapper.Map<UserLoginDto>(userEntity);
            return Ok(CreateJwtPacket(userEntity.Id, userEntity.FirstName));
        }
       
        [HttpPost("register")]
        public ActionResult Register([FromBody]UserForCreationDto user)
        {
            user.Id = Guid.NewGuid().ToString();
            var newUserEntity = Mapper.Map<AppUser>(user); 
            var result = _userInfoRepository.AddUser(newUserEntity);
            if(result == null)
            {
                return StatusCode(500, "This email already exists. Not able to register.");
            }
            if (!_userInfoRepository.Save())
            {
                return StatusCode(500, "A problem happened while handling your request. Not able to register.");
            }
            return Ok(CreateJwtPacket(user.Id,user.FirstName));
        }

        JwtPacket CreateJwtPacket(string strUserId, string strFirstName)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("this is the secret phrase"));

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, strUserId)
            };

            var jwt = new JwtSecurityToken(claims: claims, signingCredentials: signingCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new JwtPacket() { Token = encodedJwt, FirstName = strFirstName };
        }
    }


}