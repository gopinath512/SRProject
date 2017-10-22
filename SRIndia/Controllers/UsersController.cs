using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SRIndia_Models;
using SRIndia_Repository;
using SRIndiaInfo_Services;
using System.Linq;

namespace MessageBoardBackend.Controllers
{
   

    [Produces("application/json")]
    [Route("api/Users")]
    [EnableCors("Cors")]
    public class UsersController : Controller
    {
        private IUserInfoRepository _userInfoRepository;

        public UsersController(IUserInfoRepository userInfoRepository)
        {
            _userInfoRepository = userInfoRepository;
        }

        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            var user = Mapper.Map<User>(GetSecureUser());

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }
        
        [Authorize]
        [HttpGet("me")]
        public ActionResult Get()
        {
            return Ok(Mapper.Map<User>(GetSecureUser()));
        }

        [Authorize]
        [HttpPost("me")]
        public ActionResult Post([FromBody] EditProfileData profileData)
        {
            var userEntity = GetSecureUser();
            Mapper.Map(profileData, userEntity);
           if(!_userInfoRepository.Save())
            {
                return StatusCode(500, new UploadeResponse { Success = false, ErrorDescription = "Error while saving while updating" });
            }

            return Ok(userEntity);
        }

        AppUser GetSecureUser()
        {
            var id = HttpContext.User.Claims.First().Value;
            var newUser = (_userInfoRepository.GetUser(id));
            return newUser;
        }
    }
}