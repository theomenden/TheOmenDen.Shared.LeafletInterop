using System.Runtime.CompilerServices;
using Bogus;
using FluentAssertions;
using TheOmenDen.Shared.LeafletInterop.Components;

namespace TheOmenDen.Shared.LeafletInterop.Tests.StructTests;

public class LeafletBoundsTests
{
    private readonly Faker _faker = new();

    [Fact]
    public void Should_Create_LeafletBounds_From_Points()
    {
        //Arrange
        var southwest = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var northeast = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());

        //Act
        var bounds = LeafletBounds.Create(southwest, northeast);

        //Assert
        bounds.Southwest.Should().Be(southwest);
        bounds.Northeast.Should().Be(northeast);
    }

    [Fact]
    public void Should_Create_LeafletBounds_From_Array_Of_Points()
    {
        //Arrange
        var points = new[]
        {
            LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude()),
            LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude()),
            LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude())
        };

        //Act
        var bounds = LeafletBounds.Create(points);

        //Assert
        bounds.Southwest.Latitude.Should().Be(points.Min(p => p.Latitude));
        bounds.Southwest.Longitude.Should().Be(points.Min(p => p.Longitude));
        bounds.Northeast.Latitude.Should().Be(points.Max(p => p.Latitude));
        bounds.Northeast.Longitude.Should().Be(points.Max(p => p.Longitude));
    }

    [Fact]
    public void Should_Throw_ArgumentException_When_Creating_Bounds_From_Empty_Array()
    {
        //Arrange
        var points = Array.Empty<LeafletPoint>();

        //Act
        Action act = () => LeafletBounds.Create(points);

        //Assert
        act.Should().Throw<ArgumentException>().WithMessage("At least one point is required to create a bounds. (Parameter 'points')");
    }

    [Fact]
    public void Should_Calculate_Quadrilateral_Dimensions()
    {
        //Arrange
        var southwest = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var northeast = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var bounds = LeafletBounds.Create(southwest, northeast);

        //Act
        var width = bounds.Width;
        var height = bounds.Height;

        //Assert
        width.Should().Be(northeast.Longitude - southwest.Longitude);
        height.Should().Be(northeast.Latitude - southwest.Latitude);
    }

    [Fact]
    public void Should_Check_If_Point_Is_Contained_In_Bounds()
    {
        //Arrange
        var southwest = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var northeast = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var bounds = LeafletBounds.Create(southwest, northeast);
        var point = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());

        //Act
        var result = bounds.Contains(point);

        //Assert
        result.Should()
            .Be(point.Latitude >= southwest.Latitude
            && point.Latitude <= northeast.Latitude
            && point.Longitude >= southwest.Longitude
            && point.Longitude <= northeast.Longitude);
    }

    [Fact]
    public void Should_Calculate_Center()
    {
        //Arrange
        var southwest = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var northeast = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var bounds = LeafletBounds.Create(southwest, northeast);

        //Act
        var result = bounds.Center();

        //Assert
        result.Latitude.Should().Be((southwest.Latitude + northeast.Latitude) * 0.5d);
        result.Longitude.Should().Be((southwest.Longitude + northeast.Longitude) * 0.5d);
    }

    [Fact]
    public void Should_Calculate_Rounded_Center()
    {
        //Arrange
        var southwest = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var northeast = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var bounds = LeafletBounds.Create(southwest, northeast);

        //Act
        var result = bounds.Center(true);

        //Assert
        result.Latitude.Should().Be(Math.Round((southwest.Latitude + northeast.Latitude) * 0.5d));
        result.Longitude.Should().Be(Math.Round((southwest.Longitude + northeast.Longitude) * 0.5d));
    }

    [Fact]
    public void Should_Extend_Bounds()
    {
        //Arrange
        var southwest = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var northeast = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var bounds = LeafletBounds.Create(southwest, northeast);
        var point = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());

        //Act
        var extendedBounds = bounds.Extend(point);

        //Assert
        extendedBounds.Southwest.Latitude.Should().Be(Math.Min(bounds.Southwest.Latitude, point.Latitude));
        extendedBounds.Southwest.Longitude.Should().Be(Math.Min(bounds.Southwest.Longitude, point.Longitude));
        extendedBounds.Northeast.Latitude.Should().Be(Math.Max(bounds.Northeast.Latitude, point.Latitude));
        extendedBounds.Northeast.Longitude.Should().Be(Math.Max(bounds.Northeast.Longitude, point.Longitude));
    }
    [Fact]
    public void Should_Check_If_Bounds_Intersect()
    {
        //Arrange
        var southwest1 = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var northeast1 = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var bounds1 = LeafletBounds.Create(southwest1, northeast1);

        var southwest2 = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var northeast2 = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var bounds2 = LeafletBounds.Create(southwest2, northeast2);

        //Act
        var result = bounds1.Intersects(bounds2);

        //Assert
        result.Should().Be(bounds1.Southwest.Latitude <= bounds2.Northeast.Latitude
            && bounds1.Northeast.Latitude >= bounds2.Southwest.Latitude
            && bounds1.Southwest.Longitude <= bounds2.Northeast.Longitude
            && bounds1.Northeast.Longitude >= bounds2.Southwest.Longitude);
    }

    [Fact]
    public void Should_Check_If_Bounds_Are_Equal()
    {
        //Arrange
        var southwest = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var northeast = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var bounds1 = LeafletBounds.Create(southwest, northeast);
        var bounds2 = LeafletBounds.Create(southwest, northeast);

        //Act
        var result = bounds1 == bounds2;

        //Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void Should_Check_If_Bounds_Overlap()
    {
        //Arrange
        var southwest1 = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var northeast1 = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var bounds1 = LeafletBounds.Create(southwest1, northeast1);

        var southwest2 = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var northeast2 = LeafletPoint.Create(_faker.Address.Latitude(), _faker.Address.Longitude());
        var bounds2 = LeafletBounds.Create(southwest2, northeast2);

        //Act
        var result = bounds1.Overlaps(bounds2);

        //Assert
        result.Should().Be(bounds1.Southwest.Latitude < bounds2.Northeast.Latitude
            && bounds1.Northeast.Latitude > bounds2.Southwest.Latitude
            && bounds1.Southwest.Longitude < bounds2.Northeast.Longitude
            && bounds1.Northeast.Longitude > bounds2.Southwest.Longitude);
    }
}