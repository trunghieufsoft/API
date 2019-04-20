using System.Linq;
using Entities.Entities;
using Common.Core.Timing;
using Common.Core.Services;
using Entities.Enumerations;
using Common.Core.Extensions;
using System.Threading.Tasks;
using Common.Core.Enumerations;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Database.EntityFrameworkCore
{
    public class DbInitializer
    {
        private readonly APIDbContext _context;
        private readonly IConfiguration _configuration;

        public DbInitializer(APIDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        #pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task Seed()
        {
        #pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            var change = false;
            
            if (!_context.Users.Any(x => x.UserType == UserTypeEnum.SuperAdmin))
            {
                List<User> listUser = new List<User>()
                {
                    new User(){Code = EnumIDGenerate.SuperAdmin.GenerateCode(100001),UserType=UserTypeEnum.SuperAdmin,Username=_configuration["SuperAdmin:Username"],FullName=_configuration["SuperAdmin:Fullname"],Phone=_configuration["SuperAdmin:Default"],Email=_configuration["SuperAdmin:Email"],Address=_configuration["SuperAdmin:Default"],Users="All",Status=StatusEnum.Active,Password=EncryptService.Encrypt(_configuration["SuperAdmin:Password"]),CreatedBy=_configuration["Auto:Create"]},
                    new User(){Code = EnumIDGenerate.SuperAdmin.GenerateCode(100002),UserType=UserTypeEnum.SuperAdmin,Username="string",FullName="SuperAdmin",Phone="NA",Email=_configuration["SuperAdmin:Email"],Address=_configuration["SuperAdmin:Default"],Users="All",Status=StatusEnum.Active,Password=EncryptService.Encrypt("string"),CreatedBy=_configuration["Auto:Create"]},
                };
                _context.Users.AddRange(listUser);
                change = true;
            }

            IList<SystemConfiguration> defaultConfig = new List<SystemConfiguration>();
            defaultConfig.Add(item: new SystemConfiguration { Key = SystemConfigEnum.ArchiveJobHistory, Value = "2", Unit = Unit.months });
            defaultConfig.Add(item: new SystemConfiguration { Key = SystemConfigEnum.ArchiveLogData, Value = "2", Unit = Unit.weeks });
            defaultConfig.Add(item: new SystemConfiguration { Key = SystemConfigEnum.OpenEAM, Value = "false", Unit = Unit.boolean });
            defaultConfig.Add(item: new SystemConfiguration { Key = SystemConfigEnum.WebPassExpDate, Value = "30", Unit = Unit.days });
            defaultConfig.Add(item: new SystemConfiguration { Key = SystemConfigEnum.AppPassExpDate, Value = "30", Unit = Unit.minutes });
            defaultConfig.Add(item: new SystemConfiguration { Key = SystemConfigEnum.WebSessExpDate, Value = "1", Unit = Unit.hour });
            defaultConfig.Add(item: new SystemConfiguration { Key = SystemConfigEnum.AppSessExpDate, Value = "1", Unit = Unit.hour });
            foreach (var item in defaultConfig)
            {
                if (!_context.SystemConfigurations.Any(x => x.Key == item.Key))
                {
                    _context.SystemConfigurations.Add(new SystemConfiguration
                    {
                        CreatedBy = _configuration["Auto:Create"],
                        Value = item.Value,
                        Unit = item.Unit,
                        Key = item.Key,
                        CreatedDate = Clock.Now
                    });
                    change = true;
                }
            }

            if (_context.Countries.Count() == 0)
            {
                List<Country> listCountry = new List<Country>()
                {
                    new Country(){CountryId="AS",CountryName="Samoa (American)",CurrencyName="USD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="AU",CountryName="Australia",CurrencyName="AUD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="BN",CountryName="Brunei",CurrencyName="BND",Region="APAC",CurrencySig="B$"},
                    new Country(){CountryId="BT",CountryName="Bhutan",CurrencyName="BTN",Region="APAC",CurrencySig="Nu"},
                    new Country(){CountryId="HK",CountryName="Hong Kong",CurrencyName="HKD",Region="APAC",CurrencySig="₣"},
                    new Country(){CountryId="ID",CountryName="Indonesia",CurrencyName="IDR",Region="APAC",CurrencySig="Rp"},
                    new Country(){CountryId="JP",CountryName="Japan",CurrencyName="JPY",Region="APAC",CurrencySig="¥"},
                    new Country(){CountryId="KH",CountryName="Kampuchea",CurrencyName="KHR",Region="APAC",CurrencySig="CR"},
                    new Country(){CountryId="KR",CountryName="Korea (South)",CurrencyName="KRW",Region="APAC",CurrencySig="₩"},
                    new Country(){CountryId="LA",CountryName="Laos",CurrencyName="LAK",Region="APAC",CurrencySig="₭"},
                    new Country(){CountryId="MM",CountryName="Myanmar",CurrencyName="MMK",Region="APAC",CurrencySig="K"},
                    new Country(){CountryId="MN",CountryName="Mongolia",CurrencyName="MNT",Region="APAC",CurrencySig="₮"},
                    new Country(){CountryId="MO",CountryName="Macao",CurrencyName="MOP",Region="APAC",CurrencySig="MOP$"},
                    new Country(){CountryId="MY",CountryName="Malaysia",CurrencyName="MYR",Region="APAC",CurrencySig="RM"},
                    new Country(){CountryId="NC",CountryName="New Caledonia",CurrencyName="CFP",Region="APAC",CurrencySig="₣"},
                    new Country(){CountryId="NR",CountryName="Nauru",CurrencyName="AUD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="NU",CountryName="Niue",CurrencyName="NZD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="NZ",CountryName="New Zealand",CurrencyName="NZD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="P2",CountryName="French Polynesia & New Caledonia",CurrencyName="CFP",Region="APAC",CurrencySig="₣"},
                    new Country(){CountryId="PF",CountryName="Polynesia (French)",CurrencyName="CFP",Region="APAC",CurrencySig="₣"},
                    new Country(){CountryId="PH",CountryName="Philippines",CurrencyName="PHP",Region="APAC",CurrencySig="₱"},
                    new Country(){CountryId="PN",CountryName="Pitcairn",CurrencyName="NZD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="PW",CountryName="Palau",CurrencyName="USD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="SG",CountryName="Singapore",CurrencyName="SGD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="TH",CountryName="Thai Lan",CurrencyName="Baht",Region="APAC",CurrencySig=""},
                    new Country(){CountryId="VN",CountryName="Viet Nam",CurrencyName="VND",Region="APAC",CurrencySig="₫"},
                };
                _context.Countries.AddRange(listCountry);
                change = true;
            }

            if (_context.Groups.Count() == 0)
            {
                List<Group> listGroup = new List<Group>()
                {
                    new Group(){GroupCode="GG",GroupName="Group 1",CreatedBy=_configuration["Auto:Create"]},
                    new Group(){GroupCode="VG",GroupName="Group 2",CreatedBy=_configuration["Auto:Create"]},
                    new Group(){GroupCode="GD",GroupName="Group 3",CreatedBy=_configuration["Auto:Create"]},
                    new Group(){GroupCode="MD",GroupName="Group 4",CreatedBy=_configuration["Auto:Create"]},
                    new Group(){GroupCode="BG",GroupName="Group 5",CreatedBy=_configuration["Auto:Create"]},
                    new Group(){GroupCode="BA",GroupName="Group 6",CreatedBy=_configuration["Auto:Create"]},
                    new Group(){GroupCode="DF",GroupName="Group 7",CreatedBy=_configuration["Auto:Create"]},
                };
                _context.Groups.AddRange(listGroup);
                change = true;
            }
            if (change)
            {
                _context.SaveChanges();
            }
        }
    }
}
