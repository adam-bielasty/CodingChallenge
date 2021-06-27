using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Paymentsense.Coding.Challenge.Api.Models;
using Paymentsense.Coding.Challenge.Api.Services;

namespace Paymentsense.Coding.Challenge.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsenseCodingChallengeController : ControllerBase
    {
        private readonly ICountryService _countryService;

        public PaymentsenseCodingChallengeController(ICountryService countryService)
        {
            _countryService = countryService;
        }
        
        [HttpGet("countries")]
        public async Task<IActionResult> GetAllCountries()
        {
            // If there is an exception, I would catch it in Global Exception filter, log the error (e.g. AppInsights)
            // and return some error correlation id to the user.
            return Ok(await _countryService.GetAll());
        }

        [HttpGet("countries/{page:int}")]
        public async Task<IActionResult> GetCountries(int page = Consts.DefaultPageNumber, string searchText = "")
        {   
            return Ok(await _countryService.GetPaged(page, Consts.PageSize, searchText));
        }
        
        [HttpPost("countries")]
        public async Task<IActionResult> AddCountry(Country country)
        {
            await _countryService.Add(country);
            return Ok();
        }
        
        [HttpGet]
        public ActionResult<string> Get()
        {
            return Ok("Paymentsense Coding Challenge!");
        }
    }
}
