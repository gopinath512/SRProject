using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRIndia_Models
{
    public class MessageView
    {
        public string Id { get; set; }
        public string Owner { get; set; }
        public string Text { get; set; }
        public int CatId { get; set; }
        public int Type { get; set; }
        public IFormFile File { get; set; }
    }
}
