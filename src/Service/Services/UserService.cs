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
using Service.Services.Abstractions;
using System.Collections.Generic;
using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;

namespace Service.Services
{
    public class UserService : BaseService, IUserService
    {
        #region initial
        private readonly ILogService _logService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IRepository<User> _userRepository;
        private readonly ISystemConfigService _configService;
        private readonly IRepository<Group> _groupRepository;
        #endregion

        #region UserService
        public UserService(ILogService logService,
            IUnitOfWork unitOfWork,
            IEmailService emailService,
            IConfiguration configuration,
            IRepository<User> userRepository,
            IRepository<Group> groupRepository,
            ISystemConfigService configService)
        {
            _logService = logService;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
            _configService = configService;
            _configuration = configuration;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
        }
        #endregion

        #region API
        public UserOutput WebLogin(LoginInput requestDto)
        {
            Log.Error("Web login: {Username}/{Password}", requestDto.Username, requestDto.Password);
            User user = _userRepository.GetAll().FindField(x => x.Username.Equals(requestDto.Username));
            return Login(user, requestDto.Password);
        }

        public Guid CreateManager(DataInput<ManagerInput> requestDto) => Create(requestDto.CurrentUser, UserTypeEnum.Manager, ErrorCodeEnum.UserManagerExisted, ErrorCodeEnum.EmailManagerExisted, EnumIDGenerate.Manager, requestDto.Dto);

        public Guid CreateStaff(DataInput<StaffInput> requestDto) => Create(requestDto.CurrentUser, UserTypeEnum.Staff, ErrorCodeEnum.UserStaffExisted, ErrorCodeEnum.EmailStaffExisted, EnumIDGenerate.Staff, requestDto.Dto);

        public Guid CreateEmployee(DataInput<EmployeeInput> requestDto) => Create(requestDto.CurrentUser, UserTypeEnum.Employee, ErrorCodeEnum.UserEmployeeExisted, ErrorCodeEnum.EmailEmployeeExisted, EnumIDGenerate.Employee, requestDto.Dto);

        public void AssignUsers(string currentUser, string username)
        {
            try
            {
                User user = GetUserContact(username);
                if (CheckAuthority(currentUser, user.UserType))
                {
                    Log.Information("Account {Username} not authorized!", currentUser);
                    throw new DefinedException(ErrorCodeEnum.NotAuthorized);
                }
                var users = string.IsNullOrWhiteSpace(user.Users) ? string.Empty : user.Users;
                user.Users = users + _comma + GenerateUsers(user.UserType, user.CountryId, user.Groups, string.IsNullOrEmpty(users) ? null : user.Users.Split(_comma));
                _userRepository.Update(user);
            }
            catch (Exception e)
            {
                Log.Information(e, "Cannot User Assignment: " + e.Message);
                throw new DefinedException(ErrorCodeEnum.CannotUserAssignment);
            }
        }

        public void Update(UpdateInput updateInput)
            => Update(updateInput, updateInput.DataUpdate is ManagerUpdateInput ? ErrorCodeEnum.EmailManagerExisted : updateInput.DataUpdate is StaffUpdateInput ? ErrorCodeEnum.EmailStaffExisted : ErrorCodeEnum.EmailEmployeeExisted);

        public void Delete(Guid id, string currentUser)
        {
            var user = _userRepository.GetById(id);
            if (CheckAuthority(currentUser, user.UserType))
            {
                Log.Information("Account {Username} not authorized!", currentUser);
                throw new DefinedException(ErrorCodeEnum.NotAuthorized);
            }
            string value = Enum.GetName(typeof(UserTypeEnum), (int)user.UserType - 1);
            UserTypeEnum typeParent = (UserTypeEnum)Enum.Parse(typeof(UserTypeEnum), value, true);
            if (AllowDeleteUser(typeParent, user))
            {
                _unitOfWork.Delete(user);
                _unitOfWork.Commit();
            }
            else
            {
                Log.Information("Cannot delete user: {Username}!", user.FullName);
                throw new DefinedException(ErrorCodeEnum.CannotDeleteUser);
            }
        }

