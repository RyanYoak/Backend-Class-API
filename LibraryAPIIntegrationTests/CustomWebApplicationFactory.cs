using LibraryApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LibraryAPIIntegrationTests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
    }
}
