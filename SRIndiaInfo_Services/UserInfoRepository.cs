using System;
using System.Collections.Generic;
using System.Text;
using SRIndia_Repository;

using System.Linq;
using SRIndia_Models;

namespace SRIndiaInfo_Services
{
    public class UserInfoRepository : IUserInfoRepository
    {
        private SRIndiaContext _context;
        public UserInfoRepository(SRIndiaContext context)
        {
            _context = context;
        }

        public bool UserExists(string userId)
        {
            return _context.Users.Any(i => i.Id == userId);
        }

        public AppUser GetUserWithLogin(string userId, string strPassword)
        {
           return _context.Users.SingleOrDefault(u => u.Email == userId && u.PasswordHash == strPassword);
        }

        public void AddUser(AppUser appUser)
        {
            _context.Users.Add(appUser);
        }

        public void DeleteUser(AppUser appUser)
        {
            _context.Users.Remove(appUser);
        }

        public AppUser GetUser(string userId)
        {
            return _context.Users.SingleOrDefault(i => i.Id == userId);
        }

        public IEnumerable<AppUser> GetUsers()
        {
            return _context.Users.ToList();
        }


        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }

    }
}
