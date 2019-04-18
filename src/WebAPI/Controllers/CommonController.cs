using WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Service.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Entities.Enumerations;

namespace WebAPI.Controllers
{
    [Authorize]
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
        [AllowAnonymous]
        [Route("GetAllGroup")]
        public IActionResult GetAllGroup()
            => Json(data: _commonService.GetAllGroup());

        [HttpGet]
        [AllowAnonymous]
        [Route("GetAllCountry")]
        public IActionResult GetAllCountry()
            => Json(data: _commonService.GetAllCountry());
        
        [HttpGet]
        [Route("GetUsersAssign")]
        public IActionResult GetUsersAssign(string username)
            => Json(data: _commonService.GetUsersAssign(username));
        
        [HttpGet]
        [Route("GetUsersAllTypeAssignByCountry")]
        public IActionResult GetUsersAllTypeAssignByCountry(UserTypeEnum userType, string country = null)
            => Json(data: _commonService.GetUsersAllTypeAssignByCountry(userType, country));
    }
}