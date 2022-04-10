using System;

using Moq;

using NUnit.Framework;

namespace CarFunctions.Tests
{
    public sealed class CarServiceTests
    {
        private readonly Mock<ICarRepo> _repo;
        private CarService _service;

        public CarServiceTests()
        {
            _repo = new Mock<ICarRepo>();
            _service = new CarService(_repo.Object);
        }

        [Test]
        public void should_return_car()
        {
            // Arrange
            var car = new Car()
            {
                Id = Guid.NewGuid(),
                Model = "Lada"
            };
            _repo.Setup(x => x.GetCar(It.IsAny<Guid>())).Returns(car);

            // Act
            var result = _service.GetCar(car.Id);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf<Car>());
            Assert.That(result.Id, Is.EqualTo(car.Id));
            Assert.That(result.Model, Is.EqualTo(car.Model));
        }
    }
}