using System;
using EX_AUTH_BE.model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EX_AUTH_BE.Dto;
using Microsoft.EntityFrameworkCore;

namespace EX_AUTH_BE.DAL.Repositories
{
    public class PermissionRepository : Repository<Permission>
    {
        public ExternalAuthContext Ctx { get; }
        public PermissionRepository(ExternalAuthContext context) : base(context)
        {
            Ctx = context;
        }

        public async Task<List<PermissionDto>> GetAllPermissionWithPermisionAsync()
        {
            var x = await Context.Set<Permission>().ToListAsync();
            var c = x.Select
               (p => new PermissionDto
               {
                   Id = p.Id,
                   Title = p.Title,
                   Description = p.Description
               }).ToList();
            return c;
        }


        public async Task<Permission> AddOrEditPermission(PermissionDto permissionDto, Permission permission = null)
        {
            var isEdit = true;
            if (permission == null)
            {
                isEdit = false;
                permission = new Permission();

            }

            permission.Title = permissionDto.Title;
            permission.Description = permissionDto.Description;

            return permission;
        }



    }
}
