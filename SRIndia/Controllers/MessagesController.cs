using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using SRIndia_Models;
using SRIndia_Repository;
using SRIndiaInfo_Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using SRIndia.Common;

namespace SRIndia.Controllers
{
    [Produces("application/json")]
    [Route("api/Messages")]
    [EnableCors("Cors")]
    public class MessagesController : Controller
    {
        private readonly IMessageInfoRepository _messageInfoRepository;
        private readonly IUserInfoRepository _userInfoRepository;
        private readonly ILogger<MessagesController> _logger;
        private readonly IFileUpload _fileUploade;


        public MessagesController(IMessageInfoRepository messageContext, IUserInfoRepository userContext, IHostingEnvironment env, ILogger<MessagesController> logger, IFileUpload fileUploade)
        {
            _messageInfoRepository = messageContext;
            _userInfoRepository = userContext;
            _logger = logger;
            _fileUploade = fileUploade;
        }

        [HttpGet()]
        public IActionResult Get()
        {
            var messageEntity = _messageInfoRepository.GetMessagesByTypes(MessageTypes.All);
            var results = Mapper.Map<IEnumerable<MessageDto>>(messageEntity);

            return Ok(results);
        }

        [HttpGet("byuser")]
        [Authorize]
        public IActionResult GetMessageByUser()
        {
            var userId = HttpContext.User.Claims.First().Value;
            if (userId == null)
            {
                return NotFound("User id is empty");
            }

            var messageEntity = _messageInfoRepository.GetMessagesByTypes(MessageTypes.UserId, userId);
            var results = Mapper.Map<IEnumerable<MessageDto>>(messageEntity);
            return Ok(results);
        }

        //[HttpGet("{messageid}")]
        //public IActionResult GetMessageBYId(string messageid)
        //{
        //    var MessageEntity = _messageInfoRepository.GetMessagesByTypes(MessageTypes.MessageId, messageid);
        //    var results = Mapper.Map<IEnumerable<MessageDto>>(MessageEntity);
        //    return Ok(results);
        //}

        [HttpGet("{messageid}")]
        public IActionResult GetMessagesByMessageId(string messageId)
        {
            var messageEntity = _messageInfoRepository.GetMessagesByMessageId(messageId, true);
            var results = Mapper.Map<MessageAlongWithReplyDto>(messageEntity.FirstOrDefault());
            return Ok(results);
        }

        
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] JObject message)
        {
            try
            {
                MessageForCreationDto messageobj = message.ToObject<MessageForCreationDto>();
                var newMessage = Mapper.Map<Message>(messageobj);
                newMessage.UserId = GetSecureUserId() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(newMessage.UserId)) { return NotFound("User Id not found"); }
                _messageInfoRepository.AddMessage(newMessage);
                if (!_messageInfoRepository.Save())
                {
                    return StatusCode(500, new UploadeResponse { Success = false, ErrorDescription = "Error while creating message" });
                }
                return Ok(newMessage);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while adding new message.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }

        }

        [Authorize]
        [HttpPost]
        [Route("updateMessage")]
        public IActionResult UpdateMessage([FromBody] JObject message, string messageid)
        {
            try
            {
                var messageEntity = _messageInfoRepository.GetMessagesByTypes(MessageTypes.MessageId, messageid);
                MessageForUpdateDto messageobj = message.ToObject<MessageForUpdateDto>();
                Mapper.Map(messageobj, messageEntity);
                if (!_messageInfoRepository.Save())
                {
                    return StatusCode(500, new UploadeResponse { Success = false, ErrorDescription = "Error while updating message" });
                }
                return Ok(messageEntity);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while updating  message.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }

        }

        [HttpPost]
        [Authorize]
        [Route("upload")]

        public IActionResult Upload(IFormFile file)
        {
            var imgId = string.Empty;
            try
            {
                UploadeResponse objResult = _fileUploade.Upload(file);
                if (!objResult.Success)
                {
                    return StatusCode(500,
                        new UploadeResponse
                        {
                            ImageID = imgId,
                            Success = false,
                            ErrorDescription = objResult.ErrorDescription
                        });
                }
                imgId = objResult.ImageID;
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while uploading image.", ex);
                return StatusCode(500,
                    new UploadeResponse { ImageID = imgId, Success = false, ErrorDescription = ex.Message });
            }
            return Ok(new UploadeResponse { ImageID = imgId, Success = true });
        }

        //[HttpPost]
        //[Route("upload")]
        //public async Task<IActionResult> Post(List<IFormFile> files)
        //{
        //    long size = files.Sum(f => f.Length);

        //    //full path to file in temp location
        //    var filePath = Path.GetTempFileName();

        //    foreach (var formFile in files)
        //    {
        //        if (formFile.Length > 0)
        //        {
        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await formFile.CopyToAsync(stream);
        //            }
        //        }
        //    }

        //    //process uploaded files
        //    //Don't rely on or trust the FileName property without validation.

        //    return Ok(new { filePath });
        //}

        [HttpPost]
        [DisableFormValueModelBinding]

        [Route("uploadstreaming")]
        public async Task<IActionResult> Upload()
        {
            FormValueProvider formModel;
            using (var stream = System.IO.File.Create(@"C:\Users\Gopinath\AppData\Local\Temp\myfile.temp"))
            {
                formModel = await Request.StreamFile(stream);
            }

            var viewModel = new MyViewModel();

            var bindingSuccessful = await TryUpdateModelAsync(viewModel, prefix: "",
                valueProvider: formModel);

            if (!bindingSuccessful)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
            }

            return Ok(viewModel);
        }

        public class MyViewModel
        {
            public string Username { get; set; }
        }

        string GetSecureUserId()
        {
            var id = HttpContext.User.Claims.First().Value;
            if (_userInfoRepository.UserExists(id))
            {
                return id;
            }
            return null;
        }
    }
}