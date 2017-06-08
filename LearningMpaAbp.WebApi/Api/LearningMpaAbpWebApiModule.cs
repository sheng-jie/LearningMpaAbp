using System.Linq;
using System.Reflection;
using System.Web.Http;
using Abp.Application.Services;
using Abp.Configuration.Startup;
using Abp.Modules;
using Abp.WebApi;
using Swashbuckle.Application;
using Abp.Json;

namespace LearningMpaAbp.Api
{
    [DependsOn(typeof(AbpWebApiModule), typeof(LearningMpaAbpApplicationModule))]
    public class LearningMpaAbpWebApiModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            Configuration.Modules.AbpWebApi().DynamicApiControllerBuilder
                .ForAll<IApplicationService>(typeof(LearningMpaAbpApplicationModule).Assembly, "app")
                .Build();
            //var cors = new EnableCorsAttribute("*", "*", "*");
            //GlobalConfiguration.Configuration.EnableCors(cors);

            Configuration.Modules.AbpWebApi().HttpConfiguration.Filters.Add(new HostAuthenticationFilter("Bearer"));

            ConfigureSwaggerUi();
        }


        private void ConfigureSwaggerUi()
        {
            Configuration.Modules.AbpWebApi().HttpConfiguration
                .EnableSwagger(c =>
                {
                    c.SingleApiVersion("V1", "LearingMpaAbp 项目API文档");
                    c.ResolveConflictingActions(apiDesc => apiDesc.First());

                    //var baseDictory = AppDomain.CurrentDomain.BaseDirectory;
                    //var commentsFileName = "bin//LearningMpaAbp.Application.XML";
                    //var commentsFile = Path.Combine(baseDictory, commentsFileName);
                    //c.IncludeXmlComments(commentsFile);
                })
                .EnableSwaggerUi(c =>
                {
                    c.InjectJavaScript(Assembly.GetAssembly(typeof(LearningMpaAbpWebApiModule)), "LearningMpaAbpWebApiModule.Api.Scripts.Swagger-Custom.js");
                });
        }

        public override void PostInitialize()
        {
            //Json时间格式化 该方法已失效
            //GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.DateFormatString =
            //    "yyyy-MM-dd HH:mm:ss";
            var converters = Configuration.Modules.AbpWebApi().HttpConfiguration.Formatters.JsonFormatter.SerializerSettings.Converters;
            foreach (var converter in converters)
            {
                if (converter is AbpDateTimeConverter)
                {
                    var tmpConverter = converter as AbpDateTimeConverter;
                    tmpConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
                }
            }
        }
    }
}