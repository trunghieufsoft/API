using Common.DTOs.Common;
using System.Collections.Generic;

namespace Common.DTOs.CommonModel
{
    public class DetailCountryInfo
    {
        public DetailCountryInfo() { }
        public IEnumerable<DropdownList> Countries { get; set; }
        public IEnumerable<DropdownList> Groups { get; set; }
        public IEnumerable<UserAssignmentInfo> Users { get; set; }
        public IEnumerable<UserAssignmentByTypeInfo> UserByType { get; set; }
    }
}
