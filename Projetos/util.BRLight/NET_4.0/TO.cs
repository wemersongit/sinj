using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;

namespace util.BRLight
{
    public static class To
    {
        /// <summary>
        /// To: Converte o resultado de uma query linq em um DataTable
        /// </summary>
        public static DataTable LinqToDataTable<T>(IEnumerable<T> varlist)
        {
            DataTable dtReturn = new DataTable();

            System.Reflection.PropertyInfo[] oProps = null;

            if (varlist == null) return dtReturn;

            foreach (T rec in varlist)
            {
                if (oProps == null)
                {
                    oProps = ((Type)rec.GetType()).GetProperties();
                    foreach (System.Reflection.PropertyInfo pi in oProps)
                    {
                        Type colType = pi.PropertyType;

                        if ((colType.IsGenericType) && (colType.GetGenericTypeDefinition()
                        == typeof(Nullable<>)))
                        {
                            colType = colType.GetGenericArguments()[0];
                        }

                        dtReturn.Columns.Add(new DataColumn(pi.Name, colType));
                    }
                }

                DataRow dr = dtReturn.NewRow();

                foreach (System.Reflection.PropertyInfo pi in oProps)
                {
                    dr[pi.Name] = pi.GetValue(rec, null) == null ? DBNull.Value : pi.GetValue(rec, null);
                }

                dtReturn.Rows.Add(dr);
            }
            return dtReturn;
        }

        /// <summary>
        /// To: Converte uma string em um byte[]
        /// </summary>
        public static byte[] StrToByteArray(string str)
        {
            var enc = Encoding.Default;
            return enc.GetBytes(str);
        }

        /// <summary>
        /// To: Converte resultado de um byte[] em uma string
        /// </summary>
        public static string ByteArrayToStr(byte[] dBytes)
        {
            var enc = EncodingDetector.DetectEncoding(dBytes);
            return enc.GetString(dBytes);
        }

        /// <summary>
        /// To: Converte Serialization Time /Date(1319266795390+0800) as String
        /// </summary>
        public static string JsonDateToDateString(Match m)
        {
            string result = string.Empty;
            DateTime dt = new DateTime(1970, 1, 1);
            dt = dt.AddMilliseconds(long.Parse(m.Groups[1].Value));
            dt = dt.ToLocalTime();
            result = dt.ToString("dd/MM/yyyy HH:mm:ss");
            return result;
        }

        /// <summary>
        /// To: Converte Date String as Json Time
        /// </summary>
        public static string DateStringToJsonDate(Match m)
        {
            string result = string.Empty;
            DateTime dt = DateTime.Parse(m.Groups[0].Value);
            dt = dt.ToUniversalTime();
            TimeSpan ts = dt - DateTime.Parse("1970-01-01");
            result = string.Format("\\/Date({0}+0800)\\/", ts.TotalMilliseconds);
            return result;
        }
    }
}
