using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BuildCompany.WebUI.Startup))]
namespace BuildCompany.WebUI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
