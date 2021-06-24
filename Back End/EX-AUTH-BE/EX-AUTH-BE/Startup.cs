using AutoMapper;
using EX_AUTH_BE.DAL;
using EX_AUTH_BE.DAL.Repositories;
using EX_AUTH_BE.model;
using EX_AUTH_BE.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace EX_AUTH_BE
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContextPool<ExternalAuthContext>(
                    options => options.UseSqlServer(Configuration.GetConnectionString("ExternalAuth")));
            services.AddCors();

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddScoped(provider => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingProfile());
            }).CreateMapper());



            // Adding Authentication  
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })

            // Adding Jwt Bearer  
            .AddJwtBearer(options =>
            {
                options.SaveToken = true;
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration.GetSection("Token:Secrete").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            //.AddFacebook(facebookOptions =>
            //{
            //    facebookOptions.AppId = Configuration["AuthenticationFaceBook:AppId"];
            //    facebookOptions.AppSecret = Configuration["AuthenticationFaceBook:AppSecret"];
            //})
            ;




            services.AddAuthorization();
            //services.AddAuthentication();


            //services.AddTransient<IConfiguration>();
            //services.AddTransient<IMapper>();
            services.AddTransient<UserService>();
            services.AddTransient<UnitOfWork>();
            services.AddTransient<AuthenticationRepository>();
            services.AddTransient<UserRepository>();
            services.AddTransient<RoleRepository>();
            services.AddTransient<PermissionRepository>();
            services.AddTransient<RolePermissionsRepository>();
            services.AddTransient<externalLoginRepository>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors(builder =>
                      builder.AllowAnyOrigin()
                      .AllowAnyHeader()
                      .AllowAnyMethod());

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
