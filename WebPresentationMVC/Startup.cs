using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebPresentationMVC.Startup))]
namespace WebPresentationMVC
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}