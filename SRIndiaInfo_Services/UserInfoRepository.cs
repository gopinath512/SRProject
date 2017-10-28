using System;
using System.Collections.Generic;
using SRIndia_Repository;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace SRIndiaInfo_Services
{
    public class UserInfoRepository : PasswordHasher<AppUser>, IUserInfoRepository
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

        public AppUser GetUserWithLogin(string emailId, string strPassword)
        {
            var appUser = GetUserByEmail(emailId);

            if (appUser != null)
                if (base.VerifyHashedPassword(appUser, appUser.PasswordHash, strPassword) == PasswordVerificationResult.Success)
                    return appUser;

            return null;
        }

        public AppUser AddUser(AppUser appUser)
        {
            var objUserResult = GetUserByEmail(appUser.Email);
            if (objUserResult == null)
            {
                appUser.PasswordHash = base.HashPassword(appUser, appUser.PasswordHash);
                _context.Users.Add(appUser);
                return appUser;
            }

            return null;
        }

        public void DeleteUser(AppUser appUser)
        {
            _context.Users.Remove(appUser);
        }

        public AppUser GetUserByUserId(string userId)
        {
            return _context.Users.SingleOrDefault(i => i.Id == userId);
        }

        private AppUser GetUserByEmail(string emailId)
        {
            return _context.Users.SingleOrDefault(i => i.Email == emailId);
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
