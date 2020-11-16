using GMap.NET;
using ISSTracker.Infrastructure.Models;
using ISSTracker.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;

namespace ISSTracker.Test
{
    public class APIMapperServiceUnitTest
    {
        [Fact]
        public void TestNullIssApiResponse()
        {
            //Arrange
            IssApiResponse apiResult = null;
            APIMapperService apiMapperservice = new APIMapperService();

            //Act
            Func<PointLatLng> action = () => apiMapperservice.Map(apiResult);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage("*cannot be null");
        }

        [Theory]
        [InlineData(true, null, null)]
        [InlineData(false, null, null)]
        [InlineData(false, null, "0.0")]
        [InlineData(false, "", "0.0")]
        [InlineData(false, "0.0", null)]
        [InlineData(false, "0.0", "")]
        public void TestNotWellFormedIssApiResponse(bool nullIssPosition, string latitude, string longitude)
        {
            //Arrange
            IssApiResponse apiResult = new IssApiResponse();
            if(!nullIssPosition)
            {
                apiResult.iss_position = new IssPosition() { latitude = latitude, longitude = longitude };
            }
            
            APIMapperService apiMapperservice = new APIMapperService();

            //Act
            Func<PointLatLng> action1 = () => apiMapperservice.Map(apiResult);            

            //Assert
            action1.Should().Throw<ArgumentException>().WithMessage("*is not well formed");            
        }

        [Theory]
        [InlineData("0.0", "XYZ")]
        [InlineData("XYZ", "0.0")]
        public void TestCoordinatesNotConvertible(string latitude, string longitude)
        {
            //Arrange            
            IssApiResponse apiResult = new IssApiResponse() { iss_position = new IssPosition() { latitude = latitude, longitude = longitude } };            
            APIMapperService apiMapperservice = new APIMapperService();

            //Act           
            Func<PointLatLng> action1 = () => apiMapperservice.Map(apiResult);

            //Assert
            action1.Should().Throw<FormatException>();                 
        }

        [Theory]
        [InlineData("100.0", "0.0")]
        [InlineData("-100.0", "0.0")]
        [InlineData("0.0", "200.0")]
        [InlineData("0.0", "-200.0")]
        [InlineData("0.0", "-180.00000001")]
        public void TestOutOfBoundsCoordinates(string latitude, string longitude)
        {
            //Arrange            
            IssApiResponse apiResult = new IssApiResponse() { iss_position = new IssPosition() { latitude = latitude, longitude = longitude } };            
            APIMapperService apiMapperservice = new APIMapperService();

            //Act           
            Func<PointLatLng> action1 = () => apiMapperservice.Map(apiResult);

            //Assert
            action1.Should().Throw<ArgumentException>().WithMessage("Coordinates*are out of bounds");            
        }

        [Theory]
        [InlineData("0.0", "0.0", 0, 0)]
        [InlineData("-90.0", "0.0", -90, 0)]
        [InlineData("90.0", "0.0", 90, 0)]
        [InlineData("0.0", "-180.0", 0, -180)]
        [InlineData("0.0", "180.0", 0, 180)]
        [InlineData("35.4685", "-97.3454", 35.4685, -97.3454)]
        public void TestValidCoordinates(string latitude, string longitude, double expectedLatitude, double expectedLongitude)
        {
            //Arrange            
            IssApiResponse apiResult = new IssApiResponse() { iss_position = new IssPosition() { latitude = latitude, longitude = longitude } };
            APIMapperService apiMapperservice = new APIMapperService();

            //Act           
            PointLatLng result = apiMapperservice.Map(apiResult);

            //Assert
            result.Lat.Should().Be(expectedLatitude);
            result.Lng.Should().Be(expectedLongitude);
        }
    }
}
