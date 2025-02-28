using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DTOs.User
{
    public class Password
    {
        public Guid Id { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public Password()
        {
            PasswordHash = string.Empty;
            PasswordSalt = string.Empty;
        }
    }
}
