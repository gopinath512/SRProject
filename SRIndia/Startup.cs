﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using SRIndiaInfo_Services;
using SRIndia_Repository;
using SRIndia_Models;

namespace SRIndia
{
    public class Startup
    {
        
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            _Configuration = builder.Build();
        }
        public IConfigurationRoot _Configuration { get;  }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddIdentity<AppUser, IdentityRole>(config =>
            {
                config.User.RequireUniqueEmail = true;
                config.Password.RequiredLength = 8;
            }).AddEntityFrameworkStores<SRIndiaContext>();

            
            services.AddEntityFrameworkSqlServer().AddDbContext<SRIndiaContext>(opt => opt.UseSqlServer(_Configuration["ConnectionStrings:SRIndiaConnection"], b => b.MigrationsAssembly("SRIndia")));

            //var connectionString = Startup._Configuration["connectionStrings:cityInfoDBConnectionString"];
            //services.AddDbContext<SRIndiaContext>(o => o.UseSqlServer(connectionString));

            services.AddScoped<IMessageInfoRepository, MessageInfoRepository>();
            services.AddScoped<IUserInfoRepository, UserInfoRepository>();

            // Add framework services.
            services.AddCors(options => options.AddPolicy("Cors", builder =>
            {
                builder
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
            }));
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseStaticFiles(); // For the wwwroot folder
            app.UseStaticFiles(new StaticFileOptions()
            {
                FileProvider = new PhysicalFileProvider(
           Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "images")),
                RequestPath = new PathString("/Images")
            });

            app.UseDirectoryBrowser(new DirectoryBrowserOptions()
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot", "images")),
                RequestPath = new PathString("/Images")
            });
            loggerFactory.AddConsole(_Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            Mapper.Initialize(config =>
            {
                config.CreateMap<User, AppUser>().ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password)).ReverseMap();
                config.CreateMap<Message, MessageView > ();
                config.CreateMap<User, AppUser>();
                config.CreateMap<AppUser, EditProfileData > ();
            });

            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("this is the secret phrase"));

            app.UseJwtBearerAuthentication(new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingKey,
                    ValidateLifetime = false,
                    ValidateIssuer = false,
                    ValidateAudience = false
                }
            });
            app.UseCors("Cors");
            app.UseMvc();
            //SeedData(app.ApplicationServices.GetService<SRIndiaContext>());
        }

        public void SeedData(SRIndiaContext context)
        {
            context.Messages.Add(new Message
            {
                Owner = "John",
                Text = "hello"
            });
            context.Messages.Add(new Message
            {
                Owner = "Tim",
                Text = "Hi"
            });

            context.Users.Add(new AppUser { Email = "a", FirstName = "Tim", Password = "a", Id = "1" });

            context.SaveChanges();
        }
    }
}
