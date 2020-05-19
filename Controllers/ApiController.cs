using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace CashflowApi.Controllers
{
    [ApiController]
    [Route("")]
    public class ApiController : ControllerBase
    {
        private readonly ILogger<ApiController> _logger;

        public ApiController(ILogger<ApiController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public string Get()
        {
            return "Cashflow Api";
        }
    }
}
