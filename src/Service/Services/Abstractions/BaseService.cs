using System;
using Serilog;
using System.Linq;
using Entities.Entities;
using System.Reflection;
using Common.Core.Timing;
using Common.DTOs.Common;
using Entities.Enumerations;
using Common.Core.Extensions;
using System.Linq.Expressions;
using System.Linq.Dynamic.Core;
using Common.Core.Enumerations;
using System.Collections.Generic;

namespace Services.Services.Abstractions
{
    public abstract class BaseService
    {
        protected virtual List<Expression<Func<T, bool>>> GetExpressions<T>(SearchInput input, int number)
        {
            if (input == null || input.KeySearch == null)
            {
                return null;
            }
            ParameterExpression parameterExpression = Expression.Parameter(typeof(T));
            Expression left, right, expBody;
            List<Expression<Func<T, bool>>> listExpresion = new List<Expression<Func<T, bool>>>();
            int indexCheck = 0;
            foreach (KeyValuePair<string, string> item in input.KeySearch)
            {
                if (!string.IsNullOrEmpty(item.Value) && indexCheck <= number && (typeof(T).GetProperty(item.Key.FirstCharToUpper()) != null))
                {
                    MemberExpression parameterExp = Expression.Property(parameterExpression, item.Key);
                    PropertyInfo propertyInfo = (PropertyInfo)parameterExp.Member;
                    Type propertyType = propertyInfo.PropertyType;
                    if (propertyType == typeof(string))
                    {
                        MethodInfo methodContains = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                        left = Expression.Call(parameterExp, typeof(string).GetMethod("ToUpper", new Type[] { }));
                        string value = item.Value.ToUpper();
                        right = Expression.Constant(value, typeof(string));
                        expBody = Expression.Call(left, methodContains, right);
                    }
                    else if (propertyType == typeof(bool?))
                    {
                        expBody = Expression.Equal(parameterExp, Expression.Constant(true, typeof(bool?)));
                    }
                    else
                    {
                        var value = propertyType.IsEnum ? Enum.Parse(propertyType, item.Value.FirstCharToUpper()) : Int32.Parse(item.Value);
                        expBody = Expression.Equal(parameterExp, Expression.Constant(value, propertyType));
                    }
                    Expression<Func<T, bool>> condition = Expression.Lambda<Func<T, bool>>(expBody, parameterExpression);
                    listExpresion.Add(condition);
                }
                indexCheck++;
            }
            return listExpresion;
        }

        // Sort
        protected virtual IQueryable<T> ApplyOrderBy<T>(SearchInput input, IQueryable<T> data)
        {
            try
            {
                if (input == null || string.IsNullOrEmpty(input.OrderBy) || typeof(T).GetProperty(FirstCharToUpper(input.OrderBy)) == null)
                {
                    return data.OrderBy("LastUpdateDate desc");
                }
                if (input.IsSortDescending)
                {
                    return data.OrderBy(input.OrderBy + " desc");
                }

                return data.OrderBy(input.OrderBy + " asc");
            }
            catch (Exception e)
            {
                Log.Error("Cant not sort data {e}", e);
                return data;
            }
        }

        //Page
        protected virtual SearchOutput ApplyPaging(SearchInput input, IQueryable<object> data)
        {
            int total = 0;
            int limit = 0;
            int index = 0;
            IQueryable<object> paged = data;
            try
            {
                total = data != null ? data.Count() : 0;
                index = input != null && input.PageIndex != null && input.PageIndex > 0 ? input.PageIndex.Value : 0;
                limit = input != null && input.Limit > 0 ? input.Limit.Value : total;
                if (data != null && data.Count() > 0)
                {
                    paged = data.Skip(index * limit).Take(limit);
                }
            }
            catch (Exception e)
            {
                Log.Error(e.Message);
            }
            return new SearchOutput
            {
                TotalData = total,
                DataResult = paged,
                LimitData = limit,
                PageIndex = index
            };
        }

        public static string FirstCharToUpper(string s)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            return char.ToUpper(s[0]) + s.Substring(1);
        }

        public string GenerateID(EnumIDGenerate enumID)
        {
            string key;
            string id = DateTime.Now.Year.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Day.ToString();
            id = Convert.ToInt32(id).ToString("00000000") + "M" + DateTime.Now.Minute.ToString() + "S" + DateTime.Now.Second.ToString();
            switch (enumID)
            {
                case EnumIDGenerate.SuperAdmin:
                    key = "SA" + id;
                    break;

                case EnumIDGenerate.Manager:
                    key = "MG" + id;
                    break;

                case EnumIDGenerate.Staff:
                    key = "SF" + id;
                    break;

                case EnumIDGenerate.Customer:
                    key = "CT" + id;
                    break;

                default:
                    key = "DF" + id;
                    break;
            }
            return key;
        }

        public int CaculateDayOfConfig(SystemConfiguration obj)
        {
            try
            {
                if (obj == null)
                {
                    return 1;
                }
                if (obj.ValueUnit.Equals(Unit.days.ToString()))
                {
                    return Int32.Parse(obj.Value);
                }
                if (obj.ValueUnit.Equals(Unit.weeks.ToString()))
                {
                    return Int32.Parse(obj.Value) * 7;
                }
                if (obj.ValueUnit.Equals(Unit.months.ToString()))
                {
                    var totals = 0;
                    var value = Int32.Parse(obj.Value);
                    var current = Clock.Now;
                    for (int i = 0; i < value; i++)
                    {
                        int days = DateTime.DaysInMonth(current.Year, current.Month + i);
                        totals = totals + days;
                    }
                    return totals;
                }
            }
            catch (Exception e) { Log.Error("Something wrong when get system config {e}", e); }
            return 1;
        }

        public int CaculateMinutesOfConfig(SystemConfiguration obj)
        {
            try
            {
                if (obj == null)
                {
                    return 30;
                }
                if (obj.ValueUnit.Equals(Unit.days.ToString()))
                {
                    return Int32.Parse(obj.Value) * 1440;
                }
                if (obj.ValueUnit.Equals(Unit.weeks.ToString()))
                {
                    return Int32.Parse(obj.Value) * 7 * 1440;
                }
                if (obj.ValueUnit.Equals(Unit.minutes.ToString()))
                {
                    return Int32.Parse(obj.Value);
                }
                if (obj.ValueUnit.Equals(Unit.hour.ToString()))
                {
                    return Int32.Parse(obj.Value) * 60;
                }
                if (obj.ValueUnit.Equals(Unit.months.ToString()))
                {
                    var totals = 0;
                    var value = Int32.Parse(obj.Value);
                    var current = Clock.Now;
                    for (int i = 0; i < value; i++)
                    {
                        int days = DateTime.DaysInMonth(current.Year, current.Month + i);
                        totals = totals + days;
                    }
                    return totals * 1440;
                }
            }
            catch (Exception e) { Log.Error("Something wrong when get system config {e}", e); }
            return 30;
        }
    }
}