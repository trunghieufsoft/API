using Microsoft.AspNetCore.Mvc;
using Common.DTOs.Common;
using Common.Core.Enumerations;
using Service.Services.Abstractions;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuditLogController : BaseController
    {
        private readonly ILogService _logService;

        public AuditLogController(ILogService logService)
        {
            _logService = logService;
        }

        [HttpPost]
        [Route("Search")]
        public IActionResult Report([FromBody] SearchFromTo<TypeLogEnum> requestDto)
        {
            SearchOutput data = _logService.Search(requestDto, GetCurrentUser());

            return Json(data);
        }
    }
}