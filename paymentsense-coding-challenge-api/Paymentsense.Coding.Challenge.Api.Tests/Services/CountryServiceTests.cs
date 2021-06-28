using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Moq.Language.Flow;
using Moq.Protected;
using Newtonsoft.Json;
using Paymentsense.Coding.Challenge.Api.Models;
using Paymentsense.Coding.Challenge.Api.Services;
using Xunit;

namespace Paymentsense.Coding.Challenge.Api.Tests.Services
{
    public class CountryServiceTests
    {
        private ICountryService _countryService;
        private readonly Mock<IHttpClientFactory> _httpClientFactoryMock;
        
        private readonly Mock<HttpMessageHandler> _httpMessageHandlerMock;
        
        private readonly IMemoryCache _memoryCache = new MemoryCache(new MemoryCacheOptions());

        public CountryServiceTests()
        {
            _httpClientFactoryMock = new Mock<IHttpClientFactory>();
            _httpMessageHandlerMock = new Mock<HttpMessageHandler>();
            _httpClientFactoryMock.Setup(x => x.CreateClient(Consts.HttpClientNames.RestCountry))
                .Returns(new HttpClient());
            
            _httpClientFactoryMock.Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(new HttpClient(_httpMessageHandlerMock.Object)
                {
                    BaseAddress = new Uri("https://restcountries.eu")
                });

            SetupDefaultMocks();
            
            _countryService = new CountryService(_httpClientFactoryMock.Object, _memoryCache);
        }

        [Fact]
        public async Task GetAll_SearchTextIsEmpty_ReturnsAllCountries()
        {
            // Arrange
            var searchText = "";
            
            // Act
            var results = await _countryService.GetAll(searchText);

            // Assert
            Assert.Equal(8, results.Count());
        }
        
        [Theory]
        [InlineData("AL")]
        [InlineData("al")]
        public async Task GetAll_SearchTextApplied_ReturnsFilteredCountries(string searchText)
        {
            // Arrange
            // Act
            var results = await _countryService.GetAll(searchText);

            // Assert
            Assert.Equal(2, results.Count());
        }
        
        [Fact]
        public async Task GetAll_SearchTextApplied_ReturnsProperFilteredSingleCountryWithDetails()
        {
            // Arrange
            // Act
            var results = (await _countryService.GetAll("Åland Islands")).ToList();

            // Assert
            Assert.Single(results);
            results.Single().Name.Should().Be("Åland Islands");
            results.Single().Capital.Should().Be("Mariehamn");
            results.Single().Flag.Should().Be("https://restcountries.eu/data/ala.svg");
            results.Single().Population.Should().Be(28875);
            results.Single().Alpha3Code.Should().Be("ALA");
            results.Single().TimeZones.Should().HaveCount(1);
            results.Single().Languages.Should().ContainSingle(x => x.Name.Equals("Swedish"));
            results.Single().Currencies.Should().ContainSingle(x => x.Name.Equals("Euro"));
            // Json was modified in SetupDefaultMocks method to have that border info
            results.Single().Borders.Should().Contain("AFG");
            results.Single().CountryBorders.Should().Contain("Afghanistan");
        }
        
        [Fact]
        public async Task GetAll_SearchTextIsEmpty_AppendAdditionalCountries_ReturnsAllCountriesCombined()
        {
            // Arrange
            var searchText = "";
            var additionalCountry = new Country
            {
                Name = "Additional Country Name 1"
            };
            
            // Act
            await _countryService.Add(additionalCountry);
            var results = (await _countryService.GetAll(searchText)).ToList();

            // Assert
            Assert.Equal(9, results.Count);
            Assert.Contains(results, c => c.Name == additionalCountry.Name);
        }
        
