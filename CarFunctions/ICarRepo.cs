using System;

namespace CarFunctions
{
    public interface ICarRepo
    {
        Car GetCar(Guid id);
    }
}
