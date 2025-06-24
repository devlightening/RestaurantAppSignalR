using AutoMapper;
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
        private readonly IMapper _mapper;

        public MoneyCasesController(IMoneyCaseService moneyCaseService, IMapper mapper)
        {
            _moneyCaseService = moneyCaseService;
            _mapper = mapper;
        }

        [HttpGet("TotalMoneyCaseAmount")]
        public IActionResult TotalMoneyCaseAmount()
        {
           var count= _moneyCaseService.TTotalMoneyCaseAmount();
            return Ok(count);
        }


       
    }
}
