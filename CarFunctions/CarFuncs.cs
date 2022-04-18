using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace CarFunctions
{
    public class CarFuncs
    {
        private readonly ILogger _logger;
        private readonly ICarService _carService;

        public CarFuncs(ICarService carService, ILoggerFactory loggerFactory)
        {
            _carService = carService;
            _logger = loggerFactory.CreateLogger<CarFuncs>();
        }

        [Function("GetCar")]
        public async Task<HttpResponseData> GetCar([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "cars/{id}")] HttpRequestData req, string id)
        {
            _logger.LogInformation("C# GetCar function is starting by HTTP trigger.");

            var carId = new Guid(id); // fda8d667-f43c-4d49-afae-624b7caf0bd3
            var car = _carService.GetCar(carId);
            var json = JsonSerializer.Serialize(car);

            var response = req.CreateResponse(HttpStatusCode.OK);
            response.Headers.Add("Content-Type", "application/json");
            await response.WriteStringAsync(json, Encoding.UTF8);

            return response;
        }
    }
}
