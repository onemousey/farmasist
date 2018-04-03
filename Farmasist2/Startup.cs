using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Farmasist2.Startup))]
namespace Farmasist2
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
