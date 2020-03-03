using System;
using System.Collections.Generic;
using System.Reflection;

namespace util.BRLight
{

public class objDiff
    {
        public string field { get; set; }
        public objDiff(MemberInfo member, object value1, object value2)
        {
            field = "" + member.Name.Replace("k__BackingField", "").Replace("<", "").Replace(">", "") + ": '" + value1.ToString() + (value1.Equals(value2) ? "' == '" : "' != '") + value2.ToString() + "'";
        }
    }

public static class objHelp {

    public static List<objDiff> Comparison<T>(T x, T y)
        {
            List<objDiff> list = new List<objDiff>();

            foreach (MemberInfo m in typeof(T).GetMembers(BindingFlags.Public | BindingFlags.Instance))
                if (m.MemberType == MemberTypes.Field)
                {
                    FieldInfo field = (FieldInfo)m;
                    var xValue = field.GetValue(x);
                    var yValue = field.GetValue(y);
                    if (!yValue.Equals(xValue))
                        list.Add(new objDiff(field, yValue, xValue));
                }
                else if (m.MemberType == MemberTypes.Property)
                {
                    var prop = (PropertyInfo)m;
                    if (prop.CanRead && prop.GetGetMethod().GetParameters().Length == 0)
                    {
                        var xValue = prop.GetValue(x, null);
                        var yValue = prop.GetValue(y, null);
                        if (!xValue.Equals(yValue))
                            list.Add(new objDiff(prop, xValue, yValue));
                    }
                    else
                        continue;
                }
            return list;
        }

    public static T Clone<T>(T source)
    {
       return JSON.Deserializa<T>(JSON.Serialize<T>(source));
    }

}

}
