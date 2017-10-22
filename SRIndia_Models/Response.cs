using System;
using System.Collections.Generic;
using System.Text;

namespace SRIndia_Models
{
    public class Response
    {
        public bool Success { get; set; }
        public string ErrorDescription { get; set; }
    }
    public class UploadeResponse : Response
    {
        public string ImageID { get; set; }
    }
}
