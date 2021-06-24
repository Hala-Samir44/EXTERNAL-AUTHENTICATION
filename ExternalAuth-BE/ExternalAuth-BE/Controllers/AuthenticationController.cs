using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExternalAuth_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost]
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
