using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace MaloneChan {
    public static class Extensions {

        private static string _iv =  @"m7NEFgaye7V~(HZn";
        private static string _key = @"Sxghkg.4m!r+9-/g";


        public static string Decrypt(this string value) {
            if(string.IsNullOrEmpty(value)) {
                return "";
            }
            using (var aes = Aes.Create()) {
                aes.IV = Encoding.UTF8.GetBytes(_iv);
                aes.Key = Encoding.UTF8.GetBytes(_key);

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                var plain = string.Empty;
                using (var mStream = new MemoryStream(System.Convert.FromBase64String(value))) 
                using (var ctStream = new CryptoStream(mStream, decryptor, CryptoStreamMode.Read)) {
                    using (var sr = new StreamReader(ctStream)) {
                        plain = sr.ReadLine();
                    }
                    return plain;
                }
            }
        }
        public static string Encrypt(this string value) {
            if (string.IsNullOrEmpty(value)) {
                return "";
            }
            using(var aes = Aes.Create()) {
                aes.IV = Encoding.UTF8.GetBytes(_iv);
                aes.Key = Encoding.UTF8.GetBytes(_key);
                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                byte[] encrypted;
                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write)) {
                    using (var sw = new StreamWriter(cs)) {
                        sw.Write(value);
                    }
                    encrypted = ms.ToArray();
                    return (System.Convert.ToBase64String(encrypted));
                }
            }
        }
        public static string GetDefaultString(this string value, string defaultText) {
            return string.IsNullOrEmpty(value) ? defaultText : value;
        }
        public static int? ToIntN(this object value) {
            if (value == null || value == DBNull.Value) {
                return null;
            }
            int ret;
            return int.TryParse(value.ToString(), out ret) ? ret : (int?)null;
        }

        internal static int ToInt32(this object value, int defaultValue) {
            return value.ToIntN() ?? defaultValue;
        }

        internal static DateTime? ToDateTime(this object value) {
            if (value == null || value == DBNull.Value) {
                return null;
            }
            if (value is DateTime) {
                return (DateTime)value;
            }
            if (value is DateTime?) {
                return (DateTime?)value;
            }
            return null;
        }

        internal static DateTime? ToDateTime(this object value, string dateFormat) {
            if (value == null || value == DBNull.Value) {
                return null;
            }
            DateTime d;
            return DateTime.TryParseExact(value.ToString(), dateFormat, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out d) ? d : (DateTime?)null;
        }

        internal static decimal? ToDecimalN(this object value) {
            if (value == null || value == DBNull.Value) {
                return null;
            }
            decimal ret;
            return decimal.TryParse(value.ToString(), out ret) ? ret : (decimal?)null;
        }

        internal static decimal ToDecimal(this object value, decimal defaultValue) {
            return value.ToDecimalN() ?? defaultValue;
        }
        internal static float? ToFloatN(this object value) {
            if (value == null || value == DBNull.Value) {
                return null;
            }
            float ret;
            return float.TryParse(value.ToString(), out ret) ? ret : (float?)null;
        }

        internal static float ToFloat(this object value, float defaultValue) {
            return value.ToFloatN() ?? defaultValue;
        }

        internal static double? ToDoubleN(this object value) {
            if (value == null || value == DBNull.Value) {
                return null;
            }
            double ret;
            return double.TryParse(value.ToString(), out ret) ? ret : (double?)null;
        }

        internal static double ToDouble(this object value, double defaultValue) {
            return value.ToDoubleN() ?? defaultValue;
        }
        internal static long? ToLongN(this object value) {
            if (value == null || value == DBNull.Value) {
                return null;
            }
            long ret;
            return long.TryParse(value.ToString(), out ret) ? ret : (long?)null;
        }

        internal static long ToLong(this object value, long defaultValue) {
            return value.ToLongN() ?? defaultValue;
        }
    }
}
