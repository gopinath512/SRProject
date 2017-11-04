using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SRIndia_Repository
{
    public class MessageImages
    {
        public string Id { get; set; }

        [Required]
        [ForeignKey("MessageId")]
        public Message Message { get; set; }
        public string MessageId { get; set; }

        [Required]
        public string ImgId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public DateTime ModifiedDate { get; set; }
    }
}
