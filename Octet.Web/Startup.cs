using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Octet.Web.Startup))]
namespace Octet.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
