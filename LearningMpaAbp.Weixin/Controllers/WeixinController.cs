using Abp;
using Abp.Application.Services.Dto;
using Abp.IO.Extensions;
using Abp.Web.Models;
using Abp.WebApi.Client;
using LearningMpaAbp.Weixin.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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
            return View();
        }

        #region Abp中的token认证方式（密码模式）       

        /// <summary>
        /// 请求webapi
        /// 本例请求的是固定获取用户列表的api
        /// </summary>
        /// <param name="url">获取用户列表的api</param>
        /// <returns></returns>
        public async Task<PartialViewResult> SendRequest(string url)
        {
            return await GetUserList(url);
        }

        /// <summary>
        /// 向服务器申请token
        /// </summary>
        /// <param name="url">申请token的地址</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public async Task<string> GetAuthToken(string url, string user, string pwd)
        {
            var tokenResult = await _abpWebApiClient.PostAsync<string>(url, new
            {
                TenancyName = "Default",
                UsernameOrEmailAddress = user,
                Password = pwd
            });

            //将token添加到请求头
            _abpWebApiClient.RequestHeaders.Add(new NameValue("Authorization", "Bearer " + tokenResult));

            return tokenResult;
        }

        #endregion

        #region Abp中的cookie认证方式

        public async Task<PartialViewResult> SendRequestBasedCookie(string loginUrl, string url)
        {
            CookieBasedAuth(loginUrl);
            return await GetUserList(url);
        }

        /// <summary>
        /// 使用cookie认证
        /// </summary>
        /// <param name="url"></param>
        private void CookieBasedAuth(string url)
        {
            _abpWebApiClient.RequestHeaders.Clear();

            var requestBytes = Encoding.UTF8.GetBytes("TenancyName=" + "Default" + "&UsernameOrEmailAddress=" + "admin" + "&Password=" + "123qwe");

            var request = WebRequest.CreateHttp(url);

            request.Method = WebRequestMethods.Http.Post;
            request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
            request.Accept = "application/json";
            request.CookieContainer = new CookieContainer();
            request.ContentLength = requestBytes.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(requestBytes, 0, requestBytes.Length);
                stream.Flush();

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    var responseString = Encoding.UTF8.GetString(response.GetResponseStream().GetAllBytes());
                    var ajaxResponse = JsonString2Object<AjaxResponse>(responseString);

                    if (!ajaxResponse.Success)
                    {
                        throw new Exception("Could not login. Reason: " + ajaxResponse.Error.Message + " | " + ajaxResponse.Error.Details);
                    }

                    _abpWebApiClient.Cookies.Clear();
                    foreach (Cookie cookie in response.Cookies)
                    {
                        _abpWebApiClient.Cookies.Add(cookie);
                    }
                }
            }
        }

        private static TObj JsonString2Object<TObj>(string str)
        {
            return JsonConvert.DeserializeObject<TObj>(str,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
        }
        #endregion

        private async Task<PartialViewResult> GetUserList(string url)
        {
            try
            {
                var users = await _abpWebApiClient.PostAsync<ListResultDto<UserListDto>>(url);

                return PartialView("_UserListPartial", users.Items);
            }
            catch (Exception e)
            {
                ViewBag.ErrorMessage = e.Message;
            }

            return null;
        }

    }
}