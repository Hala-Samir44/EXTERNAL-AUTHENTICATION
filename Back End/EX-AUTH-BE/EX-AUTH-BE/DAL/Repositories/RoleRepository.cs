using System;
using EX_AUTH_BE.model;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using EX_AUTH_BE.Dto;

namespace EX_AUTH_BE.DAL.Repositories
{
    public class RoleRepository : Repository<Role>
    {
        private readonly IMapper _mapper;
        public ExternalAuthContext Ctx { get; }
        public RoleRepository(IMapper mapper, ExternalAuthContext context) : base(context)
        {
            Ctx = context;
            this._mapper = mapper;
        }
        public async Task<List<RoleDto>> GetAllRoleWithPermisionAsync()
        {
            var x = await Context.Set<Role>().Include(e => e.RolePermissions).ToListAsync();
            var c = x.Select
               (p => new RoleDto
               {
                   Id = p.Id,
                   Title = p.Title,
                   Description = p.Description,
                   Permissions = p.RolePermissions.Select(a => a.permissionId).ToList()


               }).ToList();
            return c;
        }


        public async Task<Role> AddOrEditRole(RoleDto roleDto, Role role = null)
        {
            var isEdit = true;
            if (role == null)
            {
                isEdit = false;
                role = new Role();

            }

            role.Title = roleDto.Title;
            role.Description = roleDto.Description;

            return role;
        }



        public static void Mapping(Profile profile)
        {
            profile.CreateMap<PermissionDto, Permission>()
                .ForMember(u => u.RolePermissions, opt => opt.Ignore())
                .ReverseMap();
        }
    }
}
