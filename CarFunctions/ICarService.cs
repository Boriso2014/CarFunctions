using System;

namespace CarFunctions
{
    public interface ICarService
    {
        Car GetCar(Guid id);
    }
}
