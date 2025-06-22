using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoneyCasesController : ControllerBase
    {
        private readonly IMoneyCaseService _moneyCaseService;

        public MoneyCasesController(IMoneyCaseService moneyCaseService)
        {
            _moneyCaseService = moneyCaseService;
        }

        [HttpGet("TotalMoneyCaseAmount")]
        public IActionResult TotalMoneyCaseAmount()
        {
           var count= _moneyCaseService.TTotalMoneyCaseAmount();
            return Ok(count);
        }


       
    }
}