        public UserOutput View(Guid id)
        {
            User row = _userRepository.GetById(id);

            if (row == null)
            {
                Log.Error("User Not Found");
                throw new DefinedException(ErrorCodeEnum.UserNotFound);
            }
            return new UserOutput(row, row.UserType.Equals(UserTypeEnum.Staff) || row.UserType.Equals(UserTypeEnum.Employee) ? GetGroupContact(row.Groups) : null);
        }

        public CountTotalUsers CountTotalUsers(string currentUser)
        {
            User user = _userRepository.GetAll().FindField(x => x.Username.Equals(currentUser));
            var allUsers = GetUserListByUser(user);

            return allUsers.ToList().Count() > 0 ? new CountTotalUsers
            {
                Manager = allUsers.Where(x => x.UserType.Equals(UserTypeEnum.Manager.ToString())).Count(),
                Staff = allUsers.Where(x => x.UserType.Equals(UserTypeEnum.Staff.ToString())).Count(),
                Employee = allUsers.Where(x => x.UserType.Equals(UserTypeEnum.Employee.ToString())).Count(),
            } : new CountTotalUsers();
        }

        public string GetSubcriseToken(Guid userid)
        {
            User user = _userRepository.GetById(userid);
            return user?.SubcriseToken;
        }

        public void EndOfDay(string currentUser)
        {
            var employee = GetUserContact(currentUser);
            if (employee.UserType.Equals(UserTypeEnum.Employee))
            {
                if (employee != null)
                {
                    employee.Status = employee.Status.Equals(StatusEnum.Active) ? StatusEnum.EndOfDay : employee.Status;
                    employee.LastUpdateDate = Clock.Now;
                }
                _unitOfWork.Update(employee);
                _unitOfWork.Commit();
            }
            else
            {
                throw new BadData();
            }
        }

        public void Logout(string currentUser, string token)
        {
            User user = GetUserContact(currentUser);

            if (user != null)
            {
                user.Token = null;
                user.LoginTime = null;
                user.SubcriseToken = null;
            }
            _unitOfWork.Update(user);
            _unitOfWork.Commit();
        }

        public SearchOutput SearchManager(DataInput<SearchInput> requestDto)
            => Search(requestDto, UserTypeEnum.Manager, 6);

        public SearchOutput SearchStaff(DataInput<SearchInput> requestDto)
            => Search(requestDto, UserTypeEnum.Staff, 7);

        public SearchOutput SearchEmployee(DataInput<SearchInput> requestDto)
            => Search(requestDto, UserTypeEnum.Employee, 7);

        public void AllowUnselectGroups(UnselectGroupsInput input)
        {
            User user = _userRepository.GetById(input.Id);
            if (!AllowUnselectGroups(input.Country ?? user.CountryId, input.Groups ?? user.Groups, input.Users ?? user.Users, user))
            {
                Log.Information("Cannot unselect the group from the list!");
                throw new DefinedException(ErrorCodeEnum.CannotUnSelectGroup);
            }
        }

        public void ChangePassword(DataInput<ChangePasswordInput> requestDto)
        {
            User user = GetUserContact(requestDto.CurrentUser);

            if (user != null)
            {
                string passEncrypt = EncryptService.Encrypt(requestDto.Dto.CurrentPassword);
                if (user.Password.Equals(passEncrypt))
                {
                    if (!requestDto.Dto.NewPassword.CheckPassFormat())
                    {
                        Log.Information("Password {Password} wrong format!", requestDto.Dto.NewPassword);
                        throw new DefinedException(ErrorCodeEnum.PasswordWrongFormat);
                    }

                    user.Password = passEncrypt;
                    user.PasswordLastUdt = DateTime.Now;
                    user.LastUpdatedBy = requestDto.CurrentUser;
                    user.LastUpdateDate = DateTime.Now;
                    try
                    {
                        _userRepository.Update(user);
                        _unitOfWork.Commit();
                    }
                    catch (Exception e)
                    {
                        Log.Information(e, "Update Error: " + e.Message);
                        throw new SqlException("Update Error: " + e.Message, e);
                    }
                }
                else
                {
                    Log.Information("Change Password Error: Incorrect Password!");
                    throw new DefinedException(ErrorCodeEnum.IncorrectPassword);
                }
            }
        }

