using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Abp.Modules;
using Abp.WebApi;

namespace LearningMpaAbp.Weixin
{
    [DependsOn(typeof(AbpWebApiModule))]
    public class LearningMpaAbpWeixinModule:AbpModule
    {
        /// <summary>
        /// 预初始化，通常是用来配置框架以及其它模块
        /// </summary>
        public override void PreInitialize()
        {
            base.PreInitialize();
        }

        /// <summary>
        /// 初始化，一般用来依赖注入的注册
        /// </summary>
        public override void Initialize()
        {
            //把当前程序集的特定类或接口注册到依赖注入容器中
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }

        /// <summary>
        /// 提交初始化，一般用来解析依赖关系
        /// </summary>
        public override void PostInitialize()
        {
            base.PostInitialize();
        }

        /// <summary>
        /// 应用关闭时调用
        /// </summary>
        public override void Shutdown()
        {
            base.Shutdown();
        }
    }
}