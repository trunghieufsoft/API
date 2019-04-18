namespace Common.DTOs.Common
{
    public class UpdateInput
    {
        public UpdateInput() { }

        public UpdateInput(dynamic dataUpdate, string currentUser)
        {
            CurrentUser = currentUser;
            DataUpdate = dataUpdate;
        }

        public string CurrentUser { get; set; }

        public dynamic DataUpdate { get; set; }
    }
}
