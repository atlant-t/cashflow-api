using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cashflow.Services.Account.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;

        public AccountController(ILogger<AccountController> logger)
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
