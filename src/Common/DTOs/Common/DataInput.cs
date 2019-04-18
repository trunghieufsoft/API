namespace Common.DTOs.Common
{
    public class DataInput<T>
    {
        public DataInput() { }

        public DataInput(T data, string currentUser) {
            CurrentUser = currentUser;
            Dto = data;
        }

        public string CurrentUser { get; set; }

        public T Dto { get; set; }
    }

    public class DataInput<TData, TProperties>
    {
        public DataInput() { }

        public DataInput(TProperties properties, TData data, string currentUser)
        {
            Properties = properties;
            Dto = data;
            CurrentUser = currentUser;
        }

        public string CurrentUser { get; set; }

        public TData Dto { get; set; }

        public TProperties Properties { get; set; }
    }
}
