using System;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SRIndia.Common;
using SRIndia_Models;
using SRIndia_Repository;
using SRIndiaInfo_Services;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;

namespace SRIndia.Controllers
{


    [Produces("application/json")]
    [Route("api/Users")]
    [EnableCors("Cors")]
    public class UsersController : Controller
    {
        private IUserInfoRepository _userInfoRepository;
        private ILogger<MessagesController> _logger;
        private readonly IFileUpload _fileUploade;

        public UsersController(IUserInfoRepository userInfoRepository, IFileUpload fileUploade)
        {
            _userInfoRepository = userInfoRepository;
            _fileUploade = fileUploade;

        }

        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            var user = Mapper.Map<UserDto>(GetSecureUser());

            if (user == null)
                return NotFound("User not found");

            return Ok(user);
        }

        [Authorize]
        [HttpGet("me")]
        public ActionResult Get()
        {
            return Ok(Mapper.Map<UserDto>(GetSecureUser()));
        }

        [HttpGet("getAll")]
        public ActionResult GetAllUsers()
        {
            var response = _userInfoRepository.GetUsers();
            return Ok(response);
        }

        [Authorize]
        [HttpPost("me")]
        public ActionResult Post([FromBody] UserForUpdateDto profileData)
        {
            var userEntity = GetSecureUser();
            Mapper.Map(profileData, userEntity);
            if (!_userInfoRepository.Save())
            {
                return StatusCode(500, new UploadeResponse { Success = false, ErrorDescription = "Error while saving while updating" });
            }

            return Ok(userEntity);
        }

        [HttpPost]
        [Route("upload")]
        [Authorize]
        public IActionResult Upload(IFormFile file)
        {
            var imgId = string.Empty;
            try
            {
                UploadeResponse objResult = _fileUploade.Upload(file);
                if (!objResult.Success)
                {
                    return StatusCode(500,
                        new UploadeResponse { ImageID = imgId, Success = false, ErrorDescription = objResult.ErrorDescription });
                }
                imgId = objResult.ImageID;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while uploading image.", ex);
            }
            return Ok(new UploadeResponse { ImageID = imgId, Success = true });
        }

        //[HttpPost]
        //[DisableFormValueModelBinding]
        //[Authorize]
        //[Route("upload")]
        //public async Task<IActionResult> Upload()
        //{
        //    FormValueProvider formModel;
        //    using (var stream = System.IO.File.Create("c:\\temp\\myfile.temp"))
        //    {
        //        formModel = await Request.StreamFile(stream);
        //    }

        //    var viewModel = new MyViewModel();

        //    var bindingSuccessful = await TryUpdateModelAsync(viewModel, prefix: "",
        //        valueProvider: formModel);

        //    if (!bindingSuccessful)
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(ModelState);
        //        }
        //    }

        //    return Ok(viewModel);
        //}

        public class MyViewModel
        {
            public string Username { get; set; }
        }

        AppUser GetSecureUser()
        {
            var id = HttpContext.User.Claims.First().Value;
            var newUser = (_userInfoRepository.GetUserByUserId(id));
            return newUser;
        }
    }
}