using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FilmKupriyanov.Startup))]
namespace FilmKupriyanov
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
