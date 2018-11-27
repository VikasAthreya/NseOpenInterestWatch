using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NseOpenInterestWatch.Startup))]
namespace NseOpenInterestWatch
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
