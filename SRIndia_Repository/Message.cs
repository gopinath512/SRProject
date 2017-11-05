using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace SRIndia_Repository
{
    public class Message
    {
        public string Id { get; set; }

        public int MessageNumber { get; set; }

        [Required]
        [MaxLength(50)]
        public string Owner { get; set; }

        public int ReplyCount { get; set; }
        public int ClickCount { get; set; }

        [Required]
        [MaxLength(200)]
        public string Topic { get; set; } = "Default";

        [Required]
        [MaxLength(200)]
        public string Text { get; set; }

        [Required]
        [MaxLength(3)]
        public int CatId { get; set; }

        [Required]
        [MaxLength(3)]
        public int Type { get; set; }

        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = System.DateTime.Now;


        public DateTime ModifiedDate { get; set; } = System.DateTime.Now;

        public ICollection<MessageReply> MessageReply { get; set; }
       = new List<MessageReply>();

        public ICollection<MessageImages> MessageImages { get; set; }
            = new List<MessageImages>();

    }
}
