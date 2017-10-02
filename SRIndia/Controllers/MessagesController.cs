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

        [HttpPost]
        public Message Post([FromBody] MessageView message)
        {
            var newUser = Mapper.Map<Message>(message);
            var dbMessage = context.Messages.Add(newUser).Entity;
            context.SaveChanges();
            //Upload(message.file);
            return dbMessage;
        }

        [HttpPost]
        public Message FileUploade([FromBody] MessageView message)
        {
            var newUser = Mapper.Map<Message>(message);
            var dbMessage = context.Messages.Add(newUser).Entity;
            context.SaveChanges();
            //Upload(message.file);
            return dbMessage;
        }

        public void Upload(IFormFile file)
        {
            long size = 0;
            var filename = ContentDispositionHeaderValue
                        .Parse(file.ContentDisposition)
                        .FileName
                        .Trim('"');
            filename = hostingEnv.WebRootPath + $@"\{filename}";
            size += file.Length;
            using (FileStream fs = System.IO.File.Create(filename))
            {
                file.CopyTo(fs);
                fs.Flush();
            }
        }
    }
}