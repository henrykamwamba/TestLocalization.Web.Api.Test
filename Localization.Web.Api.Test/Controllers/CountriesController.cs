using TestLocalization.BLL.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using TestLocalization.Web.Api;

namespace TestLocalization.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CountriesController : ControllerBase
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        public CountriesController(IStringLocalizer<SharedResource> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet("GetCountries")]
        public async Task<IActionResult> GetCountries([FromServices] IRequestClient<IGetCountriesRequest> client)
        {
            var response = await client.GetResponse<IOutputResult>(new { });
            response.Message.OutputResult.Message = _localizer[response.Message.OutputResult.Message];
            return Ok(response.Message.OutputResult);
        }
    }
}
