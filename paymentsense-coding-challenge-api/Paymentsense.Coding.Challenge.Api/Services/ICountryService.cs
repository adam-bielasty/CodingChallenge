using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paymentsense.Coding.Challenge.Api.Models;
using X.PagedList;

namespace Paymentsense.Coding.Challenge.Api.Services
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetAll(string searchText = "");
        Task<PagedListResponse<Country>> GetPaged(int page = 1, int pageSize = 10, string searchText = "");
        Task Add(Country country);
    }
}
