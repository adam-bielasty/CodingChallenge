using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Paymentsense.Coding.Challenge.Api.Models
{
    public class Country
    {
        public string Name { get; set; }
        public string Flag { get; set; }
        public string Alpha3Code { get; set; }
        public double Population { get; set; }
        public IEnumerable<string> TimeZones { get; set; }
        public IEnumerable<Currency> Currencies { get; set; }
        public IEnumerable<Language> Languages { get; set; }
        public string Capital { get; set; }
        public IEnumerable<string> Borders { get; set; }
        public IEnumerable<string> CountryBorders { get; set; }
    }

}
