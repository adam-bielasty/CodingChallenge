using Newtonsoft.Json;
using Paymentsense.Coding.Challenge.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json.Serialization;
using X.PagedList;

namespace Paymentsense.Coding.Challenge.Api.Services
{
    public class CountryService : ICountryService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _memoryCache;
        // Related to Task 5), new countries can be added manually and stored in memory
        private readonly IList<Country> _additionalCountries = new List<Country>();


        public CountryService(
            IHttpClientFactory httpClientFactory, 
            IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _httpClient = httpClientFactory.CreateClient(Consts.HttpClientNames.RestCountry);
        }

        public async Task<IEnumerable<Country>> GetAll(string searchText = "")
        {
            var countries = await _memoryCache.GetOrCreateAsync(Consts.CacheKeys.RestCountryAll, async entry =>
            {
                // It's read from Constants but this is simplification for that interview test.
                // It can be changed and read from AppSettings if necessary
                entry.SlidingExpiration = TimeSpan.FromSeconds(Consts.DefaultCacheTimespanInSeconds);
                
                return await GetCountries();
            });
            
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                return countries.Where(c => c.Name.Contains(searchText, StringComparison.InvariantCultureIgnoreCase));
            }

            return countries;
        }

        private async Task<List<Country>> GetCountries()
        {
            var response = await _httpClient.GetAsync(
                "/rest/v2/all?fields=name;flag;population;timezones;languages;currencies;capital;borders;alpha3Code");
            var restCountries = (await HandleResponse<IEnumerable<Country>>(response)).ToList();
            
            // Map border names
            restCountries.ForEach(c => c.CountryBorders = c.Borders.Select(
                b => restCountries.First(rc => rc.Alpha3Code.Equals(b, StringComparison.InvariantCultureIgnoreCase)).Name));
            
            AppendAdditionalCountries(restCountries);
            
            return restCountries;
        }

        private void AppendAdditionalCountries(IList<Country> countries)
        {
            // Rest Countries take precedence
            _additionalCountries.ToList().ForEach(ac => 
            {
                if (!countries.Select(x => x.Name).Contains(ac.Name))
                {
                    countries.Add(ac);
                }
            });
        }
        
        public async Task<PagedListResponse<Country>> GetPaged(int page = Consts.DefaultPageNumber, int pageSize = Consts.PageSize, string searchText = "")
        {
            var countries = await GetAll(searchText);
            var pagedResults = await countries.ToPagedListAsync(page, pageSize);
            
            return new PagedListResponse<Country>(pagedResults);
        }

        public async Task Add(Country country)
        {
            _additionalCountries.Add(country);
            _memoryCache.Set(Consts.CacheKeys.RestCountryAll, await GetCountries());
        }

        private async Task<T> HandleResponse<T>(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            }

            response.EnsureSuccessStatusCode();

            throw new Exception("Cannot parse the response");
        }
    }
}
