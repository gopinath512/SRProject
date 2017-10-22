using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SRIndia_Repository
{
    public class MessageReply
    {
        public string Id { get; set; }

        [Required]
        public string Owner { get; set; }

        [Required]
        public string Text { get; set; }

        [Required]
        [ForeignKey("MessageId")]
        public Message Message { get; set; }
        public string MessageId { get; set; }

        public string ReplyUserId { get; set; }

        [Required]
        public string ImgId { get; set; }
        
    }
}
