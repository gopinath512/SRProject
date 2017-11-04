using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace SRIndia_Repository
{
    public class AppUser : IdentityUser
    {
        // Extended Properties
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime? DOB { get; set; }
        public string Sex { get; set; }
        public string ProfileBGImgId { get; set; }
        public string AvatarImgId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
