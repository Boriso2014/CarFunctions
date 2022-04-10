using System;

namespace CarFunctions
{
    public class CarRepo : ICarRepo
    {
        public Car GetCar(Guid id)
        {
            var car = new Car()
            {
                Id = id,
                Model = "Nissan"
            };

            return car;
        }
    }
}
