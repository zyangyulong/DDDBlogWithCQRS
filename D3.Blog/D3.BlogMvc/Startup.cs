﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using AspectCore.Configuration;
using AspectCore.Extensions.DependencyInjection;
using D3.BlogMvc.Hubs;
using Infrastructure.Data.Database;
using Infrastructure.Identity.Data;
using Infrastructure.Identity.Models;
using Infrastructure.Logging;
using MediatR;
using MediatR.Pipeline;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.QQ;
using Microsoft.AspNetCore.Authentication.WeChat;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using AspectCore.DynamicProxy;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using D3.Blog.Application.Interface;
using D3.Blog.Application.Services.Articles;
using D3.Blog.Application.Services.Customer;
using D3.Blog.Domain.Infrastructure.IRepositorys;
using D3.BlogMvc.InitialSetup;
using D3.BlogMvc.Middlewares;
using Infrastructure.AOP;
using Infrastructure.Data.Repository.Repositorys;
using Infrastructure.Identity.Authorization;
using NLog.Extensions.Logging;
using NLog.Web;
using DependencyInjection;
using Infrastructure.Tools;

namespace D3.BlogMvc
{
    public class Startup
    {

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 读取配置文件
        /// </summary>
        public IConfiguration Configuration { get; }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {

            DiagnosticListener.AllListeners.Subscribe(new CommandListener());


            services.ServerDependencies(); //配置依赖注入  

//            services.ConfigureSeriLog(Configuration);//配置serilog

            services.AddAutoMapperSetup();

            #region IDentity
            services.AddDbContext<AppIdentityDbContext>(
                options =>options.UseMySql(Configuration.GetConnectionString("BLOG_MYSQL")));//MYSQL

//            services.AddDbContext<AppIdentityDbContext>(
//                options =>options.UseSqlServer(Configuration.GetConnectionString("ID_DBCONN")));//Sql

            services.AddIdentity<AppBlogUser, AppBlogRole>()
                    .AddEntityFrameworkStores<AppIdentityDbContext>()
                    .AddDefaultTokenProviders();
            #region Password Strength Setting
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit           = false;
                options.Password.RequiredLength         = 8;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase       = false;
                options.Password.RequireLowercase       = false;
                options.Password.RequiredUniqueChars    = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan  = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers      = true;
                 
                // User settings
                options.User.RequireUniqueEmail = true;
            }).ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath   = "/Account/AccessDenied";
                options.Cookie.Name        = "zylblog";
                options.Cookie.HttpOnly    = true;
                options.ExpireTimeSpan     = TimeSpan.FromMinutes(60);
                options.LoginPath          = "/Account/Login";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration  = true;
            });
            #endregion

            #endregion

            #region 身份验证 
            services.AddAuthentication().AddQQ(qqOptions =>
            {
                qqOptions.AppId = Configuration["Authentication:QQ:AppId"];
                qqOptions.AppKey = Configuration["Authentication:QQ:AppKey"];
            }).AddWeChat(wechatOptions =>
            {
                wechatOptions.AppId = Configuration["Authentication:WeChat:AppId"];
                wechatOptions.AppSecret = Configuration["Authentication:WeChat:AppSecret"];
            }).AddGoogle(googleOptions =>
                         {
                             googleOptions.ClientId = Configuration["Authentication:Google:ClientId"];
                             googleOptions.ClientSecret = Configuration["Authentication:Google:ClientSecret"];
                         });
            #endregion

            

            services.AddMemoryCache();
            services.AddMvc();
            services.AddSignalR(); //以后在线聊天可以使用



            
            #region Autofac  暂时当作service和repoitory的拦截器使用
            var builder = new ContainerBuilder();
            builder.Populate(services);//将原生的注入填充进去
            builder.RegisterType<BlogLogAOP>();//可以直接替换其他拦截器
            builder.RegisterType<BlogCacheAOP>();
            builder.ServerDependenciesAutofac();
            var applicationContainer = builder.Build();//构建新容器
            #endregion

            return new AutofacServiceProvider(applicationContainer);//新容器
        }

        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //使用NLog作为日志记录工具
            loggerFactory.AddNLog();
            //引入Nlog配置文件
            env.ConfigureNLog("Nlog.config");  

            if (env.IsDevelopment())
            {
                //开发环境异常
                app.UseDeveloperExceptionPage();
//                app.UseExceptionHandler("/Exception/Index");
            }
            else
            {            
                app.UseExceptionHandler("/Exception/Index");
                app.UseHsts();
            }
            

            app.UseStaticFiles();
            app.UseStatusCodePagesWithRedirects("/Exception/ErrorStatusCode/{0}");//http 错误状态码页面
            app.UseHttpsRedirection();

            
            app.UseAuthentication();
           
            app.UseCustomerAuth();//自定义异地登录处理

            //实时通信使用
            app.UseSignalR(routes =>
            {
                routes.MapHub<D3BlogHub>("/D3BlogHub");
            });

            app.UseMvc(router =>
            {
                
                router.MapRoute(
                    name: "Default",
                    template: "{controller=Home}/{action=HomePage}/{id?}");
                router.MapAreaRoute(
                    name: "area",
                    template: "{controller=Admin}/{action=Index}/{id?}",
                    areaName:"Admin");
                
            });
        }
    }
}
