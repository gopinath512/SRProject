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

        [Required]
        [MaxLength(50)]
        public string Owner { get; set; }

        [Required]
        [MaxLength(200)]
        public string Text { get; set; }

        [Required]
        [MaxLength(3)]
        public int CatId { get; set; }

        [Required]
        [MaxLength(3)]
        public int Type { get; set; }

        [Required]
        public string ImgId { get; set; }

        
        [ForeignKey("UserId")]
        public AppUser AppUser { get; set; }

        [Required]
        public string UserId { get; set; }

        public ICollection<MessageReply> MessageReply { get; set; }
       = new List<MessageReply>();
    }
}
