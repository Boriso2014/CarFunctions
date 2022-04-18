using System;

namespace CarFunctions
{
    /// <summary>
    /// Get cars
    /// </summary>
    public interface ICarRepo
    {
        Car GetCar(Guid id);
    }
}
