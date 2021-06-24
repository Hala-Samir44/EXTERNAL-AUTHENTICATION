using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EX_AUTH_BE.model
{
    public class ExternalLogins
    {
        public int Id { get; set; }
        public int UserId { get; set; }

        public string GoogleId { get; set; }

        public string FacebookId { get; set; }

        public User User { get; set; }
    }
}
