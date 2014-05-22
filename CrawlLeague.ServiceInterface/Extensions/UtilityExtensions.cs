using System;
using System.Globalization;
using System.Reflection;
using ServiceStack;

namespace CrawlLeague.ServiceInterface.Extensions
{
    public static class UtilityExtensions
    {
        public static T SanitizeDtoHtml<T>(this T obj)
        {

            if (obj != null)
            {
                PropertyInfo[] properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

                Type objType = obj.GetType();

                foreach (PropertyInfo property in properties)
                {
                    if (property.PropertyType == typeof (string) && property.SetMethod != null)
                    {
                        var propertyValue = (string)property.GetValue(obj, null);
                        objType.GetProperty(property.Name).SetValue(obj, propertyValue.StripHtml(), null);
                    }
                }
            }

            return obj;
        }

        public static string ToTitleCase(this string target)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(target);
        }
    }
}
