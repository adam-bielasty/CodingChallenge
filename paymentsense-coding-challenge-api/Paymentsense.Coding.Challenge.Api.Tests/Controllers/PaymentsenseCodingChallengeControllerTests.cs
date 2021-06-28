using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Paymentsense.Coding.Challenge.Api.Controllers;
using Paymentsense.Coding.Challenge.Api.Models;
using Paymentsense.Coding.Challenge.Api.Services;
using Xunit;

namespace Paymentsense.Coding.Challenge.Api.Tests.Controllers
{
    public class PaymentsenseCodingChallengeControllerTests
    {
        private PaymentsenseCodingChallengeController _controller;
        private Mock<ICountryService> _countryServiceMock;
        
        public PaymentsenseCodingChallengeControllerTests()
        {
            _countryServiceMock = new Mock<ICountryService>();
            _controller = new PaymentsenseCodingChallengeController(_countryServiceMock.Object);
        }
        
        [Fact]
        public async Task Get_OnInvoke_ReturnsExpectedMessage()
        {
            // Act
            var result = await _controller.Get() as OkObjectResult;

            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().Be("Paymentsense Coding Challenge!");
        }
        
        [Fact]
        public async Task GetCountries_OnInvoke_ReturnsCountries()
        {
            // Arrange
            var page = 1;
            var searchText = "some search text";
                
            // Act
            var result = await _controller.GetCountries(page, searchText) as OkObjectResult;
        
            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            _countryServiceMock.Verify(x => x.GetPaged(page, Consts.PageSize, searchText), Times.Once);
        }
        
        [Fact]
        public async Task AddCountry_OnInvoke_ReturnsCountry()
        {
            // Arrange
            var country = new Country
            {
                Name = "Some Country Name"
            };
                
            // Act
            var result = await _controller.AddCountry(country) as OkResult;
        
            // Assert
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            _countryServiceMock.Verify(x => x.Add(country), Times.Once);
        }
    }
}
