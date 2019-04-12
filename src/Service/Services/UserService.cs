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
        private readonly int[] _random = new int[] { 5, 10 };
        private readonly int _maxLogin = 3;
        private readonly string _all = "All";
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
                    Log.Information("Account {Username} not authorized!", dataInput.Username);
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
                if (dataInput.Password.CheckPassFormat())
                {
                    Log.Information("Password {Password} wrong format!", dataInput.Password);
                    throw new DefinedException(ErrorCodeEnum.PasswordWrongFormat);
                }
                User user = new User
                {
                    Id = Guid.NewGuid(),
                    Code = (Convert.ToInt32(_userRepository.GetLate().Code) + 1).ToString(),
                    CountryId = dataInput.CountryId,
                    Username = dataInput.Username,
                    FullName = dataInput.FullName,
                    Users = string.Join(",", GetUsersNotAssignByGroupsRamdum(UserTypeEnum.Manager, dataInput.CountryId, dataInput.Groups).Select(x => x.Code)),
                    Groups = dataInput.Groups.Trim(","),
                    Address = dataInput.Address,
                    Email = dataInput.Email,
                    Phone = dataInput.PhoneNo,
                    UserType = UserTypeEnum.Manager,
                    StartDate = dataInput.StartDate,
                    ExpiredDate = dataInput.ExpiredDate,
                    Password = string.IsNullOrEmpty(dataInput.Password) ? GeneratePassword() : EncryptService.Encrypt(dataInput.Password),
                    PasswordLastUdt = Clock.Now,
                    CreatedBy = requestDto.CurrentUser,
                    CreatedDate = Clock.Now,
                };
                user = _userRepository.Insert(user);
                _emailService.SendNewPassword(user.Email, EncryptService.Decrypt(user.Password), user.FullName, null);
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
                throw new DefinedException(ErrorCodeEnum.UserManagerExisted);
            }
            else
            {
                if (CheckAuthority(requestDto.CurrentUser))
                {
                    Log.Information("Account {Username} not authorized!", dataInput.Username);
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
                if (dataInput.Password.CheckPassFormat())
                {
                    Log.Information("Password {Password} wrong format!", dataInput.Password);
                    throw new DefinedException(ErrorCodeEnum.PasswordWrongFormat);
                }
                User user = new User
                {
                    Id = Guid.NewGuid(),
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
                    PasswordLastUdt = Clock.Now,
                    CreatedBy = requestDto.CurrentUser,
                    CreatedDate = Clock.Now,
                };
                user = _userRepository.Insert(user);
                _emailService.SendNewPassword(user.Email, EncryptService.Decrypt(user.Password), user.FullName, null);
                Log.Information("Create Manager Admin {Username} Successfully", dataInput.Username);
                return user.Id;
            }
        }

        private IEnumerable<DropdownList> GetUsersNotAssignByGroups(UserTypeEnum userType, string countryId, string groups = null)
        {
            IEnumerable<User> TManagers = _userRepository.GetMany(x => x.UserType.Equals(UserTypeEnum.Manager) && x.CountryId.Equals(countryId));
            IEnumerable<User> TStaff = _userRepository.GetMany(x => x.UserType.Equals(UserTypeEnum.Staff) && x.CountryId.Equals(countryId));
            IEnumerable<User> TCustomer = _userRepository.GetMany(x => x.UserType.Equals(UserTypeEnum.Customer) && x.CountryId.Equals(countryId));
            switch (userType)
            {
                case UserTypeEnum.Manager:
                    TStaff.Where(s => !TManagers.Any(m => m.Users.TrimSplit(",").Any(x => x.Equals(s.Code)))).AsQueryable()
                        .WhereIf(string.IsNullOrEmpty(groups), x => groups.TrimSplit(",").Any(g => g.Equals(x.Groups))).ToList().AsEnumerable();

                    return TStaff.Count() > 0 ?
                        TStaff.Select(x => new DropdownList()
                        {
                            Code = x.Code,
                            Name = x.Username
                        })
                        : new List<DropdownList>();

                case UserTypeEnum.Staff:
                    TCustomer.Where(c => !TStaff.Any(s => s.Users.TrimSplit(",").Any(x => x.Equals(c.Code)))).AsQueryable()
                        .WhereIf(string.IsNullOrEmpty(groups), x => groups.TrimSplit(",").Any(g => g.Equals(x.Groups))).ToList().AsEnumerable();

                    return TCustomer.Count() > 0 ?
                        TCustomer.Select(x => new DropdownList()
                        {
                            Code = x.Code,
                            Name = x.Username
                        })
                        : new List<DropdownList>();

                case UserTypeEnum.SuperAdmin:
                case UserTypeEnum.Customer:
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
                    if (max <= _random[0])
                        return UsersNotAssignByGroups;
                    break;

                case UserTypeEnum.Staff:
                    if (max <= _random[1])
                        return UsersNotAssignByGroups;
                    break;

                case UserTypeEnum.SuperAdmin:
                case UserTypeEnum.Customer:
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
                    if (RamdomList.Count == (UserTypeEnum.Manager.Equals(userType) ? _random[0] : _random[1]))
                        flag = false;
                }
            }
            return RamdomList;
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
                Customer = allUsers.Where(x => x.UserType.Equals(UserTypeEnum.Customer.ToString())).Count(),
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
        {
            if (!requestDto.Search.Property.Equals(UserTypeEnum.SuperAdmin))
            {
                return null;
            }
            if (requestDto.Search != null)
            {
                requestDto.Search.DataSearch.SearchEqual = (new string[] { "countryId", "users", "groups" }).ToList();
            }

            List<Expression<Func<UserOutput, bool>>> listExpresion = GetExpressions<UserOutput>(requestDto.Search.DataSearch, 5);

            IQueryable<UserOutput> queryResult = _userRepository.GetAll()
                .Where(x => x.UserType.Equals(UserTypeEnum.Manager))
                .Select(row => new UserOutput(row));
            var user = _userRepository.GetAll().FindField(x => x.Username.Equals(requestDto.CurrentUser));
            if (queryResult == null) return ApplyPaging(requestDto.Search.DataSearch, null);
            queryResult = SearchAuthority(requestDto.Search.Property, queryResult, user);
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
            var key = user.UserType == UserTypeEnum.Customer.ToString() ? SystemConfigEnum.AppPassExpDate.ToString() : SystemConfigEnum.WebPassExpDate.ToString();
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
            if (expiredDate.Date < now.Date && user.UserType == UserTypeEnum.Customer.ToString())
            {
                throw new DefinedException(ErrorCodeEnum.PasswordExpired, user.Username);
            }
            else
            {
                user.ExpiredPassword = Math.Round((expiredDate - now).TotalDays, 0).ToString();
            }
        }

        private IQueryable<UserOutput> GetUserListByUser(User user)
        {
            IQueryable<UserOutput> Staff = _userRepository.GetAll()
                .Where(x => x.UserType.Equals(UserTypeEnum.Staff))
                .Select(row => new UserOutput(row));
            IQueryable<UserOutput> Customer = _userRepository.GetAll()
                .Where(x => x.UserType.Equals(UserTypeEnum.Customer))
                .Select(row => new UserOutput(row));
            switch (user.UserType)
            {
                case UserTypeEnum.Manager:
                    Staff = SearchAuthority(user.UserType, Staff, user);
                    Customer = SearchAuthority(user.UserType, Customer, user);
                    return Staff.Union(Customer);

                case UserTypeEnum.Staff:
                    return SearchAuthority(user.UserType, Customer, user);

                case UserTypeEnum.Customer:
                    return null;

                case UserTypeEnum.SuperAdmin:
                default:
                    return _userRepository.GetAll().Select(row => new UserOutput(row));
            }
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

        private IQueryable<UserOutput> SearchAuthority(UserTypeEnum userType, IQueryable<UserOutput> queryResult, User user)
        {
            switch (userType)
            {
                case UserTypeEnum.SuperAdmin:
                    queryResult = queryResult.ToList().AsQueryable();
                    break;

                case UserTypeEnum.Staff:

                    break;

                case UserTypeEnum.Manager:
                    // do user create end assign for user 
                    
                    break;

                case UserTypeEnum.Customer:
                default:
                    return null;
            }
            return queryResult;
        }

        private bool ExistedUser(string username)
            => _userRepository.GetAll().Any(x => x.Username.Equals(username));

        private bool ExisedEmail(string email, bool allowBlank = false)
            => (allowBlank && string.IsNullOrEmpty(email)) ?
                false :
                _userRepository.GetAll().Any(x => x.Email.Equals(email));

        private bool CheckAuthority(string username)
            => _userRepository.Get(x => x.Username.Equals(username)).UserType.Equals(UserTypeEnum.Customer);
        #endregion
    }
}