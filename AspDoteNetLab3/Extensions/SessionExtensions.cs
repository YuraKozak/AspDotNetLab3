using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AspDotNetLab3.Extensions
{
    public static class SessionExtensions
    {

        public static void SetSession(this ISession session, string key, string value)
        {
            session.SetString(key, value);
        }

        public static string GetSession(this ISession session, string key)
        {
            var value = session.GetString(key);
            if (value == null)
            {
                return null;
            }
            else
            {
                return value;
            }
        }
    }
}
