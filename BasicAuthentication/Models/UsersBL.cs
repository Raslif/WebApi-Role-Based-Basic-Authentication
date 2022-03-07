using System.Collections.Generic;

namespace BasicAuthentication.Models
{
    public class UsersBL
    {
        public List<User> GetUsers()
        {
            // In Realtime you need to get the data from any persistent storage
            List<User> userList = new List<User>
            {
                new User()
                {
                    ID = 101,
                    UserName = "AdminUser",
                    Password = "123456",
                    Roles = "Admin",
                    Email = "Admin@a.com"
                },
                new User()
                {
                    ID = 102,
                    UserName = "BothUser",
                    Password = "abcdef",
                    Roles = "Admin,Superadmin",
                    Email = "BothUser@a.com"
                },
                new User()
                {
                    ID = 103,
                    UserName = "SuperadminUser",
                    Password = "Password@123",
                    Roles = "Superadmin",
                    Email = "Superadmin@a.com"
                }
            };
            return userList;
        }
    }
}