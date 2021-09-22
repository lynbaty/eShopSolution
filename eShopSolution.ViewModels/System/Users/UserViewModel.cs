using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.System.Users
{
    public class UserViewModel
    {
        public Guid Id { set; get; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { set; get; }
        public string Email { set; get; }
        public string PhoneNumber { set; get; }
    }
}