using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AssetManagerMvc.Startup))]
namespace AssetManagerMvc
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
