using Newtonsoft.Json;
using System;

namespace Arima.Utilitarios
{
    public static class JsonExtesions
    {
        public static string SerializeToJson<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }
        public static T DeserializeJsonToObject<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}
