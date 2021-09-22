﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace eShopSolution.ViewModels.System.Users
{
    public class RegisterRequestDto
    {
        [Display(Name = "Tên")]
        public string FirstName { get; set; }

        [Display(Name = "Họ")]
        public string LastName { get; set; }

        [Display(Name = "Tên người dùng")]
        public string UserName { set; get; }

        [Display(Name = "Mật khẩu")]
        public string Password { set; get; }

        [Display(Name = "Nhập lại mật khẩu")]
        public string ComfirmPassword { set; get; }

        [Display(Name = "Email")]
        public string Email { set; get; }

        [Display(Name = "Số điện thoại")]
        public string PhoneNumber { set; get; }
    }
}