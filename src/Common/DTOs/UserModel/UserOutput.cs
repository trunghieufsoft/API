using System;
using System.Collections.Generic;
using Entities.Entities;
using Entities.Enumerations;

namespace Common.DTOs.UserModel
{
    #region Count Total Users
    public class CountTotalUsers
    {
        public int Manager { get; set; }
        public int Staff { get; set; }
        public int Customer { get; set; }
    }
    #endregion

    public class UserOutput
    {
        public UserOutput(User user, Group group = null)
        {
            Id = user.Id;
            Code = user.Code;
            Username = user.Username;
            FullName = user.FullName;
            CountryId = user.CountryId;
            Groups = user.Groups;
            GroupName = group != null ? group.GroupName : null;
            Users = user.Users;
            UserType = user.UserTypeStr;
            Status = user.StatusStr;
            StartDate = user.StartDate;
            ExpiredDate = user.ExpiredDate;
            Email = user.Email;
            Address = user.Address;
            Phone = user.Phone;
            CreatedBy = user.CreatedBy;
            CreatedDate = user.CreatedDate;
            LasUpdatedBy = user.LastUpdatedBy;
            LasUpdatedDate = user.LastUpdateDate;
            PasswordLastUpdate = user.PasswordLastUdt;
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
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string UserType { get; set; }
        public string Status { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string ExpiredPassword { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string LasUpdatedBy { get; set; }
        public DateTime? LasUpdatedDate { get; set; }
        public DateTime? PasswordLastUpdate { get; set; }
        #endregion
        
    }
}