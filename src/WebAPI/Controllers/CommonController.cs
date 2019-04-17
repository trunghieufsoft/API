using WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Abstractions;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommonController : BaseController
    {
        private readonly ICommonService _commonService;

        public CommonController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        [HttpGet]
        [Route("GetAllGroup")]
        public IActionResult GetAllGroup()
            => Json(data: _commonService.GetAllGroup());

        [HttpGet]
        [Route("GetAllCountry")]
        public IActionResult GetAllCountry()
            => Json(data: _commonService.GetAllCountry());
    }
}