using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SRIndia_Models
{
    public class UserDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string AvatarImgId { get; set; }
        public string ProfileBGImgId { get; set; }
    }
}
