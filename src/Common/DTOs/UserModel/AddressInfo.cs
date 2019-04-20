using Entities.Entities;
using System.ComponentModel.DataAnnotations;

namespace Common.DTOs.UserModel
{
    public class AddressInfo
    {
        public AddressInfo() { }

        public AddressInfo(User user)
        {
            Email = user.Email;
            Address = user.Address;
            PhoneNo = user.Phone;
        }

        [StringLength(2048)]
        [Required(ErrorMessage = "Address is required")]
        public string Address { get; set; }

        [StringLength(128)]
        [Required(ErrorMessage = "PhoneNo is required")]
        public string PhoneNo { get; set; }

        [StringLength(2048)]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }
    }
}
