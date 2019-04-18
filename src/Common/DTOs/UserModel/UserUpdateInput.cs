using System;
using System.ComponentModel.DataAnnotations;

namespace Common.DTOs.UserModel
{
    public class ManagerUpdateInput
    {
        #region Properties
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        public bool Status { get; set; } = true;

        [StringLength(2048)]
        [Required(ErrorMessage = "CountryId is required")]
        public string CountryId { get; set; }

        [StringLength(2048)]
        [Required(ErrorMessage = "FullName is required")]
        public string FullName { get; set; }

        [StringLength(4096)]
        [Required(ErrorMessage = "Groups is required")]
        public string Groups { get; set; }

        [StringLength(4096)]
        [Required(ErrorMessage = "Users is required")]
        public string Users { get; set; }

        [Required(ErrorMessage = "Address infomation is required")]
        public AddressInfo AddressInfo { get; set; }

        public DateTime? ExpiredDate { get; set; } = DateTime.Now.AddMonths(6);
        #endregion
    }

    public class StaffUpdateInput
    {
        #region Properties
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        public bool Status { get; set; } = true;

        [StringLength(2048)]
        [Required(ErrorMessage = "CountryId is required")]
        public string CountryId { get; set; }

        [StringLength(2048)]
        [Required(ErrorMessage = "FullName is required")]
        public string FullName { get; set; }

        [StringLength(4096)]
        [Required(ErrorMessage = "Groups is required")]
        public string Group { get; set; }

        [StringLength(4096)]
        [Required(ErrorMessage = "Users is required")]
        public string Users { get; set; }

        [Required(ErrorMessage = "Address infomation is required")]
        public AddressInfo AddressInfo { get; set; }

        public DateTime? ExpiredDate { get; set; } = DateTime.Now.AddMonths(6);
        #endregion
    }

    public class EmployeeUpdateInput
    {
        #region Properties
        [Required(ErrorMessage = "Id is required")]
        public Guid Id { get; set; }

        public bool Status { get; set; } = true;

        [StringLength(2048)]
        [Required(ErrorMessage = "CountryId is required")]
        public string CountryId { get; set; }

        [StringLength(2048)]
        [Required(ErrorMessage = "FullName is required")]
        public string FullName { get; set; }

        [StringLength(4096)]
        [Required(ErrorMessage = "Groups is required")]
        public string Group { get; set; }

        [Required(ErrorMessage = "Address infomation is required")]
        public AddressInfo AddressInfo { get; set; }

        public DateTime? ExpiredDate { get; set; } = DateTime.Now.AddMonths(6);
        #endregion
    }
}
