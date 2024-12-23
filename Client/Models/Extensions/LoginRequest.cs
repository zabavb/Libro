﻿using System.ComponentModel.DataAnnotations;

namespace Client.Models.Extensions
{
    public class LoginRequest
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Password { get; set; }

        public LoginRequest()
        {
            FullName = null!;
            Password = null!;
        }
    }
}
