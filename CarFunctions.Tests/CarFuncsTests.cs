using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

using Moq;

using NUnit.Framework;

namespace CarFunctions.Tests
{
    public sealed class CarFuncsTests
    {
        private readonly Mock<ILoggerFactory> _mockLoggerFactory;
        private readonly Mock<FunctionContext> _functionContext;
        private readonly Mock<ICarService> _carService;

        public CarFuncsTests()
        {
            _mockLoggerFactory = new Mock<ILoggerFactory>();
            _functionContext = new Mock<FunctionContext>();
            _carService = new Mock<ICarService>();
        }

        [SetUp]
        public void Setup()
        {
            var mockLogger = new Mock<ILogger<CarFuncs>>();
            mockLogger.Setup(
                m => m.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()));

            _mockLoggerFactory.Setup(x => x.CreateLogger(It.IsAny<string>())).Returns(() => mockLogger.Object);
        }

        [Test]
        public async Task should_return_correct_car_object()
        {
            // Arrange
            var car = new Car()
            {
                Id = Guid.NewGuid(),
                Model = "Lada"
            };
            _carService.Setup(x => x.GetCar(It.IsAny<Guid>())).Returns(car);
            
            var request = GetHttpRequestData(_functionContext.Object, "get");
            var func = new CarFuncs(_carService.Object, _mockLoggerFactory.Object);
            var carId = "fda8d667-f43c-4d49-afae-624b7caf0bd3";

            // Act
            var result = await func.RunGetCar(request, carId);

            result.Body.Position = 0;
            var reader = new StreamReader(result.Body);
            var responseBody = reader.ReadToEnd();
            var responseCar = JsonSerializer.Deserialize<Car>(responseBody);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
            Assert.That(responseCar, Is.Not.Null);
            Assert.That(responseCar, Is.TypeOf<Car>());
            Assert.That(responseCar?.Id, Is.EqualTo(car.Id));
            Assert.That(responseCar?.Model, Is.EqualTo(car.Model));
        }

        private static HttpRequestData GetHttpRequestData(FunctionContext functionContext, string method, string? content = null, string? url = null)
        {
            var request = new Mock<HttpRequestData>(functionContext);
            request.Setup(r => r.Method).Returns(method);

            if (content != null)
            {
                var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
                request.Setup(r => r.Body).Returns(stream);
            }

            if (url != null)
            {
                request.Setup(r => r.Url).Returns(new Uri(url));
            }

            request.Setup(r => r.CreateResponse()).Returns(() =>
            {
                var response = new Mock<HttpResponseData>(functionContext);
                response.SetupProperty(r => r.Headers, new HttpHeadersCollection());
                response.SetupProperty(r => r.StatusCode);
                response.SetupProperty(r => r.Body, new MemoryStream());
                return response.Object;
            });

            return request.Object;
        }
    }
}
