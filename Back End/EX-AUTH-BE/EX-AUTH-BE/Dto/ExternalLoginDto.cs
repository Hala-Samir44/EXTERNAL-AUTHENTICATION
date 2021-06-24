using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EX_AUTH_BE.Dto
{
    public class ExternalLoginDto
    {
        public string Token { get; set; }
        public string Password { get; set; }
        public string ExternalLoginType { get; set; }
    }
}
