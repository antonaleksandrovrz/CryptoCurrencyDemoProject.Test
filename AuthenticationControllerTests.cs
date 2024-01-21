using CryptoCurrencyDemoProject.Data.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace CryptoCurrencyDemoProject.Test
{
    public class AuthenticationControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly HttpClient _client;

        public AuthenticationControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var userLogin = new UserLogin { Username = "admin", Password = "1234" };
            var content = new StringContent(JsonConvert.SerializeObject(userLogin), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Authentication/Login", content);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var token = await response.Content.ReadAsStringAsync();
            Assert.NotNull(token);
            Assert.NotEmpty(token);
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsNotFound()
        {
            // Arrange
            var userLogin = new UserLogin { Username = "invalidUsername", Password = "invalidPassword" };
            var content = new StringContent(JsonConvert.SerializeObject(userLogin), Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Authentication/Login", content);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }
    }
}