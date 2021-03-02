using LibraryApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.Hosting;
using LibraryApi.Controllers;
using LibraryApi.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LibraryAPIIntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services => {
                var descriptor = services.Single(services =>
                    services.ServiceType == typeof(ILookupServerStatus)
                );
                services.Remove(descriptor);
                services.AddTransient<ILookupServerStatus, FakeServerStatus>();
            });
        }
    }

    public class FakeServerStatus : ILookupServerStatus
    {
        public StatusResponse GetStatus()
        {
            return new StatusResponse
            {
                Message = "Ryan was here",
                LastChecked = new DateTime(1969, 4, 20, 23, 59, 00)
            };
        }
    }
}
