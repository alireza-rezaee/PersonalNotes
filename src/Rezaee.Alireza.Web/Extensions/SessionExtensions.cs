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
            foreach (var toastkey in ToastKeys(session.Keys))
            {
                toasts.Add(session.GetObjectFromJson<ToastViewModel>(toastkey));
                session.Remove(toastkey);
            }
            return toasts;
        }

        public static void SetToast(this ISession session, ToastViewModel toast) => session.SetObjectAsJson($"toast-{Guid.NewGuid()}", toast);

        private static bool HasToastSigniture(string key) => key.StartsWith("toast-");

        private static string[] ToastKeys(IEnumerable<string> keys) => keys.Where(key => HasToastSigniture(key)).ToArray();
    }
}
