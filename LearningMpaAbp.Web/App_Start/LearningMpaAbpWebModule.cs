using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Abp.BackgroundJobs;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.Zero.Configuration;
using Abp.Modules;
using Abp.Runtime.Caching.Redis;
using Abp.Threading.BackgroundWorkers;
using Abp.Web.Mvc;
using Abp.Web.SignalR;
using LearningMpaAbp.Api;
using Hangfire;
using LearningMpaAbp.Workers;

namespace LearningMpaAbp.Web
{
    [DependsOn(
        typeof(LearningMpaAbpDataModule),
        typeof(LearningMpaAbpApplicationModule),
        typeof(LearningMpaAbpWebApiModule),
        typeof(AbpWebSignalRModule),
        //typeof(AbpHangfireModule), - ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
        typeof(AbpWebMvcModule),
        typeof(AbpRedisCacheModule))]
    public class LearningMpaAbpWebModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Enable database based localization
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            //Configure navigation/menu
            Configuration.Navigation.Providers.Add<LearningMpaAbpNavigationProvider>();

            //Configure Hangfire - ENABLE TO USE HANGFIRE INSTEAD OF DEFAULT JOB MANAGER
            //Configuration.BackgroundJobs.UseHangfire(configuration =>
            //{
            //    configuration.GlobalConfiguration.UseSqlServerStorage("Default");
            //});

            //配置使用Redis缓存
            //Configuration.Caching.UseRedis();

            //配置所有Cache的默认过期时间为2小时
            Configuration.Caching.ConfigureAll(cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromHours(2);
            });

            //配置指定的Cache过期时间为10分钟
            Configuration.Caching.Configure("ControllerCache", cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(10);
            });
            
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
