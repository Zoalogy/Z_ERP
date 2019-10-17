using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Z_ERP.Models;

namespace Z_ERP .Security
{
    public class SecurityClass
    {
        public static bool Login(string username, string password)
        {
            using (MainModel entities = new MainModel())
            {
                return entities.sys_Clients.Any(user =>
                       user.ClientUserName  .Equals(username, StringComparison.OrdinalIgnoreCase)
                                          && user.ClientPassword   == password && user.IsActive ==true );
            }
        }
    }
}