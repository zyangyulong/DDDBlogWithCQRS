﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using D3.Blog.Application.Interface;
using D3.Blog.Application.ViewModels;
using D3.Blog.Domain.Core.Notifications;
using Infrastructure.Logging;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Identity;
using Infrastructure.Data.Database;
using Infrastructure.Identity.Models;
using Infrastructure.NLoger;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace D3.BlogMvc.Controllers
{
    public class HomeController : BaseController
    {
        private readonly IArticleService _articleService;
        public HomeController(
            IArticleService articleService,
            UserManager<AppBlogUser> userManager, 
            RoleManager<AppBlogRole> roleManager, 
            SignInManager<AppBlogUser> signInManager, 
            INotificationHandler<DomainNotification> notifications,
            ICustomerLogging _logger)
           : base(userManager, roleManager, signInManager, notifications,_logger)
        {
            _articleService = articleService;
        }

        /// <summary>
        /// 目前测试命令模式
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            ViewBag.num = "111";
            ViewBag.container = "container";//写文章页面和其他页面的样式控制
//            var model = await _customerService.GetById(1);

            //CustomerViewModel model = new CustomerViewModel();
            //model.Name = "张玉龙";
            //model.Email = "yulong0204@qq.com";
            //model.BirthDate = DateTime.Parse("1995-10-02");
            //_customerService.Add(model);

            //var error = _notifications.GetNotifications().Select(n => n.Value);//通知结果

            //_logger.CustomInformation(user: "小王", enviornment: "Dev", informationMessage: "添加数据成功");
            //_logger.CustomError(errorMessage: "Data Critical Added Successfully");
            //_logger.CustomFatal(fatalMessage: "Data Error Added Successfully");

            return Json("1212");
        }


        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public IActionResult HomePage()
        {
            ViewBag.title = "YLBlog-首页";
            ViewBag.container = "container";//写文章页面和其他页面的样式控制

//            var postlist= _articleService.GetListByPage(10, 1, null, a => a.AddTime).ToList();

            return View();
        }

        [HttpGet]
        public IActionResult GetPostList()
        {
            var postlist= _articleService.GetListByPage(10, 1, null, a => a.AddTime).ToList();
            return Json(postlist);
        }

        
        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="searchtext"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public IActionResult Search([FromForm]string searchtext)
        {
            ViewBag.title = "YLBlog-搜索结果";
            ViewBag.searchKeyWord=searchtext;    
            ViewBag.container = "container";//写文章页面和其他页面的样式控制
            return View();
        }

        /// <summary>
        /// 异地登录提示页面
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult NewAreaLogin()
        {
            ViewBag.NewAreaLogin = true;//异地登录标志
            return View("HomePage");
        }


    }
}