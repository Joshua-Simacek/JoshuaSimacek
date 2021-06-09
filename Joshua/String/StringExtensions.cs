using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Joshua.String
{
    public static class StringExtensions
    {
        public static string FormatAsPhoneNumber(this string phoneNumber)
        {
            var len = phoneNumber.Length;
            switch (len)
            {
                case 10:
                    return FormatTenDigitPhoneNumber(phoneNumber);
                default:
                    return null;


            }
        }

        private static string FormatTenDigitPhoneNumber(string number)
        {
            var areaCode = number.Substring(0, 3);
            var exchange = number.Substring(3, 3);
            var suffix = number.Substring(6, 4);

            return $"({areaCode}) {exchange}-{suffix}";
        }
    }
}
