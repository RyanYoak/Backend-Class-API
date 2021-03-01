using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace LibraryAPIIntegrationTests
{
    public class ResourceSmokeTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly HttpClient _client;
        public ResourceSmokeTests(CustomWebApplicationFactory factory)
        {
            _client = factory.CreateDefaultClient();
        }

        [Theory]
        [InlineData("status")]
        [InlineData("cutomers/42")]
        [InlineData("blogs/2021/3/1")]
        public async Task ResourcesAreAliveAndKicking(string resource)
        {
            var response = await _client.GetAsync(resource);
            Assert.True(response.IsSuccessStatusCode);
        }
    }
}
