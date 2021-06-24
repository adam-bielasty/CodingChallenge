// ReSharper disable ClassNeverInstantiated.Global
namespace Paymentsense.Coding.Challenge.Api
{
    public class Consts
    {
        public class HttpClientNames
        {
            public const string RestCountry = "RestCountry";
        }

        public class CacheKeys
        {
            public const string RestCountryAll = "RestCountryAll";
        }

        public const int PageSize = 10;
        public const int DefaultPageNumber = 1;
        public const int DefaultCacheTimespanInSeconds = 60 * 60 * 24;
    }
}