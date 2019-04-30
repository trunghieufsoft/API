using System;
using Entities.Entities;
using Entities.Enumerations;

namespace Common.DTOs.UserModel
{
    #region Count Total Users
    public class CountTotalUsers
    {
        public CountTotalUsers(int manager = 0, int staff = 0, int employee = 0)
        {
            Manager = manager;
            Staff = staff;
            Employee = employee;
        }

        public int Manager { get; set; } = 0;
        public int Staff { get; set; } = 0;
        public int Employee { get; set; } = 0;
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
            GroupName = group?.GroupName;
            Users = user.Users;
            Status = user.Status;
            StatusStr = user.StatusStr;
            StatusDetail = !user.UserType.Equals(UserTypeEnum.Employee) ? null : new Status(user.Status, user.EndOfDay);
            StartDate = user.StartDate;
            ExpiredDate = user.ExpiredDate;
            ExpiredIn = user.UserType.Equals(UserTypeEnum.SuperAdmin) ? "(--)" : (user.ExpiredDate.Value - user.StartDate.Value).TotalDays.ToString();
            Email = user.Email;
            Address = user.Address;
            PhoneNo = user.Phone;
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
        public Status StatusDetail { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? ExpiredDate { get; set; }
        public string ExpiredIn { get; set; }
        public string Address { get; set; }
        public string PhoneNo { get; set; }
        public string Email { get; set; }
        public string ExpiredPassword { get; set; }
        public InitializeInfo InitializeInfo { get; set; }
        #endregion
    }

    public class Status
    {
        protected StatusEnum? _status { get; set; } = null;

        public Status(StatusEnum? status = null, bool endOfDay = false)
        {
            _status = status;
            switch (status)
            {
                case StatusEnum.Inactive:
                    EndOfDay = endOfDay;
                    break;
                case StatusEnum.Available:
                    Active = true;
                    Availability = true;
                    EndOfDay = endOfDay;
                    break;
                case StatusEnum.Unavailable:
                    Active = true;
                    break;
                case StatusEnum.EndOfDay:
                    Active = true;
                    EndOfDay = true;
                    break;
                default:
                    break;
            }
        }
        public bool Active { get; set; } = false;
        public bool Availability { get; set; } = false;
        public bool EndOfDay { get; set; } = false;

        public StatusEnum GetStatusEnum()
        {
            if (_status != null) return _status.Value;
            if (Active && EndOfDay) return StatusEnum.EndOfDay;
            if (Availability && Active) return StatusEnum.Available;
            if (!Availability && Active && !EndOfDay) return StatusEnum.Unavailable;
            if (!Availability && !Active) return StatusEnum.Inactive;
            return StatusEnum.Inactive;
        }
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