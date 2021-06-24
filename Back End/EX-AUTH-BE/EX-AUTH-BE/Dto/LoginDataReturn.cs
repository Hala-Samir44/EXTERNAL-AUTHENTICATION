using EX_AUTH_BE.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EX_AUTH_BE.Dto
{
    public class LoginDataReturn
    {
        public string username { get; set; }
        public string token { get; set; }
        public ResultStatusEnum status { get; set; }

        public int ExpiresIn { get; set; }
    }
}
