using System;
using Common.DTOs.Common;
using Common.DTOs.UserModel;

namespace Service.Services.Abstractions
{
    public interface IUserService
    {
        Guid CreateManager(DataInput<ManagerInput> requestDto);

        Guid CreateStaff(DataInput<StaffInput> requestDto);

        Guid CreateEmployee(DataInput<EmployeeInput> requestDto);

        void AssignUsers(string currentUser, string username);

        void Update(UpdateInput updateInput);

        void Delete(Guid id, string currentUser);

        UserOutput WebLogin(LoginInput requestDto);

        void UpdateToken(Guid userid, string subcriseToken, string token);

        UserOutput View(Guid id);

        CountTotalUsers CountTotalUsers(string currentUser);

        void EndOfDay(string currentUser);

        void Logout(string currentUser);

        string GetSubcriseToken(Guid userid);

        SearchOutput SearchManager(DataInput<SearchInput> requestDto);

        SearchOutput SearchStaff(DataInput<SearchInput> requestDto);

        SearchOutput SearchEmployee(DataInput<SearchInput> requestDto);

        void AllowUnselectGroups(UnselectGroupsInput requestDto);

        void ChangePassword(DataInput<ChangePasswordInput> requestDto);

        void ForgotPassword(DataInput<ResetPasswordInput> requestDto);
    }
}