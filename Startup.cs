using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System.Linq;
using Turning.Services;
using WebWindows.Blazor;

namespace Turning
{
    public class Startup
    {

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<MapsService>();
            services.AddSingleton<NConfigGenerator>();
            services.AddSingleton<UserConfiguration>();
        }

        public void Configure(DesktopApplicationBuilder app)
        {
            app.AddComponent<App>("app");

            var userconfig = app.Services.GetService<UserConfiguration>();
            userconfig.Load();
            if (userconfig.StartNginxOnBoot)
            {
                NginxService.Start();
            }

            var nconfigGenerator = app.Services.GetService<NConfigGenerator>();
            nconfigGenerator.ModifyPatched += (s, e) =>
            {
                NginxService.Reload();
            };
            var mapsservice = app.Services.GetService<MapsService>();
            mapsservice.Updated += (s, e) =>
              {
                  nconfigGenerator.Apply(mapsservice
                      .GetList()
                      .Where(o => o.Enabled)
                      .ToArray());
              };

            mapsservice.Load();
        }
    }
}
