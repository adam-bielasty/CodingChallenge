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

        public CountryService(
            IHttpClientFactory httpClientFactory, 
            IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
            _httpClient = httpClientFactory.CreateClient(Consts.HttpClientNames.RestCountry);
        }

        public async Task<IEnumerable<Country>> GetAll()
        {
            return await _memoryCache.GetOrCreateAsync(Consts.CacheKeys.RestCountryAll, async entry =>
            {
                // It's read from Constants but this is simplification for that test.
                // It can be changed and read from AppSettings if necessary
                entry.SlidingExpiration = TimeSpan.FromMinutes(Consts.DefaultCacheTimespanInSeconds);

                var response = await _httpClient.GetAsync("/rest/v2/all?fields=name;flag;population;timezones;languages;currencies;capital;borders;alpha3Code");
                return await HandleResponse<IEnumerable<Country>>(response);
            });
        }

        public async Task<PagedListResponse<Country>> GetPaged(int page = Consts.DefaultPageNumber, int pageSize = Consts.PageSize)
        {
            var countries = await GetAll();
            var pagedResults = await countries.ToPagedListAsync(page, pageSize);
            
            return new PagedListResponse<Country>(pagedResults);
        }

        public async Task<Country> GetByCode(string code)
        {
            return (await GetAll()).FirstOrDefault(x => x.Alpha3Code.Equals(code, StringComparison.InvariantCultureIgnoreCase));
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
