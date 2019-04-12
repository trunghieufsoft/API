namespace Common.DTOs.Common
{
    public class DropdownList
    {
        public DropdownList(object code = null, string name = null)
        {
            Code = code;
            Name = name;
        }
        public object Code { get; set; }
        public string Name { get; set; }
    }
}