        [Fact]
        public async Task GetAll_SearchTextIsEmpty_AppendAdditionalCountries_DoNotOverrideCountriesByName_ReturnsAllCountriesCombined()
        {
            // Arrange
            var searchText = "";
            var additionalCountry = new Country
            {
                // Already existing country in RestClient response
                Name = "Albania"
            };
            
            // Act
            await _countryService.Add(additionalCountry);
            var results = (await _countryService.GetAll(searchText)).ToList();

            // Assert
            Assert.Equal(8, results.Count);
        }
        
        [Fact]
        public async Task GetPaged_NoFiltering_ReturnsPagedCountries()
        {
            // Arrange
            var page = 1;
            var pageSize = 3;
            
            // Act
            var pagedResults = await _countryService.GetPaged(page, pageSize);

            // Assert
            Assert.NotNull(pagedResults);
            Assert.NotNull(pagedResults.Items);
            Assert.Equal(pageSize, pagedResults.Items.Count());
            Assert.NotNull(pagedResults.MetaData);
            Assert.Equal(8, pagedResults.MetaData.TotalItemCount);
            Assert.Equal(3, pagedResults.MetaData.PageCount);
        }
        
        [Fact]
        public async Task GetPaged_Filtering_ReturnsPagedCountries()
        {
            // Arrange
            var page = 1;
            var pageSize = 1;
            var searchText = "al";
            
            // Act
            var pagedResults = await _countryService.GetPaged(page, pageSize, searchText);

            // Assert
            Assert.NotNull(pagedResults);
            Assert.NotNull(pagedResults.Items);
            Assert.Equal(pageSize, pagedResults.Items.Count());
            Assert.NotNull(pagedResults.MetaData);
            Assert.Equal(2, pagedResults.MetaData.TotalItemCount);
            Assert.Equal(2, pagedResults.MetaData.PageCount);
        }
        
