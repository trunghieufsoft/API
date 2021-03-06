﻿using Microsoft.AspNetCore.Mvc;
using Common.DTOs.Common;
using Common.Core.Enumerations;
using Service.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;

namespace WebApi.Controllers
{
    [Authorize]
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