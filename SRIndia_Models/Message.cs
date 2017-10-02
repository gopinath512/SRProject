using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SRIndia_Models.Models
{
    public class Message
    {
        public string Id { get; set; }
        public string Owner { get; set; }
        public string Text { get; set; }
        public int CatId { get; set; }
        public string ImageUrl { get; set; }
        public int Type { get; set; }
    }
}
