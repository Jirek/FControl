using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FControl.Startup))]
namespace FControl
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
