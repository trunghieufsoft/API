using System;
using Entities.Entities;
using Entities.Enumerations;

namespace Common.DTOs.UserModel
{
    #region Count Total Users
    public class CountTotalUsers
    {
        public int Manager { get; set; }
        public int Staff { get; set; }
        public int Employee { get; set; }
    }
    #endregion

    public class UserOutput
    {
        public UserOutput(User user, Group group = null)
        {
            Id = user.Id;
            Code = user.Code;
            CountryId = user.CountryId;
            UserType = user.UserTypeStr;
            Username = user.Username;
            FullName = user.FullName;
            Groups = user.Groups;
            GroupName = group != null ? group.GroupName : null;
            Users = user.Users;
            Status = user.Status;
            StatusStr = user.StatusStr;
            ExpiredDate = user.ExpiredDate;
            ExpiredIn = user.UserType.Equals(UserTypeEnum.SuperAdmin) ? "(--)" : (user.ExpiredDate.Value - user.StartDate.Value).TotalDays.ToString();
            AddressInfo = new AddressInfo(user);
            InitializeInfo = new InitializeInfo(user);
        }

        #region Properties
        public Guid Id { get; set; }
        public string Code { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string CountryId { get; set; }
        public string Groups { get; set; }
        public string GroupName { get; set; }
        public string Users { get; set; }
        public string UserType { get; set; }
        public StatusEnum Status { get; set; }
        public string StatusStr { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string ExpiredIn { get; set; }
        public AddressInfo AddressInfo { get; set; }
        public string ExpiredPassword { get; set; }
        public InitializeInfo InitializeInfo { get; set; }
        #endregion
    }

    public class InitializeInfo
    {
        public InitializeInfo(User user)
        {
            CreatedBy = user.CreatedBy;
            CreatedDate = user.CreatedDate;
            LasUpdatedBy = user.LastUpdatedBy;
            LasUpdatedDate = user.LastUpdateDate;
            PasswordLastUpdate = user.PasswordLastUdt;
        }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LasUpdatedBy { get; set; }
        public DateTime? LasUpdatedDate { get; set; }
        public DateTime? PasswordLastUpdate { get; set; }
    }
}