using EX_AUTH_BE.model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace EX_AUTH_BE.DAL.Repositories
{
    public class AuthenticationRepository
    {
        private UserRepository userRepository;

        public AuthenticationRepository(UserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        public async Task<bool> UserExists(string email)
        {
            try
            {
                var user = await userRepository.SingleOrDefaultAsync(e => e.Email == email);

                if (user == null)
                {
                    return false;
                }

                return true;
            }
            catch (Exception e)
            {
                throw new Exception();
            }


        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;

            CreatePasswordHash(password, out passwordHash, out passwordSalt);

            user.Password = passwordHash;
            user.PasswordSalt = passwordSalt;

            return await this.userRepository.AddAsync(user);
        }

        public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;

                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        public IEnumerable<Claim> DecodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var decodedToken = handler.ReadToken(token) as JwtSecurityToken;

            return decodedToken.Claims;
        }

        public async Task<User> Login(string userName, string password)
        {
            /*            userName = userName.ToLower();*/
            var user = await this.userRepository.FirstOrDefaultAsync(u => u.Email == userName);

            if (user == null || !VerifyPasswordHash(password, user.Password, user.PasswordSalt))
                return null;

            return user;
        }
        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedPasswordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedPasswordHash.Length; i++)
                {
                    if (computedPasswordHash[i] != passwordHash[i])
                        return false;
                }
            }

            return true;
        }




    }
}
