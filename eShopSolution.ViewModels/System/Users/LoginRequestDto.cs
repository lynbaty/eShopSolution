using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModels.System.Users
{
    public class LoginRequestDto
    {
        public string UserName { set; get; }
        public string Password { set; get; }
        public bool Remember { set; get; }
    }
}