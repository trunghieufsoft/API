﻿using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Entities.Entities;
using Common.Core.Timing;
using Common.Core.Services;
using Entities.Enumerations;
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

        public async Task Seed()
        {
            var change = false;
            
            if (!_context.Users.Any(x => x.UserType == UserTypeEnum.SuperAdmin))
            {
                // create SuperAdmin
                _context.Users.Add(new User
                {
                    Code = "1000",
                    Users = "All",
                    UserType = UserTypeEnum.SuperAdmin,
                    CreatedBy = _configuration["Auto:Create"],
                    Email = _configuration["SuperAdmin:Email"],
                    Phone = _configuration["SuperAdmin:Default"],
                    Address = _configuration["SuperAdmin:Default"],
                    Username = _configuration["SuperAdmin:Username"],
                    FullName = _configuration["SuperAdmin:Fullname"],
                    Password = EncryptService.Encrypt(_configuration["SuperAdmin:Password"]),
                });
                List<User> listUser = new List<User>()
                {
                    new User(){Code = "1001",UserType=UserTypeEnum.SuperAdmin,Username="SuperAdmin1",FullName="SuperAdmin",Phone="NA",Email=_configuration["SuperAdmin:Email"],Address=_configuration["SuperAdmin:Default"],Users="All",Status=StatusEnum.Active,Password=EncryptService.Encrypt(_configuration["SuperAdmin:Password"]),CreatedBy=_configuration["Auto:Create"]},
                    new User(){Code = "1002",UserType=UserTypeEnum.SuperAdmin,Username="SuperAdmin2",FullName="SuperAdmin",Phone="NA",Email=_configuration["SuperAdmin:Email"],Address=_configuration["SuperAdmin:Default"],Users="All",Status=StatusEnum.Active,Password=EncryptService.Encrypt(_configuration["SuperAdmin:Password"]),CreatedBy=_configuration["Auto:Create"]},
                    new User(){Code = "1003",UserType=UserTypeEnum.SuperAdmin,Username="SuperAdmin3",FullName="SuperAdmin",Phone="NA",Email=_configuration["SuperAdmin:Email"],Address=_configuration["SuperAdmin:Default"],Users="All",Status=StatusEnum.Active,Password=EncryptService.Encrypt(_configuration["SuperAdmin:Password"]),CreatedBy=_configuration["Auto:Create"]},
                    new User(){Code = "1004",UserType=UserTypeEnum.SuperAdmin,Username="SuperAdmin4",FullName="SuperAdmin",Phone="NA",Email=_configuration["SuperAdmin:Email"],Address=_configuration["SuperAdmin:Default"],Users="All",Status=StatusEnum.Active,Password=EncryptService.Encrypt(_configuration["SuperAdmin:Password"]),CreatedBy=_configuration["Auto:Create"]},
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
                    new Country(){CountryId="CC",CountryName="Cocos Islands",CurrencyName="AUD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="CK",CountryName="Cook Islands",CurrencyName="NZD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="CN",CountryName="Christmas Island",CurrencyName="AUD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="CX",CountryName="Christmas Island",CurrencyName="AUD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="FJ",CountryName="Fiji",CurrencyName="FJD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="FM",CountryName="Micronesia",CurrencyName="USD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="GU",CountryName="Guam",CurrencyName="USD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="HK",CountryName="Hong Kong",CurrencyName="HKD",Region="APAC",CurrencySig="₣"},
                    new Country(){CountryId="HM",CountryName="Heard and McDonald Islands",CurrencyName="AUD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="ID",CountryName="Indonesia",CurrencyName="IDR",Region="APAC",CurrencySig="Rp"},
                    new Country(){CountryId="JP",CountryName="Japan",CurrencyName="JPY",Region="APAC",CurrencySig="¥"},
                    new Country(){CountryId="KH",CountryName="Kampuchea",CurrencyName="KHR",Region="APAC",CurrencySig="CR"},
                    new Country(){CountryId="KI",CountryName="Kiribati",CurrencyName="AUD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="KR",CountryName="Korea (South)",CurrencyName="KRW",Region="APAC",CurrencySig="₩"},
                    new Country(){CountryId="LA",CountryName="Laos",CurrencyName="LAK",Region="APAC",CurrencySig="₭"},
                    new Country(){CountryId="MH",CountryName="Marshall Islands",CurrencyName="",Region="APAC",CurrencySig=""},
                    new Country(){CountryId="MM",CountryName="Myanmar",CurrencyName="MMK",Region="APAC",CurrencySig="K"},
                    new Country(){CountryId="MN",CountryName="Mongolia",CurrencyName="MNT",Region="APAC",CurrencySig="₮"},
                    new Country(){CountryId="MO",CountryName="Macao",CurrencyName="MOP",Region="APAC",CurrencySig="MOP$"},
                    new Country(){CountryId="MP",CountryName="Northern Marianas Islands",CurrencyName="USD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="MY",CountryName="Malaysia",CurrencyName="MYR",Region="APAC",CurrencySig="RM"},
                    new Country(){CountryId="NC",CountryName="New Caledonia",CurrencyName="CFP",Region="APAC",CurrencySig="₣"},
                    new Country(){CountryId="NF",CountryName="Norfolk Island",CurrencyName="AUD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="NR",CountryName="Nauru",CurrencyName="AUD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="NU",CountryName="Niue",CurrencyName="NZD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="NZ",CountryName="New Zealand",CurrencyName="NZD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="P1",CountryName="Minor Pacific Islands",CurrencyName="",Region="APAC",CurrencySig=""},
                    new Country(){CountryId="P2",CountryName="French Polynesia & New Caledonia",CurrencyName="CFP",Region="APAC",CurrencySig="₣"},
                    new Country(){CountryId="PF",CountryName="Polynesia (French)",CurrencyName="CFP",Region="APAC",CurrencySig="₣"},
                    new Country(){CountryId="PG",CountryName="Papua Niu Gini",CurrencyName="PGK",Region="APAC",CurrencySig="K"},
                    new Country(){CountryId="PH",CountryName="Philippines",CurrencyName="PHP",Region="APAC",CurrencySig="₱"},
                    new Country(){CountryId="PN",CountryName="Pitcairn",CurrencyName="NZD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="PW",CountryName="Palau",CurrencyName="USD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="SB",CountryName="Solomon Islands",CurrencyName="SBD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="SG",CountryName="Singapore",CurrencyName="SGD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="TH",CountryName="Thai Lan",CurrencyName="Baht",Region="APAC",CurrencySig=""},
                    new Country(){CountryId="TK",CountryName="Tokelau",CurrencyName="NZD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="TL",CountryName="East Timor",CurrencyName="USD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="TO",CountryName="Tonga",CurrencyName="TOP",Region="APAC",CurrencySig="T$"},
                    new Country(){CountryId="TV",CountryName="Tuvalu",CurrencyName="TVD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="TW",CountryName="Taiwan",CurrencyName="TWD",Region="APAC",CurrencySig=" NT$"},
                    new Country(){CountryId="UM",CountryName="U.S. Oceania",CurrencyName="TWD",Region="APAC",CurrencySig="$"},
                    new Country(){CountryId="VN",CountryName="Viet Nam",CurrencyName="VND",Region="APAC",CurrencySig="₫"},
                    new Country(){CountryId="VU",CountryName="Vanuatu",CurrencyName="VUV",Region="APAC",CurrencySig="VT"},
                    new Country(){CountryId="WF",CountryName="Wallis & Futuna",CurrencyName="CFP",Region="APAC",CurrencySig="₣"},
                    new Country(){CountryId="WS",CountryName="Samoa (Western)",CurrencyName="USD",Region="APAC",CurrencySig="$"},
                };
                _context.Countries.AddRange(listCountry);
                change = true;
            }

            if (_context.Groups.Count() == 0)
            {
                List<Group> listGroup = new List<Group>()
                {
                    new Group(){GroupCode="MS",GroupName="Group 1",CreatedBy=_configuration["Auto:Create"]},
                    new Group(){GroupCode="CF",GroupName="Group 2",CreatedBy=_configuration["Auto:Create"]},
                    new Group(){GroupCode="WE",GroupName="Group 3",CreatedBy=_configuration["Auto:Create"]},
                    new Group(){GroupCode="ES",GroupName="Group 4",CreatedBy=_configuration["Auto:Create"]},
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