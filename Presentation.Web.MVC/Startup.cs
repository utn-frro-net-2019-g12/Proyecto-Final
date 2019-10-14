using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Presentation.Web.MVC.Startup))]
namespace Presentation.Web.MVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}