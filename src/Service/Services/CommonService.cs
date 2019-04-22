using System;
using System.Linq;
using Entities.Entities;
using Common.DTOs.Common;
using Database.UnitOfWork;
using Database.Repositories;
using System.Collections.Generic;
using Service.Services.Abstractions;
using Entities.Enumerations;
using Common.Core.Linq.Extensions;
using Serilog;
using Common.Core.Exceptions;
using Common.Core.Enumerations;
using Common.DTOs.CommonModel;
using Common.Core.Extensions;

namespace Service.Services
{
    public class CommonService : BaseService, ICommonService
    {
        #region initial
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<Group> _groupRepository;
        #endregion

        #region CommonService
        public CommonService(IUnitOfWork unitOfWork,
            IRepository<User> userRepository,
            IRepository<Country> countryRepository,
            IRepository<Group> groupRepository)
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _groupRepository = groupRepository;
            _countryRepository = countryRepository;
        }
        #endregion

        #region API
        public IEnumerable<DropdownList> GetAllCountry()
            => _countryRepository.GetAll().Select(x => new DropdownList(x.CountryId, x.CountryName));

        public IEnumerable<DropdownList> GetAllGroup()
            => _groupRepository.GetAll().Select(x => new DropdownList(x.GroupCode, x.GroupName));

        public DetailCountryInfo GetDetailCountry(string username)
        {
            User user = _userRepository.Get(x => x.Username.Equals(username));
            IEnumerable<DropdownList> country = new List<DropdownList>();
            IEnumerable<DropdownList> groups = new List<DropdownList>();
            IEnumerable<UserAssignmentInfo> users = new List<UserAssignmentInfo>();
            if (user != null)
            {
                country = _countryRepository.GetAll().WhereIf(!user.CountryId.Equals(_all), x => x.CountryId.Equals(user.CountryId)).Select(row => new DropdownList(row.CountryId, row.CountryName));
                groups = _groupRepository.GetAll().AsEnumerable().WhereIf(!user.Groups.Equals(_all), item => user.Groups.SplitTrim(_comma).Any(g => g.Equals(item.GroupCode)))
                    .Select(row => new DropdownList(row.GroupCode, row.GroupName));
                users = GetUsersAllTypeAssignByCountry(user.UserType, user.CountryId);
            }
            return new DetailCountryInfo()
            {
                Countries = country,
                Users = users,
                Groups = groups
            };
        }

        public IEnumerable<UserAssignmentInfo> GetUsersAllTypeAssignByCountry(UserTypeEnum userType, string country = null)
        {
            IList<UserAssignmentInfo> result = new List<UserAssignmentInfo>();
            if (userType.Equals(UserTypeEnum.SuperAdmin))
                return new List<UserAssignmentInfo>() { GetUsersAssign("System") };

            _userRepository.GetMany(x => x.UserType.Equals(userType))
                .WhereIf(country != null, x => x.CountryId.Equals(country))
                .ForEach(item =>
                {
                    result.Add(item: GetUsersAssign(item.Username));
                });
            return result;
        }

        public UserAssignmentInfo GetUsersAssign(string username)
        {
            User user = _userRepository.Get(x => x.Username.Equals(username));
            if (user != null)
            {
                var Users = GetUsersAssignByUser(user, user.UserType.Equals(UserTypeEnum.SuperAdmin));
                return new UserAssignmentInfo()
                {
                    Username = user.Username,
                    FullName = user.FullName,
                    Users = Users,
                    TotalUser = Users.ToList().Count()
                };
            }
            else
            {
                throw new DefinedException(ErrorCodeEnum.UserNotFound);
            }
        }
        #endregion

        #region Method
        private IEnumerable<DropdownList> GetUsersAssignByUser(User user, bool isSuper = false)
        {
            if (user.UserType.Equals(UserTypeEnum.Employee))
                return new List<DropdownList>();
            UserTypeEnum type = (UserTypeEnum)Enum.Parse(typeof(UserTypeEnum), Enum.GetName(typeof(UserTypeEnum), (int)user.UserType + 1), true);
            IEnumerable<User> users = _userRepository.GetMany(x => x.UserType.Equals(type)).WhereIf(user.CountryId != null, x => x.CountryId.Equals(user.CountryId));
            return isSuper
                ? users.Select(row => new DropdownList(row.Code, row.Username, row.FullName))
                : users.Where(u => user.Users.Split(_comma).Any(x => x.Equals(u.Code))).Select(row => new DropdownList(row.Code, row.Username, row.FullName));
        }
        #endregion
    }
}
