using System;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Xpandion.Data;

namespace Xpandion.WebSite
{
    public class Startup
    {
        private IConfiguration _configuration;
        private IWebHostEnvironment _environment;

        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string connectionString = _configuration.GetConnectionString("XpandionConnection");

            services.AddDbContext<XpandionContext>(options => options.UseSqlServer(connectionString));
            services.AddControllersWithViews();
            services.AddRazorPages();
            //services.AddSignalR();
        }
        public void Configure(IApplicationBuilder app)
        {
            app.UseDeveloperExceptionPage();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Main}/{action=Index}");
                endpoints.MapRazorPages();
                //endpoints.MapHub<MainHub>("/mainHub");
            });
        }
    }
}
