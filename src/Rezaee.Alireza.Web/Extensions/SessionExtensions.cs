using Microsoft.AspNetCore.Http;
using Rezaee.Alireza.Web.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Rezaee.Alireza.Web.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value) => session.SetString(key, JsonSerializer.Serialize(value));

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }

        public static IList<ToastViewModel> GetToasts(this ISession session)
        {
            var toasts = new List<ToastViewModel>();

            for (var i = 0; i < session.Keys.Count(); i++)
            {
                var key = session.Keys.ElementAt(i);
                if (HasToatsSigniture(key))
                {
                    toasts.Add(session.GetObjectFromJson<ToastViewModel>(key));
                    session.Remove(key);
                }
            }

            return toasts;
        }

        public static void SetToast(this ISession session, ToastViewModel toast) => session.SetObjectAsJson($"toast-{DateTime.Now.Ticks}", toast);

        private static bool HasToatsSigniture(string key) => key.StartsWith("toast-");
    }
}
