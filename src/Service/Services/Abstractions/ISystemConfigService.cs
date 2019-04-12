using System.Collections.Generic;
using Entities.Entities;
using Common.DTOs.SystemConfig;

namespace Services.Services.Abstractions
{
    public interface ISystemConfigService
    {
        IEnumerable<SystemConfigOutput> GetAll(string username);
        SystemConfiguration GetSystemConfig(string key);
        void Update(List<SystemConfigOutput> requestDto, string username);
    }
}