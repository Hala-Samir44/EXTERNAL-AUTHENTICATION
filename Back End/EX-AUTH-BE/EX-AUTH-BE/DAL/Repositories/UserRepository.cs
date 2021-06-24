using EX_AUTH_BE.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EX_AUTH_BE.DAL.Repositories
{
    public class UserRepository : Repository<User>
    {
        public ExternalAuthContext Ctx { get; }
        public UserRepository(ExternalAuthContext context) : base(context)
        {
            Ctx = context;
        }

        public string GetUserNameByEmail(string email)
        {
            string firstname = Context.Users.FirstOrDefault(u => u.Email == email).FirstName;
            string lastname = Context.Users.FirstOrDefault(u => u.Email == email).LastName;
            return firstname + " " + lastname;
        }

        public async Task<User> GetUserByGoogleId(string id)
        {
            var externalLogin = await Context.ExternalLogins.SingleOrDefaultAsync(e => e.GoogleId == id);

            if (externalLogin == null)
            {
                return null;
            }

            var user = await GetByIdAsync(externalLogin.UserId);

            return externalLogin.User;
        }

    }
}
