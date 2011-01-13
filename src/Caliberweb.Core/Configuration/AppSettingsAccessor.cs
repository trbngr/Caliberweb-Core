using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Globalization;
using System.Linq;

namespace Caliberweb.Core.Configuration
{
    public abstract class AppSettingsAccessor
    {
        private readonly NameValueCollection settings;

        protected AppSettingsAccessor()
            : this(ConfigurationManager.AppSettings)
        { }

        internal AppSettingsAccessor(NameValueCollection collection)
        {
            settings = collection;
        }

        protected T GetValue<T>(string key, T defaultValue)
        {
            try
            {
                return GetValue<T>(key);
            }
            catch
            {
                return defaultValue;
            }
        }

        protected T GetValue<T>(string key)
        {
            string value = GetValue(key);
            return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
        }

        protected string GetValue(string key)
        {
            if (HasKey(key))
            {
                return settings[key];
            }

            throw new KeyNotFoundException(key);
        }

        protected bool HasKey(string key)
        {
            return settings.AllKeys.Contains(key, StringComparer.InvariantCultureIgnoreCase);
        }

        protected IEnumerable<string> GetValues(string key, string seperator)
        {
            var value = GetValue(key);

            return value.Split(new[] { seperator }, StringSplitOptions.RemoveEmptyEntries);
        }
    }
}