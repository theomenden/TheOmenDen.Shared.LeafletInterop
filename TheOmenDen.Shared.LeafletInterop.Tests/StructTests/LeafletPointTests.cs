using Bogus;
using FluentAssertions;
using TheOmenDen.Shared.LeafletInterop.Components;

namespace TheOmenDen.Shared.LeafletInterop.Tests.StructTests;

public class LeafletPointTests
{
    private readonly Faker _faker = new();

    [Fact]
    public void Should_Create_LeafletPoint()
    {
        //Arrange
        var latitude = _faker.Address.Latitude();
        var longitude = _faker.Address.Longitude();

        //Act
        var point = LeafletPoint.Create(latitude, longitude);

        //Assert
        point.Latitude.Should().Be(latitude);
        point.Longitude.Should().Be(longitude);
    }

    [Fact]
    public void Should_Create_Default_LeafletPoint()
    {
        //Arrange
        var defaultPoint = LeafletPoint.Default;

        //Act
        var point = LeafletPoint.Create(51.505, -0.09);

        //Assert
        point.Should().Be(defaultPoint);
    }

    [Fact]
    public void Points_Should_Be_Equal()
    {
        //Arrange
        var latitude = _faker.Address.Latitude();
        var longitude = _faker.Address.Longitude();

        //Act
        var point1 = LeafletPoint.Create(latitude, longitude);
        var point2 = LeafletPoint.Create(latitude, longitude);

        //Assert
        point1.Should().Be(point2);
    }

    [Fact]
    public void Should_Add_Points()
    {
        //Arrange
        var point1 = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var point2 = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());

        //Act
        var result = point1 + point2;

        //Assert
        result.Latitude.Should().Be(point1.Latitude + point2.Latitude);
        result.Longitude.Should().Be(point1.Longitude + point2.Longitude);
    }

    [Fact]
    public void Should_Subtract_Points()
    {
        //Arrange
        var point1 = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var point2 = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());

        //Act
        var result = point1 - point2;

        //Assert
        result.Latitude.Should().Be(point1.Latitude - point2.Latitude);
        result.Longitude.Should().Be(point1.Longitude - point2.Longitude);
    }

    [Fact]
    public void Should_Scale_Point()
    {
        //Arrange
        var point = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var factor = _faker.Random.Double(0.1, 10.0);

        //Act
        var result = point.Scale(factor);

        //Assert
        result.Latitude.Should().Be(point.Latitude * factor);
        result.Longitude.Should().Be(point.Longitude * factor);
    }

    [Fact]
    public void Should_Calculate_Distance_Between_Points()
    {
        //Arrange
        var point1 = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var point2 = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());

        //Act
        var result = point1.DistanceTo(point2);

        //Assert
        result.Should().Be(
            Math.Sqrt(Math.Pow(point1.Latitude - point2.Latitude, 2)
                      + Math.Pow(point1.Longitude - point2.Longitude, 2)));
    }
}