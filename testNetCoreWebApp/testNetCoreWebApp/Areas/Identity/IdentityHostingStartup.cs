using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using testNetCoreWebApp.Areas.Identity.Data;
using testNetCoreWebApp.Data;

[assembly: HostingStartup(typeof(testNetCoreWebApp.Areas.Identity.IdentityHostingStartup))]
namespace testNetCoreWebApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<DPContext>(options =>
                    options.UseSqlServer(
                        context.Configuration.GetConnectionString("DPContextConnection")));

                services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
                    .AddEntityFrameworkStores<DPContext>();
            });
        }
    }
}