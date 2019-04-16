using System;
using Common.DTOs.Common;
using Common.DTOs.UserModel;

namespace Services.Services.Abstractions
{
    public interface IUserService
    {
        Guid CreateManager(DataInput<ManagerInput> requestDto);

        Guid CreateStaff(DataInput<StaffInput> requestDto);

        Guid CreateEmployee(DataInput<EmployeeInput> requestDto);

        UserOutput WebLogin(LoginInput requestDto);

        void UpdateToken(Guid userid, string token);

        UserOutput View(Guid id);

        CountTotalUsers CountTotalUsers(string currentUser);

        string GetSubcriseToken(Guid userid);

        SearchOutput SearchManager(DataInput<SearchInput> requestDto);

        SearchOutput SearchStaff(DataInput<SearchInput> requestDto);

        SearchOutput SearchEmployee(DataInput<SearchInput> requestDto);
    }
}