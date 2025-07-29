using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SignalR.BusinessLayer.Abstracts;

namespace SignalRApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoneyCasesController : ControllerBase
    {
        private readonly IMoneyCaseService _moneyCaseService;
        private readonly IMapper _mapper;

        public MoneyCasesController(IMoneyCaseService moneyCaseService, IMapper mapper)
        {
            _moneyCaseService = moneyCaseService;
            _mapper = mapper;
        }

        [HttpGet("TotalMoneyCaseAmount")]
        public async Task<IActionResult> TotalMoneyCaseAmount()
        {
            var count = await _moneyCaseService.TTotalMoneyCaseAmountAsync();
            return Ok(count);
        }
    }
}
