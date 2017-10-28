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

namespace MessageBoardBackend.Controllers
{
    [Produces("application/json")]
    [Route("api/Messages")]
    [EnableCors("Cors")]
    public class MessagesController : Controller
    {
        private IMessageInfoRepository _messageInfoRepository;
        private IUserInfoRepository _userInfoRepository;
        private IHostingEnvironment _hostingEnv;
        private ILogger<MessagesController> _logger;

        public MessagesController(IMessageInfoRepository messageContext, IUserInfoRepository userContext, IHostingEnvironment env, ILogger<MessagesController> logger)
        {
            _messageInfoRepository = messageContext;
            _userInfoRepository = userContext;
            _hostingEnv = env;
            _logger = logger;
        }

        [HttpGet()]
        public IActionResult Get()
        {
            var MessageEntity = _messageInfoRepository.GetMessagesByTypes(MessageTypes.All);
            var results = Mapper.Map<IEnumerable<MessageDto>>(MessageEntity);
            _logger.LogCritical($"Exception while adding new message.", "test");
            return Ok(results);
        }

        [HttpGet("byuser")]
        [Authorize]
        public IActionResult GetMessageByUser()
        {
            var userId = HttpContext.User.Claims.First().Value;
            if(userId == null)
            {
                return NotFound("User id is empty");
            }

            var MessageEntity =  _messageInfoRepository.GetMessagesByTypes(MessageTypes.UserId, userId);
            var results = Mapper.Map<IEnumerable<MessageDto>>(MessageEntity);
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
            var MessageEntity = _messageInfoRepository.GetMessagesByMessageId(messageId,true);
            var results = Mapper.Map<MessageAlongWithReplyDto>(MessageEntity.FirstOrDefault());
            return Ok(results);
        }

        [Authorize]
        [HttpPost]
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
            catch(Exception ex) {
                _logger.LogCritical($"Exception while adding new message.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }
           
        }

        [Authorize]
        [HttpPost]
        public IActionResult UpdateMessage([FromBody] JObject message, string messageid)
        {
            try
            {
                var MessageEntity = _messageInfoRepository.GetMessagesByTypes(MessageTypes.MessageId, messageid);
                MessageForUpdateDto messageobj = message.ToObject<MessageForUpdateDto>();
                Mapper.Map(messageobj, MessageEntity);
                if (!_messageInfoRepository.Save())
                {
                    return StatusCode(500, new UploadeResponse { Success = false, ErrorDescription = "Error while updating message" });
                }
                return Ok(MessageEntity);
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while updating  message.", ex);
                return StatusCode(500, "A problem happened while handling your request.");
            }

        }

        [HttpPost]
        [Route("upload")]
        public IActionResult Upload(IFormFile file)
        {
            var imgId = string.Empty;
            try
            {
                long size = 0;

                var filename = ContentDispositionHeaderValue
                            .Parse(file.ContentDisposition)
                            .FileName
                            .Trim('"');
                filename = Guid.NewGuid() + "_" + filename;
                imgId = filename;
                filename = _hostingEnv.WebRootPath + "\\Images" + $@"\{filename}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical($"Exception while uploading image.", ex);
                return StatusCode(500, new UploadeResponse { ImageID = imgId, Success = false, ErrorDescription = ex.Message });
            }
            return Ok(new UploadeResponse { ImageID = imgId, Success = true });
        }

        string GetSecureUserId()
        {
            var id = HttpContext.User.Claims.First().Value;
            if(_userInfoRepository.UserExists(id))
            {
                return id;
            }
            return null;
        }
    }
}