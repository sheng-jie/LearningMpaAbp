using Abp;
using Abp.Application.Services.Dto;
using Abp.IO.Extensions;
using Abp.Timing;
using Abp.Web.Models;
using Abp.WebApi.Client;
using LearningMpaAbp.Weixin.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace LearningMpaAbp.Weixin.Controllers
{
    public class WeixinController : Controller
    {
        private readonly IAbpWebApiClient _abpWebApiClient;
        private string baseUrl = "http://shengjie.azurewebsites.net";
        private string loginUrl = "/account/login";
        private string webapiUrl = "/api/services/app/User/GetUsers";
        private string abpTokenUrl = "/api/Account/Authenticate";
        private string oAuthTokenUrl = "/oauth/token";
        private string user = "admin";
        private string pwd = "123qwe";

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
        public async Task<PartialViewResult> SendRequest()
        {
            var token = Request.Cookies["access_token"]?.Value;
            //将token添加到请求头
            _abpWebApiClient.RequestHeaders.Add(new NameValue("Authorization", "Bearer " + token));

            return await GetUserList(baseUrl + webapiUrl);
        }

        /// <summary>
        /// 向服务器申请token
        /// </summary>
        /// <param name="url">申请token的地址</param>
        /// <param name="user">用户名</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public async Task<string> GetAbpToken()
        {
            var tokenResult = await _abpWebApiClient.PostAsync<string>(baseUrl + abpTokenUrl, new
            {
                TenancyName = "Default",
                UsernameOrEmailAddress = user,
                Password = pwd
            });
            this.Response.SetCookie(new HttpCookie("access_token", tokenResult));
            return tokenResult;
        }

        #endregion

        #region Abp中的cookie认证方式

        public async Task<PartialViewResult> SendRequestBasedCookie()
        {
            CookieBasedAuth(baseUrl + loginUrl);
            return await GetUserList(baseUrl + webapiUrl);
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

        public async Task<string> GetOAuth2Token()
        {
            Uri uri = new Uri(baseUrl + oAuthTokenUrl);
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.None };

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    {"grant_type", "password"},
                    {"username", user },
                    {"password", pwd },
                    {"client_id", "app" },
                    {"client_secret", "app"},
                });

                //获取token保存到cookie，并设置token的过期日期                    
                var result = await client.PostAsync(uri, content);
                string tokenResult = await result.Content.ReadAsStringAsync();

                var tokenObj = (JObject)JsonConvert.DeserializeObject(tokenResult);
                string token = tokenObj["access_token"].ToString();
                string refreshToken = tokenObj["refresh_token"].ToString();
                long expires = Convert.ToInt64(tokenObj["expires_in"]);

                this.Response.SetCookie(new HttpCookie("access_token", token));
                this.Response.SetCookie(new HttpCookie("refresh_token", refreshToken));
                this.Response.Cookies["access_token"].Expires = Clock.Now.AddSeconds(expires);

                return tokenResult;
            }
        }

        public async Task<string> GetOAuth2TokenByRefreshToken(string refreshToken)
        {
            Uri uri = new Uri(baseUrl + oAuthTokenUrl);
            var handler = new HttpClientHandler() { AutomaticDecompression = DecompressionMethods.None };

            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = uri;
                client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var content = new FormUrlEncodedContent(new Dictionary<string, string>()
                {
                    {"grant_type", "refresh_token"},
                    {"refresh_token", refreshToken},
                    {"client_id", "app" },
                    {"client_secret", "app"},
                });

                //获取token保存到cookie，并设置token的过期日期                    
                var result = await client.PostAsync(uri, content);
                string tokenResult = await result.Content.ReadAsStringAsync();

                var tokenObj = (JObject)JsonConvert.DeserializeObject(tokenResult);
                string token = tokenObj["access_token"].ToString();
                string newRefreshToken = tokenObj["refresh_token"].ToString();
                long expires = Convert.ToInt64(tokenObj["expires_in"]);

                this.Response.SetCookie(new HttpCookie("access_token", token));
                this.Response.SetCookie(new HttpCookie("refresh_token", newRefreshToken));
                this.Response.Cookies["access_token"].Expires = Clock.Now.AddSeconds(expires);

                return tokenResult;
            }
        }

        public async Task<ActionResult> SendRequestWithOAuth2Token()
        {
            var token = Request.Cookies["access_token"]?.Value;
            if (token == null)
            {
                //throw new Exception("token已过期");
                string refreshToken = Request.Cookies["refresh_token"].Value;
                var tokenResult = await GetOAuth2TokenByRefreshToken(refreshToken);
                var tokenObj = (JObject)JsonConvert.DeserializeObject(tokenResult);
                token = tokenObj["access_token"].ToString();
            }

            _abpWebApiClient.RequestHeaders.Add(new NameValue("Authorization", "Bearer " + token));

            return await GetUserList(baseUrl + webapiUrl);
        }

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