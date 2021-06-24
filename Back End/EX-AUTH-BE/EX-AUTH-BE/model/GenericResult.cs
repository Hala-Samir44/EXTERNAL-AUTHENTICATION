using EX_AUTH_BE.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EX_AUTH_BE.model
{
    public class GenericResult<T>
    {
        public GenericResult()
        {
            Messages = new List<string>();
        }

        public T Data { get; set; }
        public string username { get; set; }
        public List<T> ListOfData { get; set; }
        // public PagedList<T> Pages { get; set; }
        public List<string> Messages { get; set; }
        public ResultStatusEnum Status { get; set; }
    }
}
