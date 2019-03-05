using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(LibCardApp.Startup))]
namespace LibCardApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
