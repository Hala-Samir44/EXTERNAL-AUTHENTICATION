using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EX_AUTH_BE.Enum
{
    public enum ResultStatusEnum
    {
        Failed,
        Succeeded,
        Warning,
        Validation,
        Exception,
        AlreadyExist,
        NotExist
    }
}
