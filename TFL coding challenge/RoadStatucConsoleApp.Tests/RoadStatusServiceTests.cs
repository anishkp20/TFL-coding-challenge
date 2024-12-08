using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using RoadStatucConsoleApp.Interface;
using RoadStatucConsoleApp.Models;
using RoadStatucConsoleApp.Service;
using System.Net;

namespace RoadStatucConsoleApp.Tests
{
    [TestFixture]
    public class RoadStatusServiceTests
    {
        public Mock<HttpMessageHandler> _httpMessageHandlerMock;
        private IRoadStatusService _service;
        private HttpClient _httpClient;
        private string _appId;
        private string _appKey;
        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) // Set the base path for configuration files
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
            _appId = configuration["APISettings:AppId"];
            _appKey = configuration["APISettings:AppKey"];
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock.Object);
            _service = new RoadStatusService(_appId, _appKey, _httpClient);
        }

        [Test]
        public async Task GetRoadStatusAsync_ValidRoadId()
        {
            string roadId = "A2";
            
            var jsonResponse = JsonConvert.SerializeObject(new List<RoadStatus> { 
                new RoadStatus { DisplayName = "A2", StatusSeverity = "Good", 
                    StatusSeverityDescription = "No Exceptional Delays" } });

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.OK, Content = new StringContent(jsonResponse) });

            var result = await _service.GetRoadStatusAsync(roadId);

            Assert.NotNull(result);
            Assert.AreEqual("A2", result.DisplayName);
            Assert.AreEqual("Good", result.StatusSeverity);
            Assert.AreEqual("No Exceptional Delays", result.StatusSeverityDescription);
        }

        [Test]
        public async Task GetRoadStatusAsync_InvalidRoadId()
        {
            string roadId = "A233";
            
            var jsonResponse = JsonConvert.SerializeObject(new RoadStatusError { 
                Message = $"The following road id is not recognised: {roadId}"});

            _httpMessageHandlerMock.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage { StatusCode = HttpStatusCode.NotFound, Content = new StringContent(jsonResponse) });

            var ex = Assert.ThrowsAsync<NotFoundException>(async () => await _service.GetRoadStatusAsync(roadId));
            Assert.That(ex.Message, Is.EqualTo($"The following road id is not recognised: {roadId}"));
        }

        [Test]
        public async Task GetRoadStatusAsync_UnexpectedError()
        {
            _httpMessageHandlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>()
            )
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Content = new StringContent("Internal Server Error")
            });

            var ex = Assert.ThrowsAsync<Exception>(async () =>
           await _service.GetRoadStatusAsync("test-road-id"));

            Assert.AreEqual("Unexpected error while accessing API.", ex.Message);
        }
    }
}