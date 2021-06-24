using EX_AUTH_BE.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EX_AUTH_BE.DAL.Repositories
{
    public class externalLoginRepository : Repository<ExternalLogins>
    {
        public ExternalAuthContext Ctx { get; }
        public externalLoginRepository(ExternalAuthContext context) : base(context)
        {
            Ctx = context;
        }
    }
}
