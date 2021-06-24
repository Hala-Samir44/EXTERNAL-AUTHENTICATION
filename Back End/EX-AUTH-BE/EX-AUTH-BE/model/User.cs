
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EX_AUTH_BE.model
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public byte[] Password { get; set; }
        public byte[] PasswordSalt { get; set; }
        public ExternalLogins ExternalLogins { get; set; }
        public string ProfilePictureURL { get; internal set; }
    }

}
