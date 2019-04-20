using Common.DTOs.Common;
using Common.DTOs.CommonModel;
using Entities.Enumerations;
using System.Collections.Generic;

namespace Service.Services.Abstractions
{
    public interface ICommonService
    {
        IEnumerable<DropdownList> GetAllGroup();

        IEnumerable<DropdownList> GetAllCountry();

        UserAssignmentInfo GetUsersAssign(string username);

        IEnumerable<UserAssignmentInfo> GetUsersAllTypeAssignByCountry(UserTypeEnum userType, string country = null);
    }
}
