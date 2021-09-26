using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.System.Users
{
    public class UserRolesDto
    {
        public IList<string> roles { set; get; }

        public Guid Id { set; get; }
    }
}