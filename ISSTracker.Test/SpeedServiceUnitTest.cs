using ISSTracker.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using FluentAssertions;

namespace ISSTracker.Test
{
    public class SpeedServiceUnitTest
    {
        [Theory]
        [MemberData(nameof(GetInvalidData))]
        public void TestInvalidNegativeOrZeroValues(double distance, long timeDiff)
        {
            //Arrange
            SpeedService speedService = new SpeedService();

            //Act
            Action action = () => speedService.CalculateSpeed(distance, timeDiff);

            //Assert
            action.Should().Throw<ArgumentException>().WithMessage($"Invalid value of*for parameter*");
        }

        [Theory]
        [MemberData(nameof(GetValidData))]
        public void TestCalculateSpeed(double distance, long timeDiff, double expectedSpeed)
        {
            //Arrange
            SpeedService speedService = new SpeedService();

            //Act
            double speed = speedService.CalculateSpeed(distance, timeDiff);

            //Assert
            speed.Should().Be(expectedSpeed);
        }

        public static IEnumerable<object[]> GetInvalidData()
        {
            var allData = new List<object[]>
            {
                new object[] { 0, 0 },
                new object[] { 0, 5 },
                new object[] { -1, 5 },
                new object[] { 1, 0 },
                new object[] { 1, -5 },
                new object[] { -1, -5 },
            };

            return allData;
        }

        public static IEnumerable<object[]> GetValidData()
        {
            var allData = new List<object[]>
            {
                new object[] { 10, 1, 36000 }                
            };

            return allData;
        }
    }
}
