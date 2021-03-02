using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryAPIIntegrationTests
{
    public class GettingTheServerStatus : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        public GettingTheServerStatus(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateDefaultClient();
        }

        [Fact]
        public async Task HasOkStatus()
        {
            var response = await _client.GetAsync("/status");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task IsEncodedAsJson()
        {
            var response = await _client.GetAsync("/status");
            Assert.Equal("application/json", response.Content.Headers.ContentType.MediaType);
        }
    }
}
