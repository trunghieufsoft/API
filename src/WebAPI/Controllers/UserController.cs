using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Common.Core.Timing;
using Common.DTOs.Common;
using Common.DTOs.UserModel;
using Services.Services.Abstractions;
using Entities.Enumerations;

namespace WebApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IConfiguration _config;
        private readonly ISessionService _sessionService;

        public UserController(IUserService userService, IConfiguration config, ISessionService sessionService)
        {
            _userService = userService;
            _config = config;
            _sessionService = sessionService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("WebLogin")]
        public IActionResult WebLogin([FromBody]LoginInput requestDto)
        {
            UserOutput result = _userService.WebLogin(requestDto);
            return Json(GenerateJSONWebToken(result));
        }

        [HttpPost]
        [Route("CreateManager")]
        public IActionResult CreateManager([FromBody] ManagerInput requestDto)
        {
            _sessionService.CheckSession(GetToken(), GetCurrentUser());
            var data = _userService.CreateManager(new DataInput<ManagerInput>(requestDto, GetCurrentUser()));
            return Json(data);
        }

        [HttpPost]
        [Route("CreateStaff")]
        public IActionResult CreateStaff([FromBody] StaffInput requestDto)
        {
            _sessionService.CheckSession(GetToken(), GetCurrentUser());
            var data = _userService.CreateStaff(new DataInput<StaffInput>(requestDto, GetCurrentUser()));
            return Json(data);
        }

        [HttpGet]
        [Route("View")]
        public IActionResult View(Guid id)
        {
            _sessionService.CheckSession(GetToken(), GetCurrentUser());
            UserOutput data = _userService.View(id);
            return Json(data);
        }

        [HttpPost]
        [Route("Search/ManagerAdmin")]
        public IActionResult SearchManager([FromBody] SearchInput searchInput)
        {
            _sessionService.CheckSession(GetToken(), GetCurrentUser());
            var requestDto = new DataInput<SearchInput>()
            {
                CurrentUser = GetCurrentUser(),
                Dto = searchInput
            };
            return Json(_userService.SearchManager(requestDto));
        }

        [HttpPost]
        [Route("Search/Staff")]
        public IActionResult SearchStaff([FromBody] SearchInput searchInput)
        {
            _sessionService.CheckSession(GetToken(), GetCurrentUser());
            var requestDto = new DataInput<SearchInput>()
            {
                CurrentUser = GetCurrentUser(),
                Dto = searchInput
            };
            return Json(_userService.SearchStaff(requestDto));
        }

        [HttpPost]
        [Route("Search/Employee")]
        public IActionResult SearchEmployee([FromBody] SearchInput searchInput)
        {
            _sessionService.CheckSession(GetToken(), GetCurrentUser());
            var requestDto = new DataInput<SearchInput>()
            {
                CurrentUser = GetCurrentUser(),
                Dto = searchInput
            };
            return Json(_userService.SearchEmployee(requestDto));
        }

        [HttpGet]
        [Route("TotalUsers")]
        public IActionResult TotalUsers()
        {
            CountTotalUsers result = _userService.CountTotalUsers(GetCurrentUser());

            return Json(result);
        }

        [HttpGet]
        [Route("GetProfile")]
        public IActionResult GetProfile()
        {
            _sessionService.CheckSession(GetToken(), GetCurrentUser());
            Guid? userId = GetUserIdFromHeader();
            UserOutput result = _userService.View(userId.Value);

            return Json(result);
        }

        [HttpGet]
        [Route("GetSubcriseToken")]
        public IActionResult GetSubcriseToken()
        {
            return Json(_userService.GetSubcriseToken(GetUserIdFromHeader().Value));
        }

        private string GenerateJSONWebToken(UserOutput userInfo)
        {
            if (userInfo == null)
            {
                return string.Empty;
            }
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            string fullName = string.IsNullOrEmpty(userInfo.FullName) ? userInfo.Username : userInfo.FullName;

            string tokenGuid = Guid.NewGuid().ToString();
            DateTime expried = Clock.Now.AddMinutes(Math.Max(Convert.ToDouble(_config["Config:TokenExpiryTimeInMinutes"]), 5));
            Claim[] claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub,  userInfo.Username),
                new Claim("Username", userInfo.Username),
                new Claim("Fullname", fullName),
                new Claim("UserType", userInfo.UserType),
                new Claim("ExpiredPassword", userInfo.ExpiredPassword),
                new Claim("UserId", userInfo.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, tokenGuid)
            };

            JwtSecurityToken token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Issuer"], claims, signingCredentials: credentials);
            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            _userService.UpdateToken(userInfo.Id, tokenGuid);

            return tokenString;
        }
    }
}