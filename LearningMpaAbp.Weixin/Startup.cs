using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LearningMpaAbp.Weixin.Startup))]
namespace LearningMpaAbp.Weixin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
