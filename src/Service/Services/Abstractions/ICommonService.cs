using Common.DTOs.Common;
using System.Collections.Generic;

namespace Service.Services.Abstractions
{
    public interface ICommonService
    {
        IEnumerable<DropdownList> GetAllGroup();
        IEnumerable<DropdownList> GetAllCountry();
    }
}
