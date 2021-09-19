using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.System.Users
{
    public class RegisterRequestDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string UserName { set; get; }
        public string Password { set; get; }
        public string ComfirmPassword { set; get; }
        public string Email { set; get; }
        public string PhoneNumber { set; get; }
    }
}