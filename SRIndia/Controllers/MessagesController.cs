using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SRIndia_Repository;
using SRIndia_Models.Models;
using Microsoft.AspNetCore.Cors;
using System.IO;
using Microsoft.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using SRIndia_Models;
using AutoMapper;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Authorization;

namespace MessageBoardBackend.Controllers
{
    [Produces("application/json")]
    [Route("api/Messages")]
    [EnableCors("Cors")]
    public class MessagesController : Controller
    {
        readonly SRIndiaContext context;
        private IHostingEnvironment hostingEnv;

        public MessagesController(SRIndiaContext context, IHostingEnvironment env)
        {
            this.context = context;
            this.hostingEnv = env;
        }

        public IEnumerable<Message> Get()
        {
            return context.Messages;
        }

        [HttpGet("{name}")]
        public IEnumerable<Message> Get(string name)
        {
            return context.Messages.Where(message => message.Owner == name);
        }

        [HttpGet("message/{id}")]
        public IEnumerable<Message> GetMessageBYId(string id)
        {
            return context.Messages.Where(message => message.Id == id);
        }


        [Authorize]
        [HttpPost]
        public Message Post([FromBody] JObject message)
        {
            dynamic jsonData = message;
            MessageView messageobj = jsonData.ToObject<MessageView>();
            var newMessage = Mapper.Map<Message>(messageobj);
            newMessage.UserId = GetSecureUser().Id;
            var dbMessage = context.Messages.Add(newMessage).Entity;
            context.SaveChanges();
            return dbMessage;
        }

        [HttpPost]
        [Route("upload")]
        public UploadeResponse Upload(IFormFile file)
        {
            var newId = Guid.NewGuid();
            var imgId = string.Empty;
            try
            {
                long size = 0;

                var filename = ContentDispositionHeaderValue
                            .Parse(file.ContentDisposition)
                            .FileName
                            .Trim('"');
                filename = newId + "_" + filename;
                imgId = filename;
                filename = hostingEnv.WebRootPath + "\\Images" + $@"\{filename}";
                size += file.Length;
                using (FileStream fs = System.IO.File.Create(filename))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }
            catch (Exception ex)
            {
                return new UploadeResponse { ImageID = imgId, Success = false };
            }
            return new UploadeResponse { ImageID = imgId, Success = true };
        }

        User GetSecureUser()
        {
            var id = HttpContext.User.Claims.First().Value;
            var newUser = Mapper.Map<User>(context.Users.SingleOrDefault(u => u.Id == id));
            return newUser;
        }
    }
}