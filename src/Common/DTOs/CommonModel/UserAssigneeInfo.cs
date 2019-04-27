using System.Collections.Generic;
using System.Linq;

namespace Common.DTOs.CommonModel
{
    public class UserAssignmentInfo
    {
        public string Code { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public IEnumerable<UserAssignmentInfo> Users { get; set; } = new List<UserAssignmentInfo>();
        public int TotalUser {
            get {
                return Users.ToList().Count();
            }
            set
            {
                TotalUser = value;
            }
        }
    }

    public class UserAssignmentByTypeInfo
    {
        public UserAssignmentByTypeInfo() { }

        public UserAssignmentByTypeInfo(IEnumerable<UserAssignmentInfo> users)
        {
            Users = users;
        }

        public IEnumerable<UserAssignmentInfo> Users { get; set; } = new List<UserAssignmentInfo>();

        public int TotalUser
        {
            get
            {
                return Users.ToList().Count();
            }
            set
            {
                TotalUser = value;
            }
        }
    }
}
