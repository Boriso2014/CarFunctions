using System;

namespace CarFunctions
{
    public class CarService : ICarService
    {
        public readonly ICarRepo _carRepo;

        public CarService(ICarRepo repo)
        {
            _carRepo = repo;
        }

        public Car GetCar(Guid id)
        {
            var car = _carRepo.GetCar(id);

            return car;
        }
    }
}
