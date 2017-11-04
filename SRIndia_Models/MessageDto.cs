using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace SRIndia_Models
{
    public class MessageDto
    {
        public string Id { get; set; }
        public int MessageNumber { get; set; }
        public string Owner { get; set; }
        public int ReplyCount { get; set; }
        public int ClickCount { get; set; }
        public string Topic { get; set; }
        public string Text { get; set; }
        public int CatId { get; set; }
        public int Type { get; set; }

        public ICollection<MessageImagesDto> MessageImages { get; set; }
            = new List<MessageImagesDto>();

        public string UserId { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
