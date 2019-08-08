using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace System
{
    public static class MyExtensions
    {
        public static long toPersianToEnglish(this string persianStr)
        {
            persianStr = persianStr.Replace(",", "");

            Dictionary<char, char> LettersDictionary = new Dictionary<char, char>
            {
                ['۰'] = '0',
                ['۱'] = '1',
                ['۲'] = '2',
                ['۳'] = '3',
                ['۴'] = '4',
                ['۵'] = '5',
                ['۶'] = '6',
                ['۷'] = '7',
                ['۸'] = '8',
                ['۹'] = '9',
            };
            foreach (var item in persianStr)
            {
                persianStr = persianStr.Replace(item, LettersDictionary[item]);
            }
            persianStr = Regex.Replace(persianStr, "[^0-9]+", string.Empty);

            return Convert.ToInt64(persianStr);
        }
    }
}
