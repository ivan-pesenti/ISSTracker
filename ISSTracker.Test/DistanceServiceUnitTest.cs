using GMap.NET;
using ISSTracker.Infrastructure.Services;
using System;
using Xunit;
using FluentAssertions.Numeric;
using FluentAssertions;
namespace ISSTracker.Test
{
    public class DistanceServiceUnitTest
    {
        [Fact]
        public void TestCalculateDistance()
        {
            //Arrange
            DistanceService distanceService = new DistanceService();
            PointLatLng nortPole = new PointLatLng(90, 0);
            PointLatLng greenwichOnEquator = new PointLatLng(0, 0);

            //Act            
            double distance = distanceService.CalculateDistanceBetweenPoints(nortPole, greenwichOnEquator, currentEarthRadiusInMeters: 6371000, currentIssOrbitalHeight: 0.0) / 1000;

            //Assert
            //10002Km from Wikipedia. Admit a tolerance ok 10Km.
            distance.Should().BeInRange(9992, 10012);          
        }
    }
}
