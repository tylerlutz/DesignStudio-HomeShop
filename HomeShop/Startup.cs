using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HomeShop.Startup))]
namespace HomeShop
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
