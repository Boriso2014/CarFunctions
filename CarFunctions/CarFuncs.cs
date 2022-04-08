using System;
using System.Net;
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
        public async Task<HttpResponseData> RunGetCar([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "cars/{id}")] HttpRequestData req, string id)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            var carId = new Guid(id); // fda8d667-f43c-4d49-afae-624b7caf0bd3
            var car = _carService.GetCar(carId);

            var response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(car);

            return response;
        }
    }
}
