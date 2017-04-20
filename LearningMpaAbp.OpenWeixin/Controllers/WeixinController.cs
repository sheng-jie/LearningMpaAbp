using Abp;
using Abp.Application.Services.Dto;
using Abp.WebApi.Client;
using LearningMpaAbp.Weixin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LearningMpaAbp.Weixin.Controllers
{
    public class WeixinController : Controller
    {
        private readonly IAbpWebApiClient _abpWebApiClient;

        public WeixinController()
        {
            _abpWebApiClient = new AbpWebApiClient();
        }

        // GET: Wexin
        public async Task<ActionResult> Index()
        {
            string baseUrl = "http://localhost:61759";
            try
            {
                var tokenResult = await GetAuthToken(baseUrl + "/api/Account/Authenticate");
                _abpWebApiClient.RequestHeaders.Add(new NameValue("Authorization", "Bearer " + tokenResult));

                var users = await _abpWebApiClient.PostAsync<ListResultDto<UserListDto>>(baseUrl + "/api/services/app/User/GetUsers");
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }
            return View();
        }


        private async Task<string> GetAuthToken(string url)
        {
            var tokenResult = await _abpWebApiClient.PostAsync<string>(url, new
            {
                TenancyName = "Default",
                UsernameOrEmailAddress = "Admin",
                Password = "123qwe"
            });

            return tokenResult;
        }
    }
}