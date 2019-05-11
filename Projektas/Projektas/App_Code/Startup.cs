using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Projektas.Startup))]
namespace Projektas
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
