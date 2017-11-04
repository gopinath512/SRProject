using System;
using System.Collections.Generic;
using System.Text;

namespace SRIndia_Models
{
    public class MessageImagesDto
    {
        public string Id { get; set; }

        public string MessageId { get; set; }

        public string ImgId { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime ModifiedDate { get; set; }
    }
}
