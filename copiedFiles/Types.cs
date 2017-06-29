using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.ComponentModel.DataAnnotations;

namespace cogsBurger
{
    [AttributeUsage(AttributeTargets.All)]
    public class ExclusiveRangeAttribute : RangeAttribute
    {

        public ExclusiveRangeAttribute(int minimum, int maximum) : base(minimum, maximum) { }

        public override bool IsValid(object value)
        {
            // Automatically pass if value is null or empty. RequiredAttribute should be used to assert a value is not empty.
            if (value == null)
            {
                return true;
            }
            string s = value as string;
            if (s != null && String.IsNullOrEmpty(s))
            {
                return true;
            }
            dynamic val = value;
            dynamic min = Minimum;
            dynamic max = Maximum;

            if (val <= min) { return false; }
            if (val >= max) { return false; }
            return true;
        }
    }


    [AttributeUsage(AttributeTargets.All)]
    public class StringValidationAttribute : ValidationAttribute
    {
        Regex Rgx;
        List<string> Enumerations;

        public StringValidationAttribute(string[] enumerations, string pattern = null)
        {
            if (pattern != null) { this.Rgx = new Regex(pattern); }
            if (enumerations != null) { this.Enumerations = new List<string>(enumerations); }
        }

        public override bool IsValid(object value)
        {
            if(value == null)
            {
                return true;
            }
            if (Enumerations != null && !Enumerations.Contains(value.ToString())) { return false; }
            // check regex Pattern
            if (Rgx != null && !this.Rgx.IsMatch(value.ToString())) { return false; }
            return true;
        }
    }


    public struct CogsDate
    {
        public DateTimeOffset DateTime { get; set; }
        public DateTimeOffset Date { get; set; }
        public Tuple<int, int> GYearMonth { get; set; }
        public int GYear { get; set; }
        public TimeSpan Duration { get; set; }
        public enum CogsDateType { DateTime, Date, GYearMonth, GYear, Duration } 
        public CogsDateType UsedType { get; private set; }

        public CogsDate(DateTimeOffset item, bool isDate = false) : this()
        {
            if (isDate)
            {
                Date = item;
                UsedType = CogsDateType.Date;
            }
            else
            {
                DateTime = item;
                UsedType = CogsDateType.DateTime;
            }
        }

        public CogsDate(Tuple<int, int> item) : this()
        {
            GYearMonth = item;
            UsedType = CogsDateType.GYearMonth;
        }

        public CogsDate(int item) : this()
        {
            GYear = item;
            UsedType = CogsDateType.GYear;
        }

        public CogsDate(TimeSpan item) : this()
        {
            Duration = item;
            UsedType = CogsDateType.Duration;
        }

        public string GetValue()
        {
            switch (UsedType)
            {
                case CogsDateType.DateTime:
                    {
                        return DateTime.DateTime.ToString();
                    }
                case CogsDateType.Date:
                    {
                        return Date.Date.ToString();
                    }
                case CogsDateType.GYearMonth:
                    {
                        return GYearMonth.Item1 + "-" + GYearMonth.Item2;
                    }
                case CogsDateType.GYear:
                    {
                        return GYear.ToString();
                    }
                case CogsDateType.Duration:
                    {
                        return Duration.Duration().ToString();
                    }
            }
            throw new InvalidOperationException();
        }
    }
}