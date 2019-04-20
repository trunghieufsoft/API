using Common.DTOs.Common;
using System.Collections.Generic;

namespace Common.DTOs.CommonModel
{
    public class UserAssignmentInfo
    {
        public string Username { get; set; }
        public string FullName { get; set; }
        public IEnumerable<DropdownList> Users { get; set; } = new List<DropdownList>();
        public int TotalUser { get; set; } = 0;
    }
}
