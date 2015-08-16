using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HoltDan.Startup))]
namespace HoltDan
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
