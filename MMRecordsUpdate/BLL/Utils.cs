using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace MMRecordsUpdate.BLL
{
    public static partial class Utils
    {
        public static string GetFormattedPhoneNumber(string phoneNo)
        {
            phoneNo = Regex.Replace(phoneNo, @"[^0-9]", "");
            return Regex.Replace(phoneNo, @"^(...)(...)(....)$", "$1-$2-$3");
        }

        public static string GetQueryString(object obj)
        {
            IEnumerable<string> properties =
                from p in obj.GetType().GetProperties()
                where p.GetValue(obj, null) != null
                select p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(obj, null).ToString());

            return string.Join("&", properties.ToArray());
        }

        public static string GetFormattedPostalCode(string postalCode)
        {
            postalCode = Regex.Replace(postalCode, @"\s+", "").ToUpper();
            return Regex.Replace(postalCode, @"^(...)(...)$", "$1 $2");
        }
    }
}