        public void ResetEndOfDate()
        {
            var userQuery = _userRepository.GetAll().Where(x => x.Status.Equals(StatusEnum.EndOfDay) == true && x.LastUpdateDate.HasValue && x.LastUpdateDate.Value.Date != Clock.Now.Date);
            if (userQuery != null && userQuery.Count() > 0)
            {
                foreach (var user in userQuery)
                {
                    user.Status = user.Status.Equals(StatusEnum.EndOfDay) ? StatusEnum.Available : user.Status;
                    user.LastUpdateDate = Clock.Now;
                    user.LastUpdatedBy = _configuration["Auto:Update"];
                    _unitOfWork.Update(user);
                }
                _unitOfWork.Commit();
            }
        }

        public void UpdateToken(Guid userid, string subcriseToken, string token)
        {
            User user = _userRepository.GetById(userid);

            if (user != null)
            {
                user.Token = token;
                user.SubcriseToken = subcriseToken;
                if (string.IsNullOrEmpty(token))
                    user.LoginTime = null;
                else
                    user.LoginTime = DateTime.Now;
                _userRepository.Update(user);
                _unitOfWork.Commit();
            }
        }

        public void ForgotPassword(DataInput<ResetPasswordInput> requestDto)
        {
            User user = GetUserContact(requestDto.CurrentUser);
            if (user != null)
            {
                if (requestDto.Dto.Email.Trim().Equals(user.Email.Trim()))
                {
                    var diffInSeconds = Math.Round(user.PasswordLastUdt.HasValue ? (Clock.Now - user.PasswordLastUdt.Value).TotalSeconds : 300);
                    if (diffInSeconds >= 300)
                    {
                        user.PasswordLastUdt = Clock.Now;
                        user.LastUpdateDate = Clock.Now;
                        user.Password = GeneratePassword();
                        try
                        {
                            _emailService.SendForgotPassword(user.Email, EncryptService.Decrypt(user.Password), user.FullName, user.UserType.Equals(UserTypeEnum.Employee));
                            Log.Information("Reset Password For User: {Username} Successfully.", user.Username);
                            _unitOfWork.Update(user);
                            _unitOfWork.Commit();
                        }
                        catch (Exception e)
                        {
                            Log.Error(user.FullName + "reset password error {e}", e);
                            throw new DefinedException(ErrorCodeEnum.CannotSendEmailToResetPassword);
                        }
                    }
                    else
                    {
                        throw new DefinedException(ErrorCodeEnum.MultiplePasswordResetting, 300 - diffInSeconds);
                    }
                }
                else
                {
                    Log.Information("Email is incorrect!", ErrorCodeEnum.IncorrectEmail);
                    throw new DefinedException(ErrorCodeEnum.IncorrectEmail);
                }
            }
            else
            {
                Log.Information("User is incorrect!", ErrorCodeEnum.IncorrectUser);
                throw new DefinedException(ErrorCodeEnum.IncorrectUser);
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
                Log.Information("User does not existed!", ErrorCodeEnum.IncorrectUser);
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
            if (!user.InitializeInfo.PasswordLastUpdate.HasValue)
            {
                var account = _userRepository.GetById(user.Id);
                if (account != null)
                {
                    account.PasswordLastUdt = user.InitializeInfo.PasswordLastUpdate = Clock.Now;
                    _unitOfWork.Update(account);
                    _unitOfWork.Commit();
                }
            }
            var expiredDate = user.InitializeInfo.PasswordLastUpdate.Value.AddDays((CaculateDayOfConfig(config)));
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

        private IEnumerable<DropdownList> GetUsersHasNotBeenAssignedByGroups(UserTypeEnum userType, string countryId, string groups = null)
        {
            IEnumerable<User> TManagers = GetAllType(UserTypeEnum.Manager, countryId);
            IEnumerable<User> TStaff = GetAllType(UserTypeEnum.Staff, countryId);
            IEnumerable<User> TEmployee = GetAllType(UserTypeEnum.Employee, countryId);
            switch (userType)
            {
                case UserTypeEnum.Manager:
                    TManagers = TManagers.AsQueryable().Where(x => !string.IsNullOrEmpty(x.Users));
                    TStaff = TStaff.Where(s => !TManagers.Any(m => m.Users.SplitTrim(_comma).Any(x => x.Equals(s.Code)))).AsQueryable()
                        .WhereIf(!string.IsNullOrEmpty(groups), x => groups.SplitTrim(_comma).Any(g => g.Equals(x.Groups)));

                    return TStaff.Count() > 0 ?
                        TStaff.AsQueryable().Select(x => new DropdownList(x.Code, x.Username)).AsEnumerable()
                        : new List<DropdownList>();

                case UserTypeEnum.Staff:
                    TStaff = TStaff.AsQueryable().Where(x => !string.IsNullOrEmpty(x.Users));
                    TEmployee = TEmployee.Where(c => !TStaff.Any(s => s.Users.SplitTrim(_comma).Any(x => x.Equals(c.Code)))).AsQueryable()
                        .WhereIf(!string.IsNullOrEmpty(groups), x => groups.SplitTrim(_comma).Any(g => g.Equals(x.Groups)));

                    return TEmployee.Count() > 0 ?
                        TEmployee.AsQueryable().Select(x => new DropdownList(x.Code, x.Username)).AsEnumerable()
                        : new List<DropdownList>();

                case UserTypeEnum.SuperAdmin:
                case UserTypeEnum.Employee:
                default:
                    return new List<DropdownList>();
            }
        }

        private IEnumerable<DropdownList> GetUsersHasNotBeenAssignedByGroupsRamdum(UserTypeEnum type, string countryId, string groups = null, string[] hasValue = null)
        {
            bool flag = true;
            Random rd = new Random();
            IList<int> indexLs = new List<int>();
            int randomStaff = 0, randomManager = 0;
            IList<DropdownList> RamdomList = new List<DropdownList>();
            int had = (hasValue ?? new string[0]).Count();
            switch (type)
            {
                case UserTypeEnum.Manager:
                    randomManager = had < _randomManager
                        ? _randomManager - had
                        : 0;
                    if (randomManager == 0)
                    {
                        return RamdomList;
                    }
                    break;

                case UserTypeEnum.Staff:
                    randomStaff = had < _randomStaff
                        ? _randomStaff - had
                        : 0;
                    if (randomStaff == 0)
                    {
                        return RamdomList;
                    }
                    break;

                default:
                case UserTypeEnum.SuperAdmin:
                case UserTypeEnum.Employee:
                    return RamdomList;
            }
            IEnumerable<DropdownList> UsersHasNotBeenAssignedByGroups = GetUsersHasNotBeenAssignedByGroups(type, countryId, groups);
            int max = UsersHasNotBeenAssignedByGroups.Count();
            if (max == 0)
                return new List<DropdownList>();
            DropdownList[] arr = UsersHasNotBeenAssignedByGroups.ToArray();

            switch (type)
            {
                case UserTypeEnum.Manager:
                    if (max <= randomManager)
                        return UsersHasNotBeenAssignedByGroups;
                    break;

                case UserTypeEnum.Staff:
                    if (max <= randomStaff)
                        return UsersHasNotBeenAssignedByGroups;
                    break;

                default:
                case UserTypeEnum.SuperAdmin:
                case UserTypeEnum.Employee:
                    return RamdomList;
            }
            while (flag)
            {
                int index = rd.Next(0, max - 1);
                if (indexLs.Any(x => x != index))
                {
                    indexLs.Add(index);
                    RamdomList.Add(arr[index]);
                    if (RamdomList.Count == (UserTypeEnum.Manager.Equals(type) ? _randomManager : _randomStaff))
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

        private Guid Create(string currentUser, UserTypeEnum userType, ErrorCodeEnum exitedUser, ErrorCodeEnum exitedEmail, EnumIDGenerate genCode, dynamic dataObject)
        {
            if (!(dataObject is ManagerInput || dataObject is StaffInput || dataObject is EmployeeInput))
                throw new BadData();
            if (ExistedUser(dataObject.Username))
            {
                Log.Information("Username {Username} existed!", (string)dataObject.Username);
                throw new DefinedException(exitedUser);
            }
            else
            {
                if (CheckAuthority(currentUser, userType))
                {
                    Log.Information("Account {Username} not authorized!", currentUser);
                    throw new DefinedException(ErrorCodeEnum.NotAuthorized);
                }
                if (ExisedEmail((string)dataObject.Email))
                {
                    Log.Information("Email {Email} existed!", (string)dataObject.Email);
                    throw new DefinedException(exitedEmail);
                }
                if (((string)dataObject.Username).HasSpecial())
                {
                    Log.Information("User {Username} wrong format!", (string)dataObject.Username);
                    throw new DefinedException(ErrorCodeEnum.ExitedSpecialInUsername);
                }
                if (!((string)dataObject.Password).CheckPassFormat())
                {
                    Log.Information("Password {Password} wrong format!", (string)dataObject.Password);
                    throw new DefinedException(ErrorCodeEnum.PasswordWrongFormat);
                }
                string codeLate = _userRepository.GetAll().OrderBy(x => x.CreatedDate).Last().Code.Base64ToString();
                var Code = genCode.GenerateCode(Convert.ToInt32(codeLate) + 1);
                User user = new User
                {
                    Id = Guid.NewGuid(),
                    Code = Code,
                    CountryId = (string)dataObject.CountryId,
                    Username = (string)dataObject.Username,
                    FullName = (string)dataObject.FullName,
                    Users = dataObject is EmployeeInput ? null : GenerateUsers(userType, (string)dataObject.CountryId, (string)dataObject.Groups),
                    Groups = dataObject is ManagerInput ? (string)dataObject.Groups : (string)dataObject.Group,
                    Address = (string)dataObject.Address,
                    Email = (string)dataObject.Email,
                    Phone = (string)dataObject.PhoneNo,
                    UserType = userType,
                    Status = dataObject is EmployeeInput ? StatusEnum.Available : StatusEnum.Active,
                    StartDate = (DateTime?)dataObject.StartDate,
                    ExpiredDate = (DateTime?)dataObject.ExpiredDate,
                    Password = string.IsNullOrEmpty(dataObject.Password) ? GeneratePassword() : dataObject.Password,
                    CreatedBy = currentUser,
                };
                user = _userRepository.Insert(user);
                //_emailService.SendNewPassword(user.Email, EncryptService.Decrypt(user.Password), user.FullName, null);
                Log.Information("Create " + user.UserTypeStr + ": {Username} Successfully", user.Username);
                return user.Id;
            }
        }

        private bool AllowUnselectGroups(string country, string groups, string users, User user)
        {
            bool returnResult = true;
            var userArr = users.SplitTrim(_comma);
            var groupsArr = groups.SplitTrim(_comma);
            if (!user.CountryId.Equals(country) || groupsArr.Any(g => user.Groups.SplitTrim(_comma).Any(x => x.Equals(g))))
                return returnResult;
            //check has any account used user?
            if (!user.UserType.Equals(UserTypeEnum.SuperAdmin))
            {
                UserTypeEnum typeParent = (UserTypeEnum)Enum.Parse(typeof(UserTypeEnum), Enum.GetName(typeof(UserTypeEnum), (int)user.UserType - 1), true);
                if (!typeParent.Equals(UserTypeEnum.SuperAdmin))
                {
                    IEnumerable<User> listUserParent = GetAllType(typeParent, country);
                    returnResult = !listUserParent.Any(u => u.Users.SplitTrim(_comma).Any(x => user.Code.Equals(x)));
                    if (!returnResult)
                    {
                        listUserParent = listUserParent.Where(u => u.Users.SplitTrim(_comma).Any(x => u.Code.Equals(x)));
                        returnResult = listUserParent.Any(u => u.Groups.SplitTrim(_comma).Any(g => groupsArr.Any(x => x.Equals(g))));
                    }
                }
            }

            //check user list has reference to groups
            if (!user.UserType.Equals(UserTypeEnum.Employee) && returnResult)
            {
                UserTypeEnum typeChildren = (UserTypeEnum)Enum.Parse(typeof(UserTypeEnum), Enum.GetName(typeof(UserTypeEnum), (int)user.UserType + 1), true);
                IEnumerable<User> listUserChildren = GetAllType(typeChildren, country);
                listUserChildren = listUserChildren.Where(u => userArr.Any(x => u.Code.Equals(x)));
                returnResult = groupsArr.Any(arr => listUserChildren.Any(u => u.Groups.SplitTrim(_comma).Equals(arr)));
            }
            return returnResult;
        }

        private void Update(UpdateInput requestDto, ErrorCodeEnum exitedEmail)
        {
            dynamic dataInput = requestDto.DataUpdate;
            if (!(dataInput is ManagerUpdateInput || dataInput is StaffUpdateInput || dataInput is EmployeeUpdateInput))
                throw new BadData();
            User user = _userRepository.GetById((Guid)dataInput.Id);
            if (user != null)
            {
                if (ExisedEmail(email: (string)dataInput.AddressInfo.Email, idUpdate: (Guid)dataInput.Id))
                {
                    Log.Information("Email {Email} existed!", (string)dataInput.AddressInfo.Email);
                    throw new DefinedException(exitedEmail);
                }
                #region handle variable
                bool dataStatus = (user.ExpiredDate.Value - DateTime.Now).TotalDays < 0 ? false : (bool)dataInput.Status;
                StatusEnum valueType = dataStatus
                    ? dataInput is EmployeeUpdateInput ? StatusEnum.Available : StatusEnum.Active
                    : dataInput is EmployeeUpdateInput ? StatusEnum.Unavailable : StatusEnum.Inactive;
                if (!AllowUnselectGroups((string)dataInput.CountryId, dataInput is ManagerUpdateInput ? (string)dataInput.Groups : (string)dataInput.Group, (string)dataInput.Users, user))
                {
                    Log.Information("Cannot unselect the group from the list!", ErrorCodeEnum.CannotUnSelectGroup);
                    throw new DefinedException(ErrorCodeEnum.CannotUnSelectGroup);
                }
                #endregion

                #region update user
                user.Status = valueType;
                user.CountryId = (string)dataInput.CountryId;
                user.FullName = (string)dataInput.FullName;
                user.Groups = dataInput is ManagerUpdateInput ? (string)dataInput.Groups : (string)dataInput.Group;
                user.Users = dataInput is EmployeeUpdateInput ? null : !user.CountryId.Equals((string)dataInput.CountryId) ? (string)dataInput.Users : string.Empty;
                user.Address = (string)dataInput.AddressInfo.Address;
                user.Phone = (string)dataInput.AddressInfo.PhoneNo;
                user.Email = (string)dataInput.AddressInfo.Email;
                user.ExpiredDate = (DateTime?)dataInput.ExpiredDate;
                user.LastUpdatedBy = (string)dataInput.CurrentUser;
                user.LastUpdateDate = DateTime.Now;
                #endregion

                try
                {
                    _unitOfWork.Update(user);
                    _unitOfWork.Commit();
                }
                catch
                {
                    Log.Information("Cannot Update " + user.UserTypeStr + ": {Username}.", user.Username);
                    throw new DefinedException(ErrorCodeEnum.CannotUpdateUsers);
                }
            }
            else
            {
                Log.Information("Not Found User: {Username}.", (string)dataInput.Username);
                throw new DefinedException(ErrorCodeEnum.UserNotFound);
            }
        }

        private SearchOutput Search(DataInput<SearchInput> requestDto, UserTypeEnum searchUserType, int fieldNumber)
        {
            if (CheckAuthority(requestDto.CurrentUser, searchUserType))
            {
                return ApplyPaging(requestDto.Dto, new List<UserOutput>().AsQueryable());
            }
            if (requestDto.Dto != null)
            {
                requestDto.Dto.SearchEqual = (new string[] { "countryId", "users", "groups" }).ToList();
            }

            List<Expression<Func<UserOutput, bool>>> listExpresion = GetExpressions<UserOutput>(requestDto.Dto, fieldNumber);

            var user = GetUserContact(requestDto.CurrentUser);
            IQueryable<UserOutput> queryResult = GetAllType(user.UserType.Equals(UserTypeEnum.SuperAdmin) ? searchUserType : user.UserType, user.CountryId ?? null)
                .Select(row => new UserOutput(row, row.UserType.Equals(UserTypeEnum.Staff) || row.UserType.Equals(UserTypeEnum.Employee) ? GetGroupContact(row.Groups) : null));

            if (queryResult == null)
                return ApplyPaging(requestDto.Dto, new List<UserOutput>().AsQueryable());

            queryResult = SearchAuthority(queryResult, user, searchUserType);
            if (listExpresion != null)
            {
                foreach (Expression<Func<UserOutput, bool>> expression in listExpresion)
                {
                    queryResult = queryResult.Where(expression);
                }
            }

            queryResult = ApplyOrderBy(requestDto.Dto, queryResult);
            return ApplyPaging(requestDto.Dto, queryResult);
        }

        /// <summary>
        /// SearchAuthority
        /// </summary>
        /// <param name="queryResult">list all account follow user type</param>
        /// <param name="user">account login</param>
        /// <param name="searchUserType">key recursively</param>
        /// <returns></returns>
        private IQueryable<UserOutput> SearchAuthority(IQueryable<UserOutput> queryResult, User user, UserTypeEnum searchUserType)
        {
            if (user.UserType.Equals(UserTypeEnum.SuperAdmin))
                searchUserType = user.UserType;
            if (string.IsNullOrEmpty(user.Users))
                return new List<UserOutput>().AsQueryable();
            switch (searchUserType)
            {
                case UserTypeEnum.Manager:
                    var staffUsers = GetAllType(UserTypeEnum.Staff, user.CountryId ?? null)
                        .Select(row => new UserOutput(row, row.UserType.Equals(UserTypeEnum.Staff) || row.UserType.Equals(UserTypeEnum.Employee) ? GetGroupContact(row.Groups) : null));
                    return staffUsers.Where(staff => user.Users.SplitTrim(_comma).Any(x => x.Equals(staff.Code)));

                case UserTypeEnum.Staff:
                    if (user.UserType.Equals(UserTypeEnum.Manager))
                        return SearchAuthority(queryResult, user, user.UserType);
                    IEnumerable<UserOutput> employeeUsers = GetAllType(UserTypeEnum.Employee, user.CountryId ?? null)
                        .Select(row => new UserOutput(row, row.UserType.Equals(UserTypeEnum.Staff) || row.UserType.Equals(UserTypeEnum.Employee) ? GetGroupContact(row.Groups) : null));
                    IEnumerable<UserOutput> filterStaff = queryResult.Where(staff => !string.IsNullOrEmpty(staff.Users)).ToList();
                    if (filterStaff.Count() == 0)
                        return new List<UserOutput>().AsQueryable();
                    IEnumerable<UserOutput> result = employeeUsers.Where(employee => filterStaff.Any(staff => staff.Users.SplitTrim(_comma).Any(x => x.Equals(employee.Code))));
                    return result.AsQueryable();

                case UserTypeEnum.Employee:
                    if (user.UserType.Equals(UserTypeEnum.Manager))
                    {
                        queryResult = SearchAuthority(queryResult, user, UserTypeEnum.Staff);
                        user.UserType = UserTypeEnum.Staff;
                        return SearchAuthority(queryResult, user, user.UserType);
                    }
                    IQueryable<UserOutput> employeeUser = GetAllType(UserTypeEnum.Employee, user.CountryId ?? null).Select(row => new UserOutput(row, null));
                    return employeeUser.Where(employee => user.Users.SplitTrim(_comma).Any(x => x.Equals(employee.Code)));

                default:
                case UserTypeEnum.SuperAdmin:
                    return queryResult.ToList().AsQueryable();
            }
        }

        private IQueryable<UserOutput> GetUserListByUser(User user)
        {
            IQueryable<UserOutput> users = GetAllType(user.UserType, user.CountryId ?? null)
                .Select(row => new UserOutput(row, row.UserType.Equals(UserTypeEnum.Staff) || row.UserType.Equals(UserTypeEnum.Employee) ? GetGroupContact(row.Groups) : null));

            switch (user.UserType)
            {
                case UserTypeEnum.Manager:
                    IEnumerable<UserOutput> Staff = SearchAuthority(users, user, UserTypeEnum.Staff);
                    IEnumerable<UserOutput> Employee = SearchAuthority(users, user, UserTypeEnum.Employee);
                    var result = Staff.Union(Employee);
                    return result.AsQueryable();

                case UserTypeEnum.Staff:
                    return SearchAuthority(users, user, UserTypeEnum.Employee);

                case UserTypeEnum.Employee:
                    return null;

                case UserTypeEnum.SuperAdmin:
                default:
                    return _userRepository.GetAll()
                        .Select(row => new UserOutput(row, row.UserType.Equals(UserTypeEnum.Staff) || row.UserType.Equals(UserTypeEnum.Employee) ? GetGroupContact(row.Groups) : null));
            }
        }
        #endregion

        #region function
        private bool ExistedUser(string username)
            => _userRepository.GetAll().Any(x => x.Username.Equals(username));

        private bool ExisedEmail(string email, bool allowBlank = false, Guid? idUpdate = null)
            => !(allowBlank && string.IsNullOrEmpty(email))
                ? _userRepository.GetAll().WhereIf(idUpdate != null, x => !idUpdate.Equals(x.Id)).Any(x => x.Email.Equals(email))
                : false;

        private bool CheckAuthority(string username, UserTypeEnum type)
        {
            var userType = GetUserContact(username).UserType;
            switch (userType)
            {
                case UserTypeEnum.Manager:
                case UserTypeEnum.Staff:
                    return (int)userType >= (int)type;

                case UserTypeEnum.Employee:
                    return true;

                case UserTypeEnum.SuperAdmin:
                default:
                    return false;
            }
        }

        private bool AllowDeleteUser(UserTypeEnum type, User user)
            => !(string.IsNullOrEmpty(user.Users) ||
            GetAllType(type, user.CountryId).Where(u => !string.IsNullOrEmpty(u.Users)).Any(u => u.Users.SplitTrim(_comma).Any(x => x.Equals(user.Code))));

        private User GetUserContact(string username)
            => _userRepository.Get(x => x.Username.Equals(username));

        private Group GetGroupContact(string group)
            => _groupRepository.Get(x => x.GroupCode.Equals(group));

        private IQueryable<User> GetAllType(UserTypeEnum type, string country = null)
            => _userRepository.GetMany(x => x.UserType.Equals(type)).WhereIf(country != null, x => x.CountryId.Equals(country));

        private string GenerateUsers(UserTypeEnum type, string countryId, string groups, string[] hasValue = null)
            => string.Join(_comma, GetUsersHasNotBeenAssignedByGroupsRamdum(type, countryId, groups, hasValue).Select(x => x.Id));
        #endregion
    }
}