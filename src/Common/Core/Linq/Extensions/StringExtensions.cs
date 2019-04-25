using Common.Core.Enumerations;
using Common.Core.Extensions;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Core.Linq.Extensions
{
    public static class StringExtensions
    {
        private static readonly string _comma = ",";

        public static bool BuildAny(this string source, string value = "")
        {
            if (string.IsNullOrEmpty(source))
                throw new ArgumentNullException("source is require!");
            if (string.IsNullOrEmpty(value))
                return false;
            string[] sources = source.ToUpper().SplitTrim(_comma);
            string[] values = value.ToUpper().SplitTrim(_comma);
            return sources.Any(sorc => values.Any(val => val.Equals(sorc)));
        }
    }
}
