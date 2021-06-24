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
    public class PermissionController : ControllerBase
    {
        public UnitOfWork unitOfWork { get; set; }
        public PermissionController(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<GenericResult<List<Permission>>> GetAllPermissions()
        {
            var result = new GenericResult<List<Permission>>();

            var permisssions = await unitOfWork.PermissionRepository.GetAllAsync();

            if (permisssions != null)
            {
                result.Data = permisssions;
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
        public async Task<GenericResult<Permission>> AddPermission(PermissionDto permissionDto)
        {
            var result = new GenericResult<Permission>();

            var permission = await unitOfWork.PermissionRepository.AddOrEditPermission(permissionDto);
            var c = await unitOfWork.PermissionRepository.AddAsync(permission);
            if (c != null)
            {
                result.Messages.Add("Added Permission Successfully");
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
        public async Task<GenericResult<Permission>> EditPermission(PermissionDto permissionDto)
        {
            var result = new GenericResult<Permission>();
            var oldPermission = await unitOfWork.PermissionRepository.FirstOrDefaultAsync(e => e.Id == permissionDto.Id);
            var permission = await unitOfWork.PermissionRepository.AddOrEditPermission(permissionDto, oldPermission);
            var c = unitOfWork.PermissionRepository.Update(permission);

            if (c != null)
            {
                result.Messages.Add("Modified Permission Successfully");
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
        public async Task<GenericResult<bool>> DeletePermission(PermissionDto permissionDto)
        {
            var result = new GenericResult<bool>();
            var oldPermission = await unitOfWork.PermissionRepository.FirstOrDefaultAsync(e => e.Id == permissionDto.Id);
            var permission = unitOfWork.PermissionRepository.Remove(oldPermission);

            if (permission != null)
            {
                result.Messages.Add("Deleted Permission Successfully");
                result.Status = ResultStatusEnum.Succeeded;

            }
            else
            {
                result.Messages.Add("Some Thing went Wrong ");

                result.Status = ResultStatusEnum.Failed;
            }

            return result;
        }




    }
}
