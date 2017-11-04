using System;
using System.Collections.Generic;
using System.Text;

namespace SRIndia_Models
{
    public class MessageAlongWithReplyDto
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
        public string UserId { get; set; }
        public DateTime ModifiedDate { get; set; }

        public int NumberOfRepliest
        {
            get
            {
                return MessageReply.Count;
            }
        }

        public ICollection<MessageReplyDto> MessageReply { get; set; }
        = new List<MessageReplyDto>();

        public ICollection<MessageImagesDto> MessageImages { get; set; }
            = new List<MessageImagesDto>();
    }
}
