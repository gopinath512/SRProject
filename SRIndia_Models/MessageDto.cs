using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRIndia_Models
{
    public class MessageDto
    {
        public string Id { get; set; }
        public string Owner { get; set; }
        public string Text { get; set; }
        public int CatId { get; set; }
        public int Type { get; set; }
        public string ImgId { get; set; }
        public string UserId { get; set; }
    }
}
