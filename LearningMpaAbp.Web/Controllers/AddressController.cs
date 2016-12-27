using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.IO;

namespace LearningMpaAbp.Web.Controllers
{
    /// <summary>
    /// 完成省市县下拉框联动
    /// </summary>
    public class AddressController : LearningMpaAbpControllerBase
    {
        private string areaJsonPath = System.AppDomain.CurrentDomain.BaseDirectory + @"\js\Area.json";

        // GET: Address
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取省份数据
        /// </summary>
        /// <returns>省份数据</returns>
        public JsonResult GetProvinces()
        {
            List<AreaValue> provinces = new List<AreaValue>();

            using (StreamReader sr = new StreamReader(areaJsonPath))
            {
                try
                {
                    JsonSerializer serializer = new JsonSerializer();
                    //serializer.Converters.Add(new JavaScriptDateTimeConverter());
                    //serializer.NullValueHandling = NullValueHandling.Ignore;

                    //构建Json.net的读取流  
                    JsonReader reader = new JsonTextReader(sr);
                    //对读取出的Json.net的reader流进行反序列化，并装载到模型中  
                    var areas = serializer.Deserialize<List<Area>>(reader);

                    foreach (var area in areas)
                    {
                        if (area.Level == 1)
                        {
                            provinces.Add(new AreaValue { Id = area.Code, Name = area.Name });
                        }
                    }

                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            }

            return AbpJson(provinces, behavior: JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取城市数据
        /// </summary>
        /// <returns>城市数据</returns>
        public JsonResult GetCities(string provinceCode)
        {
            List<AreaValue> cities = new List<AreaValue>();

            using (StreamReader sr = new StreamReader(areaJsonPath))
            {
                try
                {
                    JsonSerializer serializer = new JsonSerializer();
                    //serializer.Converters.Add(new JavaScriptDateTimeConverter());
                    //serializer.NullValueHandling = NullValueHandling.Ignore;

                    //构建Json.net的读取流  
                    JsonReader reader = new JsonTextReader(sr);
                    //对读取出的Json.net的reader流进行反序列化，并装载到模型中  
                    var areas = serializer.Deserialize<List<Area>>(reader);

                    var province = areas.FirstOrDefault(a => a.Code == provinceCode);

                    foreach (var area in areas)
                    {
                        if (area.Level == 2 && area.Province == province.Province)
                        {
                            cities.Add(new AreaValue { Id = area.Code, Name = area.Name });
                        }
                    }

                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            }

            return AbpJson(cities, behavior: JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// 获取县级数据
        /// </summary>
        /// <returns>县级数据</returns>
        public JsonResult GetCounties(string cityCode)
        {            
            List<AreaValue> counties = new List<AreaValue>();

            using (StreamReader sr = new StreamReader(areaJsonPath))
            {
                try
                {
                    JsonSerializer serializer = new JsonSerializer();
                    //serializer.Converters.Add(new JavaScriptDateTimeConverter());
                    //serializer.NullValueHandling = NullValueHandling.Ignore;

                    //构建Json.net的读取流  
                    JsonReader reader = new JsonTextReader(sr);
                    //对读取出的Json.net的reader流进行反序列化，并装载到模型中  
                    var areas = serializer.Deserialize<List<Area>>(reader);

                    var city = areas.FirstOrDefault(a => a.Code == cityCode);

                    foreach (var area in areas)
                    {
                        if (area.Level == 3 && area.Province == city.Province && area.City == city.City)
                        {
                            counties.Add(new AreaValue { Id = area.Code, Name = area.Name });
                        }
                    }

                }
                catch (Exception ex)
                {
                    ex.Message.ToString();
                }
            }

            return AbpJson(counties, behavior: JsonRequestBehavior.AllowGet);
        }
    }

    public class Area
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string County { get; set; }
    }

    public class AreaValue
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }


}