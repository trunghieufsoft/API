using System;
using Serilog;
using System.Linq;
using Entities.Entities;
using Common.Core.Timing;
using Common.DTOs.Common;
using Database.UnitOfWork;
using Common.Core.Services;
using Database.Repositories;
using Entities.Enumerations;
using Common.DTOs.UserModel;
using Common.Core.Extensions;
using Common.Core.Exceptions;
using Common.Core.Enumerations;
using Common.Core.Linq.Extensions;
using Services.Services.Abstractions;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Services.Services
{
    public class UserService : BaseService, IUserService
    {
        #region initial
        private readonly ILogService _logService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly ISystemConfigService _configService;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Group> _groupRepository;
        #endregion

        #region UserService
        public UserService(ILogService logService,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            IRepository<User> userRepository,
            IRepository<Group> groupRepository,
            ISystemConfigService configService)
        {
            _logService = logService;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _configService = configService;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
        }
        #endregion

        #region API
        public Guid CreateManager(DataInput<ManagerInput> requestDto)
        {
            var dataInput = requestDto.Dto;
            if (ExistedUser(dataInput.Username))
            {
                Log.Information("Username {Username} existed!", dataInput.Username);
                throw new DefinedException(ErrorCodeEnum.UserManagerExisted);
            }
            else
            {
                if (CheckAuthority(requestDto.CurrentUser))
                {
                    Log.Information("Account {Username} not authorized!", requestDto.CurrentUser);
                    throw new DefinedException(ErrorCodeEnum.NotAuthorized);
                }
                if (ExisedEmail(dataInput.Email))
                {
                    Log.Information("Email {Email} existed!", dataInput.Email);
                    throw new DefinedException(ErrorCodeEnum.EmailMangerExisted);
                }
                if (dataInput.Username.HasSpecial())
                {
                    Log.Information("User {Username} wrong format!", dataInput.Username);
                    throw new DefinedException(ErrorCodeEnum.ExitedSpecialInUsername);
                }
                if (!dataInput.Password.CheckPassFormat())
                {
                    Log.Information("Password {Password} wrong format!", dataInput.Password);
                    throw new DefinedException(ErrorCodeEnum.PasswordWrongFormat);
                }
                string codeLate = _userRepository.GetAll().OrderBy(x => x.CreatedDate).Last().Code.Base64ToString();
                var Code = EnumIDGenerate.Manager.GenerateCode(Convert.ToInt32(codeLate) + 1);
                User user = new User
                {
                    Id = Guid.NewGuid(),
                    Code = Code,
                    CountryId = dataInput.CountryId,
                    Username = dataInput.Username,
                    FullName = dataInput.FullName,
                    Users = string.Join(",", GetUsersNotAssignByGroupsRamdum(UserTypeEnum.Manager, dataInput.CountryId, dataInput.Groups).Select(x => x.Code)),
                    Groups = dataInput.Groups.SplitJoin(","),
                    Address = dataInput.Address,
                    Email = dataInput.Email,
                    Phone = dataInput.PhoneNo,
                    UserType = UserTypeEnum.Manager,
                    StartDate = dataInput.StartDate,
                    ExpiredDate = dataInput.ExpiredDate,
                    Password = string.IsNullOrEmpty(dataInput.Password) ? GeneratePassword() : EncryptService.Encrypt(dataInput.Password),
                    CreatedBy = requestDto.CurrentUser,
                };
                user = _userRepository.Insert(user);
                //_emailService.SendNewPassword(user.Email, EncryptService.Decrypt(user.Password), user.FullName, null);
                Log.Information("Create Manager Admin {Username} Successfully", dataInput.Username);
                return user.Id;
            }
        }

        public Guid CreateStaff(DataInput<StaffInput> requestDto)
        {
            var dataInput = requestDto.Dto;
            if (ExistedUser(dataInput.Username))
            {
                Log.Information("Username {Username} existed!", dataInput.Username);
                throw new DefinedException(ErrorCodeEnum.UserStaffExisted);
            }
            else
            {
                if (CheckAuthority(requestDto.CurrentUser))
                {
                    Log.Information("Account {Username} not authorized!", requestDto.CurrentUser);
                    throw new DefinedException(ErrorCodeEnum.NotAuthorized);
                }
                if (ExisedEmail(dataInput.Email))
                {
                    Log.Information("Email {Email} existed!", dataInput.Email);
                    throw new DefinedException(ErrorCodeEnum.EmailMangerExisted);
                }
                if (dataInput.Username.HasSpecial())
                {
                    Log.Information("User {Username} wrong format!", dataInput.Username);
                    throw new DefinedException(ErrorCodeEnum.ExitedSpecialInUsername);
                }
                if (!dataInput.Password.CheckPassFormat())
                {
                    Log.Information("Password {Password} wrong format!", dataInput.Password);
                    throw new DefinedException(ErrorCodeEnum.PasswordWrongFormat);
                }
                string codeLate = _userRepository.GetAll().OrderBy(x => x.CreatedDate).Last().Code.Base64ToString();
                var Code = EnumIDGenerate.Staff.GenerateCode(Convert.ToInt32(codeLate) + 1);
                User user = new User
                {
                    Id = Guid.NewGuid(),
                    Code = Code,
                    CountryId = dataInput.CountryId,
                    Username = dataInput.Username,
                    FullName = dataInput.FullName,
                    Users = string.Join(",", GetUsersNotAssignByGroupsRamdum(UserTypeEnum.Staff, dataInput.CountryId, dataInput.Group).Select(x => x.Code)),
                    Groups = dataInput.Group,
                    Address = dataInput.Address,
                    Email = dataInput.Email,
                    Phone = dataInput.PhoneNo,
                    UserType = UserTypeEnum.Staff,
                    StartDate = dataInput.StartDate,
                    ExpiredDate = dataInput.ExpiredDate,
                    Password = string.IsNullOrEmpty(dataInput.Password) ? GeneratePassword() : dataInput.Password,
                    CreatedBy = requestDto.CurrentUser,
                };
                user = _userRepository.Insert(user);
                //_emailService.SendNewPassword(user.Email, EncryptService.Decrypt(user.Password), user.FullName, null);
                Log.Information("Create Manager Admin {Username} Successfully", dataInput.Username);
                return user.Id;
            }
        }

        public UserOutput WebLogin(LoginInput requestDto)
        {
            Log.Error("Web login: {Username}/{Password}", requestDto.Username, requestDto.Password);
            User user = _userRepository.GetAll().FindField(x => x.Username.Equals(requestDto.Username));
            return Login(user, requestDto.Password);
        }

        public UserOutput View(Guid id)
        {
            User user = _userRepository.GetById(id);

            if (user == null)
            {
                Log.Error("User Not Found");
                throw new DefinedException(ErrorCodeEnum.IncorrectManagerUser);
            }
            return new UserOutput(user);
        }

        public CountTotalUsers CountTotalUsers(string currentUser)
        {
            User user = _userRepository.GetAll().FindField(x => x.Username.Equals(currentUser));
            var allUsers = GetUserListByUser(user);

            return allUsers != null ? new CountTotalUsers
            {
                Manager = allUsers.Where(x => x.UserType.Equals(UserTypeEnum.Manager.ToString())).Count(),
                Staff = allUsers.Where(x => x.UserType.Equals(UserTypeEnum.Staff.ToString())).Count(),
                Customer = allUsers.Where(x => x.UserType.Equals(UserTypeEnum.Employee.ToString())).Count(),
            } : new CountTotalUsers();
        }

        public string GetSubcriseToken(Guid userid)
        {
            var user = _userRepository.GetById(userid);
            if (user != null)
            {
                return user.SubcriseToken;

            }
            return null;
        }

        public SearchOutput SearchManager(SearchByAuthority requestDto)
            => Search(requestDto, UserTypeEnum.Manager, 5);

        public SearchOutput SearchStaff(SearchByAuthority requestDto)
            => Search(requestDto, UserTypeEnum.Manager, 7);

        public void UpdateToken(Guid userid, string token)
        {
            User user = _userRepository.GetById(userid);

            if (user != null)
            {
                user.Token = token;
                if (string.IsNullOrEmpty(token))
                    user.LoginTime = null;
                else
                    user.LoginTime = DateTime.Now;
                _userRepository.Update(user);
                _unitOfWork.Commit();
            }
        }
        #endregion

        #region Method
        private UserOutput Login(User user, string password)
        {
            if (user != null)
            {
                string passEncrypt = EncryptService.Encrypt(password);
                if (user.Password.Equals(passEncrypt))
                {
                    if (user.Status != StatusEnum.Inactive && (user.LoginFailedNumber == null || (user.LoginFailedNumber != null && user.LoginFailedNumber.Value < _maxLogin)))
                    {
                        user.LoginFailedNumber = 0;
                        var userOutput = new UserOutput(user);
                        CheckExpiredPass(ref userOutput);
                        _unitOfWork.Update(user);
                        _unitOfWork.Commit();
                        _logService.Synchronization(user.Username);
                        return userOutput;
                    }
                    else
                    {
                        throw new DefinedException(ErrorCodeEnum.UserInactive);
                    }
                }
                else
                {
                    if (user.LoginFailedNumber != null && user.LoginFailedNumber.Value >= (_maxLogin - 1) && user.Status != StatusEnum.Inactive)
                    {
                        user.Status = StatusEnum.Inactive;
                        user.LoginFailedNumber = 0;
                        _unitOfWork.Update(user);
                        _unitOfWork.Commit();
                        throw new DefinedException(ErrorCodeEnum.LoginFailed3Time);
                    }
                    if (user.Status == StatusEnum.Inactive)
                    {
                        throw new DefinedException(ErrorCodeEnum.UserInactive);
                    }
                    user.LoginFailedNumber = user.LoginFailedNumber == null ? 1 : user.LoginFailedNumber.Value + 1;
                    _unitOfWork.Update(user);
                    _unitOfWork.Commit();
                    throw new DefinedException(ErrorCodeEnum.LoginFailed);
                }
            }
            else
            {
                Log.Information("User does not existed! { requestDto }", ErrorCodeEnum.IncorrectUser);
                throw new DefinedException(ErrorCodeEnum.IncorrectUser);
            }
        }

        private void CheckExpiredPass(ref UserOutput user)
        {
            var key = user.UserType == UserTypeEnum.Employee.ToString() ? SystemConfigEnum.AppPassExpDate.ToString() : SystemConfigEnum.WebPassExpDate.ToString();
            var config = _configService.GetSystemConfig(key);
            if (config == null || (config.LastUpdateDate.HasValue && config.LastUpdateDate.Value.Date == Clock.Now.Date))
            {
                user.ExpiredPassword = "30";
                return;
            }
            if (!user.PasswordLastUpdate.HasValue)
            {
                var account = _userRepository.GetById(user.Id);
                if (account != null)
                {
                    account.PasswordLastUdt = user.PasswordLastUpdate = Clock.Now;
                    _unitOfWork.Update(account);
                    _unitOfWork.Commit();
                }
            }
            var expiredDate = user.PasswordLastUpdate.Value.AddDays((CaculateDayOfConfig(config)));
            var now = Clock.Now;
            if (expiredDate.Date < now.Date && user.UserType == UserTypeEnum.Employee.ToString())
            {
                throw new DefinedException(ErrorCodeEnum.PasswordExpired, user.Username);
            }
            else
            {
                user.ExpiredPassword = Math.Round((expiredDate - now).TotalDays, 0).ToString();
            }
        }

        private IEnumerable<DropdownList> GetUsersNotAssignByGroups(UserTypeEnum userType, string countryId, string groups = null)
        {
            IEnumerable<User> TManagers = GetAllType(UserTypeEnum.Manager, countryId);
            IEnumerable<User> TStaff = GetAllType(UserTypeEnum.Staff, countryId);
            IEnumerable<User> TEmployee = GetAllType(UserTypeEnum.Employee, countryId);
            switch (userType)
            {
                case UserTypeEnum.Manager:
                    TManagers = TManagers.AsQueryable().Where(x => !string.IsNullOrEmpty(x.Users));
                    TStaff = TStaff.Where(s => !TManagers.Any(m => m.Users.SplitTrim(",").Any(x => x.Equals(s.Code)))).AsQueryable()
                        .WhereIf(!string.IsNullOrEmpty(groups), x => groups.SplitTrim(",").Any(g => g.Equals(x.Groups)));

                    return TStaff.Count() > 0 ?
                        TStaff.AsQueryable().Select(x => new DropdownList(x.Code, x.Username)).AsEnumerable()
                        : new List<DropdownList>();

                case UserTypeEnum.Staff:
                    TStaff = TStaff.AsQueryable().Where(x => !string.IsNullOrEmpty(x.Users));
                    TEmployee = TEmployee.Where(c => !TStaff.Any(s => s.Users.SplitTrim(",").Any(x => x.Equals(c.Code)))).AsQueryable()
                        .WhereIf(!string.IsNullOrEmpty(groups), x => groups.SplitTrim(",").Any(g => g.Equals(x.Groups)));

                    return TEmployee.Count() > 0 ?
                        TEmployee.AsQueryable().Select(x => new DropdownList(x.Code, x.Username)).AsEnumerable()
                        : new List<DropdownList>();

                case UserTypeEnum.SuperAdmin:
                case UserTypeEnum.Employee:
                default:
                    throw new BadData();
            }

        }

        private IEnumerable<DropdownList> GetUsersNotAssignByGroupsRamdum(UserTypeEnum userType, string countryId, string groups = null)
        {
            bool flag = true;
            Random rd = new Random();
            IList<int> indexLs = new List<int>();
            IList<DropdownList> RamdomList = new List<DropdownList>();
            IEnumerable<DropdownList> UsersNotAssignByGroups = GetUsersNotAssignByGroups(userType, countryId, groups);
            int max = UsersNotAssignByGroups.Count();
            DropdownList[] arr = UsersNotAssignByGroups.ToArray();

            switch (userType)
            {
                case UserTypeEnum.Manager:
                    if (max <= _randomManager)
                        return UsersNotAssignByGroups;
                    break;

                case UserTypeEnum.Staff:
                    if (max <= _randomStaff)
                        return UsersNotAssignByGroups;
                    break;

                case UserTypeEnum.SuperAdmin:
                case UserTypeEnum.Employee:
                default:
                    throw new BadData();
            }
            while (flag)
            {
                int index = rd.Next(0, max - 1);
                if (indexLs.Any(x => x != index))
                {
                    indexLs.Add(index);
                    RamdomList.Add(arr[index]);
                    if (RamdomList.Count == (UserTypeEnum.Manager.Equals(userType) ? _randomManager : _randomStaff))
                        flag = false;
                }
            }
            return RamdomList;
        }

        private string GeneratePassword()
        {
            int lowercase = 4;
            int uppercase = 3;
            int numerics = 2;
            string lowers = "abcdefghijklmnopqrstuvwxyz";
            string uppers = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string number = "0123456789";

            Random random = new Random();

            string generated = "!";
            for (int i = 1; i <= lowercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    lowers[random.Next(lowers.Length - 1)].ToString()
                );

            for (int i = 1; i <= uppercase; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    uppers[random.Next(uppers.Length - 1)].ToString()
                );

            for (int i = 1; i <= numerics; i++)
                generated = generated.Insert(
                    random.Next(generated.Length),
                    number[random.Next(number.Length - 1)].ToString()
                );

            return EncryptService.Encrypt(generated);
        }

        private SearchOutput Search(SearchByAuthority requestDto, UserTypeEnum searchUserType, int fieldNumber)
        {
            if (requestDto.Search.Property.Equals(UserTypeEnum.Employee))
            {
                return ApplyPaging(requestDto.Search.DataSearch, null);
            }
            if (requestDto.Search != null)
            {
                requestDto.Search.DataSearch.SearchEqual = (new string[] { "countryId", "users", "groups" }).ToList();
            }

            List<Expression<Func<UserOutput, bool>>> listExpresion = GetExpressions<UserOutput>(requestDto.Search.DataSearch, fieldNumber);

            var user = GetUserContact(requestDto.CurrentUser);
            IQueryable<UserOutput> queryResult = GetAllType(searchUserType, user.CountryId != null ? user.CountryId : null).Select(row => new UserOutput(row, null));

            if (queryResult == null)
                return ApplyPaging(requestDto.Search.DataSearch, null);

            queryResult = SearchAuthority(queryResult, user);
            if (listExpresion != null)
            {
                foreach (Expression<Func<UserOutput, bool>> expression in listExpresion)
                {
                    queryResult = queryResult.Where(expression);
                }
            }

            queryResult = ApplyOrderBy(requestDto.Search.DataSearch, queryResult);
            return ApplyPaging(requestDto.Search.DataSearch, queryResult);
        }

        private IQueryable<UserOutput> SearchAuthority(IQueryable<UserOutput> queryResult, User user, UserTypeEnum? searchUserType = null)
        {
            searchUserType = searchUserType == null ? user.UserType : searchUserType;

            //switch (userType)
            //{
            //    case UserTypeEnum.SuperAdmin:
            //        queryResult = queryResult.ToList();
            //        break;

            //    case UserTypeEnum.Staff:
            //        queryResult = queryResult.Where(q => q.CreatedBy.Equals(user.Username) || q.Users.SplitTrim(",").Any(x => x.Equals(q.Code)));
            //        break;

            //    case UserTypeEnum.Manager:
            //        queryResult = queryResult.Where(q => q.CreatedBy.Equals(user.Username) || q.Users.SplitTrim(",").Any(x => x.Equals(q.Code)));
            //        break;

            //    case UserTypeEnum.Customer:
            //    default:
            //        return null;
            //}
            return queryResult.AsQueryable();
        }

        private IQueryable<UserOutput> GetUserListByUser(User user)
        {
            IQueryable<UserOutput> Staff = GetAllType(UserTypeEnum.Staff).Select(row => new UserOutput(row, null));
            IQueryable<UserOutput> Customer = GetAllType(UserTypeEnum.Employee).Select(row => new UserOutput(row, GetGroupContact(row.Groups)));

            switch (user.UserType)
            {
                //case UserTypeEnum.Manager:
                //    Staff = SearchAuthority(user.UserType, Staff, user);
                //    Customer = SearchAuthority(user.UserType, Customer, user);
                //    return Staff.Union(Customer);

                //case UserTypeEnum.Staff:
                //    return SearchAuthority(user.UserType, Customer, user);

                case UserTypeEnum.Employee:
                    return null;

                case UserTypeEnum.SuperAdmin:
                default:
                    return _userRepository.GetAll().Select(row => new UserOutput(row, null));
            }
        }
        #endregion

        #region function
        private bool ExistedUser(string username)
            => _userRepository.GetAll().Any(x => x.Username.Equals(username));

        private bool ExisedEmail(string email, bool allowBlank = false)
            => !(allowBlank && string.IsNullOrEmpty(email))
                ? _userRepository.GetAll().Any(x => x.Email.Equals(email))
                : false;

        private bool CheckAuthority(string username)
            => GetUserContact(username).UserType.Equals(UserTypeEnum.Employee);

        private User GetUserContact(string username)
            => _userRepository.Get(x => x.Username.Equals(username));

        private Group GetGroupContact(string group)
            => _groupRepository.Get(x => x.GroupCode.Equals(group));

        private IQueryable<User> GetAllType(UserTypeEnum type, string country = null)
            => _userRepository.GetMany(x => x.UserType.Equals(type)).WhereIf(country != null, x => x.CountryId.Equals(country));
        #endregion
    }
}