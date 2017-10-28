using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SRIndia_Models
{
    public class UserForCreationDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DOB { get; set; }
        public string Sex { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
