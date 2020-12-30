using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AglieFrame.NoSql
{
    public static class Utils
    {
        public static string ToJson(this object obj)
        {
            return JsonSerializer.Serialize(obj);
        }
        public static T ToObj<T>(this object json)
        {
            var data = json.ToString();
            return JsonSerializer.Deserialize<T>(data);
        }
    }
}
