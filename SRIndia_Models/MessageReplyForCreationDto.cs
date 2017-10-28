using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SRIndia_Models
{
    public class MessageReplyForCreationDto
    {

        [Required(ErrorMessage = "Please provide the text.")]
        public string Owner { get; set; }

        [Required(ErrorMessage = "Please provide the text.")]
        [MaxLength(200)]
        public string Text { get; set; }

        [Required(ErrorMessage = "Please provide the text.")]
        public string MessageId { get; set; }

        [Required(ErrorMessage = "Please provide the text.")]
        public int ReplyNumber { get; set; }

        [Required(ErrorMessage = "Please provide the text.")]
        public string ReplyUserId { get; set; }

        public string ImgId { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