        private void SetupDefaultMocks()
        {
            const string restClientResponse = "[" +
                "{\"currencies\":[{\"code\":\"AFN\",\"name\":\"Afghan afghani\",\"symbol\":\"؋\"}],\"languages\":[{\"iso639_1\":\"ps\",\"iso639_2\":\"pus\",\"name\":\"Pashto\",\"nativeName\":\"پښتو\"},{\"iso639_1\":\"uz\",\"iso639_2\":\"uzb\",\"name\":\"Uzbek\",\"nativeName\":\"Oʻzbek\"},{\"iso639_1\":\"tk\",\"iso639_2\":\"tuk\",\"name\":\"Turkmen\",\"nativeName\":\"Türkmen\"}],\"flag\":\"https://restcountries.eu/data/afg.svg\",\"name\":\"Afghanistan\",\"alpha3Code\":\"AFG\",\"capital\":\"Kabul\",\"population\":27657145,\"timezones\":[\"UTC+04:30\"],\"borders\":[\"IRN\",\"PAK\",\"TKM\",\"UZB\",\"TJK\",\"CHN\"]}," +
                "{\"currencies\":[{\"code\":\"EUR\",\"name\":\"Euro\",\"symbol\":\"€\"}],\"languages\":[{\"iso639_1\":\"sv\",\"iso639_2\":\"swe\",\"name\":\"Swedish\",\"nativeName\":\"svenska\"}],\"flag\":\"https://restcountries.eu/data/ala.svg\",\"name\":\"Åland Islands\",\"alpha3Code\":\"ALA\",\"capital\":\"Mariehamn\",\"population\":28875,\"timezones\":[\"UTC+02:00\"],\"borders\":[\"AFG\"]}," +
                "{\"currencies\":[{\"code\":\"ALL\",\"name\":\"Albanian lek\",\"symbol\":\"L\"}],\"languages\":[{\"iso639_1\":\"sq\",\"iso639_2\":\"sqi\",\"name\":\"Albanian\",\"nativeName\":\"Shqip\"}],\"flag\":\"https://restcountries.eu/data/alb.svg\",\"name\":\"Albania\",\"alpha3Code\":\"ALB\",\"capital\":\"Tirana\",\"population\":2886026,\"timezones\":[\"UTC+01:00\"],\"borders\":[\"MNE\",\"GRC\",\"MKD\",\"KOS\"]}," +
                "{\"currencies\":[{\"code\":\"DZD\",\"name\":\"Algerian dinar\",\"symbol\":\"د.ج\"}],\"languages\":[{\"iso639_1\":\"ar\",\"iso639_2\":\"ara\",\"name\":\"Arabic\",\"nativeName\":\"العربية\"}],\"flag\":\"https://restcountries.eu/data/dza.svg\",\"name\":\"Algeria\",\"alpha3Code\":\"DZA\",\"capital\":\"Algiers\",\"population\":40400000,\"timezones\":[\"UTC+01:00\"],\"borders\":[\"TUN\",\"LBY\",\"NER\",\"ESH\",\"MRT\",\"MLI\",\"MAR\"]}," +
                "{\"currencies\":[{\"code\":\"USD\",\"name\":\"United State Dollar\",\"symbol\":\"$\"}],\"languages\":[{\"iso639_1\":\"en\",\"iso639_2\":\"eng\",\"name\":\"English\",\"nativeName\":\"English\"},{\"iso639_1\":\"sm\",\"iso639_2\":\"smo\",\"name\":\"Samoan\",\"nativeName\":\"gagana fa'a Samoa\"}],\"flag\":\"https://restcountries.eu/data/asm.svg\",\"name\":\"American Samoa\",\"alpha3Code\":\"ASM\",\"capital\":\"Pago Pago\",\"population\":57100,\"timezones\":[\"UTC-11:00\"],\"borders\":[]}," +
                "{\"currencies\":[{\"code\":\"EUR\",\"name\":\"Euro\",\"symbol\":\"€\"}],\"languages\":[{\"iso639_1\":\"ca\",\"iso639_2\":\"cat\",\"name\":\"Catalan\",\"nativeName\":\"català\"}],\"flag\":\"https://restcountries.eu/data/and.svg\",\"name\":\"Andorra\",\"alpha3Code\":\"AND\",\"capital\":\"Andorra la Vella\",\"population\":78014,\"timezones\":[\"UTC+01:00\"],\"borders\":[\"FRA\",\"ESP\"]}," +
                "{\"currencies\":[{\"code\":\"AOA\",\"name\":\"Angolan kwanza\",\"symbol\":\"Kz\"}],\"languages\":[{\"iso639_1\":\"pt\",\"iso639_2\":\"por\",\"name\":\"Portuguese\",\"nativeName\":\"Português\"}],\"flag\":\"https://restcountries.eu/data/ago.svg\",\"name\":\"Angola\",\"alpha3Code\":\"AGO\",\"capital\":\"Luanda\",\"population\":25868000,\"timezones\":[\"UTC+01:00\"],\"borders\":[\"COG\",\"COD\",\"ZMB\",\"NAM\"]}," +
                "{\"currencies\":[{\"code\":\"XCD\",\"name\":\"East Caribbean dollar\",\"symbol\":\"$\"}],\"languages\":[{\"iso639_1\":\"en\",\"iso639_2\":\"eng\",\"name\":\"English\",\"nativeName\":\"English\"}],\"flag\":\"https://restcountries.eu/data/aia.svg\",\"name\":\"Anguilla\",\"alpha3Code\":\"AIA\",\"capital\":\"The Valley\",\"population\":13452,\"timezones\":[\"UTC-04:00\"],\"borders\":[]}]";
            SetupHttpHandler("/rest/v2/all?fields=name;flag;population;timezones;languages;currencies;capital;borders;alpha3Code", HttpStatusCode.OK, JsonConvert.DeserializeObject<dynamic>(restClientResponse));
        }
        
        private IReturnsResult<HttpMessageHandler> SetupHttpHandler(string url, HttpStatusCode statusCode, object data)
        {
            return _httpMessageHandlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.Is<HttpRequestMessage>(m => m.RequestUri.ToString().Contains(url)), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = statusCode,
                    Content = new StringContent(JsonConvert.SerializeObject(data))
                });
        }
    }
}