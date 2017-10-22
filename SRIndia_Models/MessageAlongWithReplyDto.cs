using System;
using System.Collections.Generic;
using System.Text;

namespace SRIndia_Models
{
    public class MessageAlongWithReplyDto
    {
        public string Id { get; set; }
        public string Owner { get; set; }
        public string Text { get; set; }
        public int CatId { get; set; }
        public int Type { get; set; }
        public string ImgId { get; set; }
        public string UserId { get; set; }

        public int NumberOfRepliest
        {
            get
            {
                return MessageReply.Count;
            }
        }

        public ICollection<MessageReplyDto> MessageReply { get; set; }
        = new List<MessageReplyDto>();
    }
}
