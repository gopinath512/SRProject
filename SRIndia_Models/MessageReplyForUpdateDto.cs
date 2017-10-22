using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SRIndia_Models
{
    public class MessageReplyForUpdateDto
    {
        [Required(ErrorMessage = "Please provide the text.")]
        [MaxLength(200)]
        public string Text { get; set; }
    }
}
