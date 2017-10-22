using System;
using System.Collections.Generic;
using System.Text;

namespace SRIndia_Models
{
    public class MessageReplyDto
    {
        public string Id { get; set; }

        public string Owner { get; set; }

        public string Text { get; set; }

        public string MessageId { get; set; }

        public string ReplyrId { get; set; }

        public string ReplyUserId { get; set; }

        public string ImgId { get; set; }
    }
}
