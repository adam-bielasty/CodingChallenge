using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Paymentsense.Coding.Challenge.Api.Models;
using X.PagedList;

namespace Paymentsense.Coding.Challenge.Api.Services
{
    public interface ICountryService
    {
        Task<IEnumerable<Country>> GetAll();
        Task<PagedListResponse<Country>> GetPaged(int page = 1, int pageSize = 10);
        Task<Country> GetByCode(string code);
    }
}
