using System;
using System.ComponentModel.DataAnnotations;

namespace Common.DTOs.UserModel
{
    public class ManagerInput
    {
        #region Properties
        [StringLength(2048)]
        [Required(ErrorMessage = "CountryId is required")]
        public string CountryId { get; set; }

        [StringLength(2048)]
        [Required(ErrorMessage = "FullName is required")]
        public string FullName { get; set; }

        [StringLength(2048)]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [StringLength(1024)]
        public string Password { get; set; }

        [StringLength(4096)]
        [Required(ErrorMessage = "Groups is required")]
        public string Groups { get; set; }

        [StringLength(2048)]
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [StringLength(128)]
        [Required(ErrorMessage = "PhoneNo is required")]
        public string PhoneNo { get; set; }

        [StringLength(2048)]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
        
        public DateTime? StartDate { get; set; } = DateTime.Now;

        public DateTime? ExpiredDate { get; set; } = DateTime.Now.AddMonths(6);
        #endregion
    }

    public class StaffInput
    {
        #region Properties
        [StringLength(2048)]
        [Required(ErrorMessage = "CountryId is required")]
        public string CountryId { get; set; }

        [StringLength(2048)]
        [Required(ErrorMessage = "FullName is required")]
        public string FullName { get; set; }

        [StringLength(2048)]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [StringLength(1024)]
        public string Password { get; set; }

        [StringLength(4096)]
        [Required(ErrorMessage = "Groups is required")]
        public string Group { get; set; }

        [StringLength(2048)]
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [StringLength(128)]
        [Required(ErrorMessage = "PhoneNo is required")]
        public string PhoneNo { get; set; }

        [StringLength(2048)]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        public DateTime? StartDate { get; set; } = DateTime.Now;

        public DateTime? ExpiredDate { get; set; } = DateTime.Now.AddMonths(6);
        #endregion
    }

    public class EmployeeInput
    {
        #region Properties
        [StringLength(2048)]
        [Required(ErrorMessage = "CountryId is required")]
        public string CountryId { get; set; }

        [StringLength(2048)]
        [Required(ErrorMessage = "FullName is required")]
        public string FullName { get; set; }

        [StringLength(2048)]
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; }

        [StringLength(1024)]
        public string Password { get; set; }

        [StringLength(4096)]
        [Required(ErrorMessage = "Groups is required")]
        public string Group { get; set; }

        [StringLength(2048)]
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [StringLength(128)]
        [Required(ErrorMessage = "PhoneNo is required")]
        public string PhoneNo { get; set; }

        [StringLength(2048)]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        public DateTime? StartDate { get; set; } = DateTime.Now;

        public DateTime? ExpiredDate { get; set; } = DateTime.Now.AddMonths(6);
        #endregion
    }
}