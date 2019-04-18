using System;
using System.ComponentModel.DataAnnotations;

namespace Common.DTOs.UserModel
{
    public class UnselectGroupsInput
    {
        [Required(ErrorMessage = "User id is required")]
        public Guid Id { get; set; }

        [StringLength(128)]
        public string Country { get; set; }

        [StringLength(128)]
        public string Groups { get; set; }

        [StringLength(4096)]
        public string Users { get; set; }
    }
}
