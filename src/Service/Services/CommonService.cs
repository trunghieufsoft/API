using System.Linq;
using Entities.Entities;
using Common.DTOs.Common;
using Database.UnitOfWork;
using Database.Repositories;
using System.Collections.Generic;
using Service.Services.Abstractions;

namespace Service.Services
{
    public class CommonService : BaseService, ICommonService
    {
        #region initial
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<Country> _countryRepository;
        private readonly IRepository<Group> _groupRepository;
        #endregion

        #region CommonService
        public CommonService(IUnitOfWork unitOfWork,
            IRepository<Country> countryRepository,
            IRepository<Group> groupRepository)
        {
            _unitOfWork = unitOfWork;
            _groupRepository = groupRepository;
            _countryRepository = countryRepository;
        }
        #endregion

        #region API
        public IEnumerable<DropdownList> GetAllCountry() 
            =>_countryRepository.GetAll().Select(x => new DropdownList(x.CountryId, x.CountryName));

        public IEnumerable<DropdownList> GetAllGroup()
            => _groupRepository.GetAll().Select(x => new DropdownList(x.GroupCode, x.GroupName));
        #endregion
    }
}
