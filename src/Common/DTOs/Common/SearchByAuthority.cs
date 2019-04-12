using Entities.Enumerations;

namespace Common.DTOs.Common
{
    public class SearchByAuthority
    {
        public SearchInput<UserTypeEnum> Search { get; set; }

        public string CurrentUser { get; set; }
    }
}
