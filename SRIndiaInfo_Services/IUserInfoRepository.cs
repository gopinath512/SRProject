using System;
using System.Collections.Generic;
using System.Text;
using SRIndia_Repository;

namespace SRIndiaInfo_Services
{
    public interface IUserInfoRepository
    {
        bool UserExists(string userId);

        AppUser GetUserWithLogin(string emailId, string strPassword);

        IEnumerable<AppUser> GetUsers();

        AppUser GetUserByUserId(string userId);

        AppUser AddUser(AppUser appUser);

        void DeleteUser(AppUser appUser);

        bool Save();
    }
}
