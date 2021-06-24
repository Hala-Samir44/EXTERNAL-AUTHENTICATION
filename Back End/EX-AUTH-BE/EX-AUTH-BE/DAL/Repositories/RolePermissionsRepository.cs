using EX_AUTH_BE.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EX_AUTH_BE.DAL.Repositories
{
    public class RolePermissionsRepository : Repository<RolePermissions>
    {
        public ExternalAuthContext Ctx { get; }
        public RolePermissionsRepository(ExternalAuthContext context) : base(context)
        {
            Ctx = context;
        }
    }
}
