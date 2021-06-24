using AutoMapper;
using EX_AUTH_BE.DAL.Repositories;
using EX_AUTH_BE.Dto;
using EX_AUTH_BE.Enum;
using EX_AUTH_BE.model;
using EX_AUTH_BE.Services;
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
    public class AuthenticationController : ControllerBase
    {
        private readonly UserService userService;

        public AuthenticationController(UserService userService)
        {
            this.userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<GenericResult<LoginDataReturn>> GoogleSignUp(ExternalLoginDto request)
        {
            var result = new GenericResult<LoginDataReturn>();

            var isRegistered = await userService.GoogleSignUp(request);
            if (isRegistered.status == ResultStatusEnum.AlreadyExist)
            {
                result.Status = ResultStatusEnum.AlreadyExist;
                result.Messages.Add("This Email Already Exist");
            }
            else if (isRegistered.status == ResultStatusEnum.Succeeded)
            {

                result.username = isRegistered.username;
                result.Data = isRegistered;
                result.Status = ResultStatusEnum.Succeeded;
                result.Messages.Add("Sign Up Succeeded");
            }
            else
            {
                result.Messages.Add("Sign Up Faild");

                result.Status = ResultStatusEnum.Failed;
            }

            return result;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<GenericResult<LoginDataReturn>> ExternalLogin(ExternalLoginDto token)
        {
            var result = new GenericResult<LoginDataReturn>();
            var loginDataReturn = await userService.LoginUserAsync(null, token);
            if (loginDataReturn.status == ResultStatusEnum.NotExist)
            {
                result.Status = ResultStatusEnum.AlreadyExist;
                result.Messages.Add("This Email Not Exist, Please Sign Up First");
            }
            else if (loginDataReturn.status == ResultStatusEnum.Succeeded)
            {
                result.Data = loginDataReturn;
                result.username = loginDataReturn.username;
                result.Messages.Add("Login Success");
                result.Status = ResultStatusEnum.Succeeded;
            }
            else
            {
                result.Messages.Add("Login Faild");
                result.Status = ResultStatusEnum.Failed;
            }

            return result;
        }


    }
}
