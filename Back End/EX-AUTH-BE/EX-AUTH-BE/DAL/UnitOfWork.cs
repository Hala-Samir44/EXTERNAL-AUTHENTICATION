using EX_AUTH_BE.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EX_AUTH_BE.DAL
{
    public class UnitOfWork
    {
        public UserRepository UserRepository { get; set; }
        public RoleRepository RoleRepository { get; set; }
        public PermissionRepository PermissionRepository { get; set; }
        public RolePermissionsRepository RolePermissionsRepository { get; set; }


        public UnitOfWork(UserRepository userRepository, RolePermissionsRepository rolePermissionsRepository, RoleRepository roleRepository, PermissionRepository permissionRepository)
        {
            UserRepository = userRepository;
            RoleRepository = roleRepository;
            PermissionRepository = permissionRepository;
            RolePermissionsRepository = rolePermissionsRepository;
        }

    }
}

