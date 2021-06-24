using AutoMapper;
using EX_AUTH_BE.DAL.Repositories;
using EX_AUTH_BE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EX_AUTH_BE.model
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            UserService.Mapping(this);
            RoleRepository.Mapping(this);
        }

    }
}
