using AutoMapper;
using EX_AUTH_BE.DAL.Repositories;
using EX_AUTH_BE.Dto;
using EX_AUTH_BE.Enum;
using EX_AUTH_BE.model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace EX_AUTH_BE.Services
{
    public class UserService
    {
        string UserName;
        private readonly AuthenticationRepository authentication;
        private UserRepository userRepository;
        private externalLoginRepository externalLoginRepository;
        private readonly IMapper _mapper;

        private readonly IConfiguration configuration;



        public UserService(IConfiguration configuration, IMapper mapper, AuthenticationRepository authentication, UserRepository userRepository, externalLoginRepository externalLoginRepository)
        {
            this._mapper = mapper;
            this.userRepository = userRepository;
            this.externalLoginRepository = externalLoginRepository;
            this.authentication = authentication;
            this.configuration = configuration;
        }

        public async Task<LoginDataReturn> GoogleSignUp(ExternalLoginDto request)
        {
            Payload payload;

            try
            {
                var result = new LoginDataReturn();
                payload = await ValidateAsync(request.Token, new ValidationSettings
                {
                    Audience = new[] { "933409184992-pbdbfktt8ibd0h9ca68es6m36tfralha.apps.googleusercontent.com" },
                });

                var userExist = await authentication.UserExists(payload.Email);
                if (userExist)
                {
                    result.status = ResultStatusEnum.AlreadyExist;
                    return result;
                }

                var registerDto = new RegisterDto
                {
                    Email = payload.Email,
                    FirstName = payload.GivenName,
                    LastName = payload.FamilyName,
                    Password = payload.Subject,
                };
                ExtenalLoginModel extenalLoginModel = new ExtenalLoginModel
                {
                    ExternalLoginId = payload.Subject,
                    ExternalLogintType = request.ExternalLoginType,
                };
                result = await RegisterUserAsync(registerDto, extenalLoginModel);
                result.status = ResultStatusEnum.Succeeded;
                return result;
            }
            catch (Exception e)
            {
                throw new Exception("Invalid Google user token", e);
            }
        }

        public async Task<LoginDataReturn> RegisterUserAsync(RegisterDto registerDto, ExtenalLoginModel extenalLoginModel = null)
        {
            bool result = true;
            try
            {
                var userToCreate = CreateUser(registerDto);
                userToCreate.ProfilePictureURL = registerDto.ProfilePictureURL;
                User createdUser;

                createdUser = await authentication.Register(userToCreate, registerDto.Password);


                if (extenalLoginModel != null)
                {
                    if (extenalLoginModel.ExternalLogintType == "google")
                    {
                        var externalLogin = new ExternalLogins
                        {
                            User = createdUser,
                            GoogleId = extenalLoginModel.ExternalLoginId,
                        };
                        await externalLoginRepository.AddAsync(externalLogin);
                    }
                    //else if (extenalLoginModel.ExternalLogintType == "facebook")
                    //{
                    //    var externalLogin = new ExternalLogins
                    //    {
                    //        User = createdUser,
                    //        FacebookId = extenalLoginModel.ExternalLoginId,
                    //    };

                    //    await externalLoginRepository.AddAsync(externalLogin);
                    //}
                }

                LoginDataReturn res = null;
                if (result)
                {
                    var templogin = new LoginDto();
                    templogin.Email = registerDto.Email;
                    templogin.Password = registerDto.Password;

                    res = await LoginUserAsync(templogin);
                }

                var claims = authentication.DecodeToken(res.token);
                var id = claims.First(claim => claim.Type == "nameid").Value;
                int userId = int.Parse(id);
                return res;
            }

            catch (Exception e)
            {
                throw new Exception();
            }

        }

        public User CreateUser(RegisterDto registerDto)
        {
            var user = _mapper.Map<User>(registerDto);

            return user;
        }

        public static void Mapping(Profile profile)
        {
            profile.CreateMap<RegisterDto, User>()
                .ForMember(u => u.Password, opt => opt.Ignore())
                .ReverseMap();
        }

        public async Task<LoginDataReturn> LoginUserAsync(LoginDto loginDto = null, ExternalLoginDto externalLogin = null)
        {
            LoginDataReturn data = new LoginDataReturn();
            User userFromDB = null;
            if (loginDto != null)
            {
                userFromDB = await authentication.Login(loginDto.Email, loginDto.Password);

                if (userFromDB == null)
                    return null;
            }
            else if (externalLogin.ExternalLoginType.ToLower().Equals("google"))
            {
                userFromDB = await LoginByGoogle(externalLogin.Token);
                if (userFromDB == null)
                {
                    data.status = ResultStatusEnum.NotExist;
                    return data;
                }
            }
            //else if (externalLogin.ExternalLoginType.ToLower() == "facebook")
            //{
            //    userFromDB = await LoginByFb(externalLogin.Token);
            //    if (userFromDB == null)
            //        return null;
            //}

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userFromDB.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromDB.Email),
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Token:Secrete").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds,
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            if (tokenDescriptor != null && loginDto != null)
            {
                UserName = userRepository.GetUserNameByEmail(loginDto.Email);
            }
            else
            {
                UserName = userFromDB.FirstName + " " + userFromDB.LastName;
            }

            var token = tokenHandler.CreateToken(tokenDescriptor);


            data.status = ResultStatusEnum.Succeeded;
            data.username = UserName;
            data.token = tokenHandler.WriteToken(token);
            data.ExpiresIn = 86400;
            return data;
        }


        public async Task<User> LoginByGoogle(string token)
        {
            /*            userName = userName.ToLower();*/
            Payload payload;
            User user;
            try
            {
                payload = await ValidateAsync(token, new ValidationSettings
                {
                    Audience = new[] { "933409184992-pbdbfktt8ibd0h9ca68es6m36tfralha.apps.googleusercontent.com" },
                });

                user = await userRepository.GetUserByGoogleId(payload.Subject);
            }
            catch (Exception e)
            {
                throw new Exception("Invalid Google user token", e);
            }

            return user;
        }



    }
}
