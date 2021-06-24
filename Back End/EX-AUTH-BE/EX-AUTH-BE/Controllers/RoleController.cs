using EX_AUTH_BE.DAL;
using EX_AUTH_BE.Dto;
using EX_AUTH_BE.Enum;
using EX_AUTH_BE.model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EX_AUTH_BE.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RoleController : ControllerBase
    {
        public UnitOfWork unitOfWork { get; set; }
        public RoleController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<GenericResult<List<RoleDto>>> GetAllRoles()
        {
            var result = new GenericResult<List<RoleDto>>();

            var roles = await unitOfWork.RoleRepository.GetAllRoleWithPermisionAsync();

            if (roles != null)
            {
                result.Data = roles;
                result.Status = ResultStatusEnum.Succeeded;
            }
            else
            {
                result.Messages.Add("Some Thing went Wrong ");

                result.Status = ResultStatusEnum.Failed;
            }

            return result;
        }

        [HttpPost]
        public async Task<GenericResult<Role>> AddRole(RoleDto roleDto)
        {
            var result = new GenericResult<Role>();

            var role = await unitOfWork.RoleRepository.AddOrEditRole(roleDto);
            var c = await unitOfWork.RoleRepository.AddAsync(role);
            if (c != null)
            {
                roleDto.Permissions.ForEach(e =>
                {
                    var p = new RolePermissions() { roleId = c.Id, permissionId = e };
                    unitOfWork.RolePermissionsRepository.AddAsync(p);
                });
                result.Messages.Add("Added Role Successfully");
                result.Status = ResultStatusEnum.Succeeded;

            }
            else
            {
                result.Messages.Add("Some Thing went Wrong ");

                result.Status = ResultStatusEnum.Failed;
            }

            return result;
        }


        [HttpPost]
        public async Task<GenericResult<Role>> EditRole(RoleDto roleDto)
        {
            var result = new GenericResult<Role>();
            var oldRole = await unitOfWork.RoleRepository.FirstOrDefaultAsync(e => e.Id == roleDto.Id);
            var role = await unitOfWork.RoleRepository.AddOrEditRole(roleDto, oldRole);
            var c = unitOfWork.RoleRepository.Update(role);

            if (c != null)
            {
                var rp = unitOfWork.RolePermissionsRepository.GetAllById(e => (e.roleId == roleDto.Id && !roleDto.Permissions.Contains(e.permissionId)));
                unitOfWork.RolePermissionsRepository.RemoveRange(rp);
                var rpExist = unitOfWork.RolePermissionsRepository.GetAllById(e => e.roleId == roleDto.Id).Select(e => e.permissionId);
                roleDto.Permissions.ForEach(e =>
                {
                    if (!rpExist.Contains(e))
                    {
                        var p = new RolePermissions() { roleId = roleDto.Id, permissionId = e };
                        unitOfWork.RolePermissionsRepository.AddAsync(p);
                    }

                });
                result.Messages.Add("Modified Role Successfully");
                result.Status = ResultStatusEnum.Succeeded;

            }
            else
            {
                result.Messages.Add("Some Thing went Wrong ");

                result.Status = ResultStatusEnum.Failed;
            }

            return result;
        }


        [HttpPost]
        public async Task<GenericResult<bool>> DeleteRole(RoleDto roleDto)
        {
            var result = new GenericResult<bool>();
            var oldRole = await unitOfWork.RoleRepository.FirstOrDefaultAsync(e => e.Id == roleDto.Id);
            var role = unitOfWork.RoleRepository.Remove(oldRole);

            if (role != null)
            {
                var rps = unitOfWork.RolePermissionsRepository.GetAllById(e => e.roleId == roleDto.Id);
                unitOfWork.RolePermissionsRepository.RemoveRange(rps);

                result.Messages.Add("Deleted Role Successfully");
                result.Status = ResultStatusEnum.Succeeded;

            }
            else
            {
                result.Messages.Add("Some Thing went Wrong ");

                result.Status = ResultStatusEnum.Failed;
            }

            return result;
        }



        [HttpGet]
        [AllowAnonymous]
        public void test()
        {

        }

    }
}
