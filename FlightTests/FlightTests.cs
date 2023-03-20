using Domain.Constants;
using Domain.Entities.FlightAggregate;
using Domain.Exceptions;
using FluentAssertions;

namespace FlightTests
{
    [TestFixture]
    public class FlightTests
    {
        [Test]
        public void Constructor_ShouldSetProperties()
        {
            // Arrange
            var origin = "Origin";
            var destination = "Destination";
            var departure = DateTimeOffset.UtcNow.AddHours(1);
            var arrival = DateTimeOffset.UtcNow.AddHours(2);
            var status = FlightStatus.OnTime;

            // Act
            var flight = new Flight(origin, destination, departure, arrival, status);

            // Assert
            flight.Origin.Should().Be(origin);
            flight.Destination.Should().Be(destination);
            flight.Departure.Should().Be(departure);
            flight.Arrival.Should().Be(arrival);
            flight.Status.Should().Be(status);
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenOriginIsNull()
        {
            // Arrange
            var origin = null as string;
            var destination = "Destination";
            var departure = DateTimeOffset.UtcNow.AddHours(1);
            var arrival = DateTimeOffset.UtcNow.AddHours(2);
            var status = FlightStatus.OnTime;

            // Act
            Action action = () => new Flight(origin, destination, departure, arrival, status);

            // Assert
            action.Should().Throw<DomainInvalidStateException>()
                .WithMessage(new DomainInvalidStateException(nameof(Flight.Origin), ValidationMessages.DefaultValue).Message);
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenDepartureIsDefault()
        {
            // Arrange
            var origin = "Origin";
            var destination = "Destination";
            var departure = default(DateTimeOffset);
            var arrival = DateTimeOffset.UtcNow.AddHours(2);
            var status = FlightStatus.OnTime;

            // Act
            Action action = () => new Flight(origin, destination, departure, arrival, status);

            // Assert
            action.Should().Throw<DomainInvalidStateException>()
                .WithMessage(new DomainInvalidStateException(nameof(Flight.Departure), ValidationMessages.DefaultValue).Message);
        }

        [Test]
        public void OnTime_ShouldUpdateStatusAndOffsetTimes()
        {
            // Arrange
            var origin = "Origin";
            var destination = "Destination";
            var departure = DateTimeOffset.UtcNow.AddHours(1);
            var arrival = DateTimeOffset.UtcNow.AddHours(2);
            var status = FlightStatus.Delayed;
            var delayDeparture = TimeSpan.FromMinutes(10);
            var delayArrival = TimeSpan.FromMinutes(20);

            var flight = new Flight(origin, destination, departure, arrival, status);

            // Act
            flight.OnTime(delayDeparture, delayArrival);

            // Assert
            flight.Status.Should().Be(FlightStatus.OnTime);
            flight.Departure.Should().Be(departure.Add(delayDeparture));
            flight.Arrival.Should().Be(arrival.Add(delayArrival));
        }

        [Test]
        public void Delayed_ShouldUpdateStatusAndOffsetTimes()
        {
            // Arrange
            var origin = "Origin";
            var destination = "Destination";
            var departure = DateTimeOffset.UtcNow.AddHours(1);
            var arrival = DateTimeOffset.UtcNow.AddHours(2);
            var status = FlightStatus.OnTime;
            var delayDeparture = TimeSpan.FromMinutes(10);
            var delayArrival = TimeSpan.FromMinutes(20);

            var flight = new Flight(origin, destination, departure, arrival, status);

            // Act
            flight.Delayed(delayDeparture, delayArrival);

            // Assert
            flight.Status.Should().Be(FlightStatus.Delayed);
            flight.Departure.Should().Be(departure.Add(delayDeparture));
            flight.Arrival.Should().Be(arrival.Add(delayArrival));
        }

        [Test]
        public void Cancel_ShouldUpdateStatus()
        {
            // Arrange
            var origin = "Origin";
            var destination = "Destination";
            var departure = DateTimeOffset.UtcNow.AddHours(1);
            var arrival = DateTimeOffset.UtcNow.AddHours(2);
            var status = FlightStatus.OnTime;

            var flight = new Flight(origin, destination, departure, arrival, status);

            // Act
            flight.Cancel();

            // Assert
            flight.Status.Should().Be(FlightStatus.Cancelled);
        }

        [Test]
        public void OffsetDepartureAndArrival_ShouldThrowException_WhenDepartureGreaterOrEqualsThanArrival()
        {
            // Arrange
            var origin = "Origin";
            var destination = "Destination";
            var departure = DateTimeOffset.UtcNow;
            var arrival = DateTimeOffset.UtcNow.AddHours(1);
            var status = FlightStatus.OnTime;

            var flight = new Flight(origin, destination, departure, arrival, status);

            // Act
            Action action = () => flight.OnTime(delayDeparture: TimeSpan.FromHours(2));

            // Assert
            action.Should().Throw<DomainInvalidStateException>()
                .WithMessage(new DomainInvalidStateException(nameof(Flight), ValidationMessages.DepartureGreaterOrEqualsThanArrival).Message);
        }

        [Test]
        public void OffsetDepartureAndArrival_ShouldNotThrowException_WhenDepartureLessThanArrival()
        {
            // Arrange
            var origin = "Origin";
            var destination = "Destination";
            var departure = DateTimeOffset.UtcNow.AddHours(1);
            var arrival = DateTimeOffset.UtcNow.AddHours(2);
            var status = FlightStatus.OnTime;

            var flight = new Flight(origin, destination, departure, arrival, status);

            // Act
            Action action = () => flight.OnTime();

            // Assert
            action.Should().NotThrow();
        }

        [Test]
        public void Constructor_ShouldThrowException_WhenOriginEqualsDestination()
        {
            // Arrange
            var origin = "Origin";
            var destination = "Origin";
            var departure = DateTimeOffset.UtcNow.AddHours(1);
            var arrival = DateTimeOffset.UtcNow.AddHours(2);
            var status = FlightStatus.OnTime;

            // Act
            Action action = () => new Flight(origin, destination, departure, arrival, status);

            // Assert
            action.Should().Throw<DomainInvalidStateException>()
                .WithMessage(new DomainInvalidStateException(nameof(Flight), ValidationMessages.OriginEqualsDestination).Message);
        }
    }
}

