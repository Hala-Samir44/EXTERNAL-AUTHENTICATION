using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EX_AUTH_BE.Dto
{
    public class LoginDto
    {
        [Display(Name = "User Name")]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}
