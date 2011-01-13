using System;
using System.Data;
using System.Linq;

namespace Caliberweb.Core.Persistance
{
    public static class DataReaderExtensions
    {
        public static bool GetBoolean(this IDataReader reader, string name)
        {
            return GetValueOrDefault<bool>(reader, name, reader.GetBoolean);
        }

        public static bool GetCtBool(this IDataReader reader, string name)
        {
            return GetValueOrDefault(reader, name, ordinal => Convert.ToBoolean(reader.GetInt16(ordinal)));
        }

        public static DateTime GetDateTime(this IDataReader reader, string name)
        {
            return GetValueOrDefault<DateTime>(reader, name, reader.GetDateTime);
        }

        public static decimal GetDecimal(this IDataReader reader, string name)
        {
            return GetValueOrDefault<decimal>(reader, name, reader.GetDecimal);
        }

        public static double GetDouble(this IDataReader reader, string name)
        {
            return GetValueOrDefault(reader, name, ordinal => Convert.ToDouble(reader.GetDecimal(ordinal)));
        }

        public static T GetEnum<T>(this IDataReader reader, string name) where T : struct
        {
            Type enumType = typeof (T);

            string value = GetString(reader, name);

            const StringComparison C = StringComparison.InvariantCultureIgnoreCase;

            if (value == default(string) || (!Enum.IsDefined(enumType, value) && !Enum.GetNames(enumType).Any(n => n.Equals(value, C))))
            {
                return default(T);
            }

            return (T) Enum.Parse(enumType, value, true);
        }

        public static Guid GetGuid(this IDataReader reader, string name)
        {
            return GetValueOrDefault<Guid>(reader, name, reader.GetGuid);
        }

        public static int GetInt(this IDataReader reader, string name)
        {
            return GetValueOrDefault<int>(reader, name, reader.GetInt32);
        }

        public static long GetLong(this IDataReader reader, string name)
        {
            return GetValueOrDefault<long>(reader, name, reader.GetInt64);
        }

        public static DateTime? GetNullableDateTime(this IDataReader reader, string name)
        {
            DateTime value;

            if (TryGetValue(reader, name, reader.GetDateTime, out value))
            {
                return value;
            }

            return null;
        }

        public static int? GetNullableInt(this IDataReader reader, string name)
        {
            int value;

            if (TryGetValue(reader, name, reader.GetInt32, out value))
            {
                return value;
            }

            return null;
        }

        public static short GetShort(this IDataReader reader, string name)
        {
            return GetValueOrDefault<short>(reader, name, reader.GetInt16);
        }

        public static string GetString(this IDataReader reader, string name)
        {
            return GetValueOrDefault<string>(reader, name, reader.GetString);
        }

        private static T GetValueOrDefault<T>(IDataRecord record, string name, Func<int, T> accessor)
        {
            T value;
            TryGetValue(record, name, accessor, out value);
            return value;
        }

        private static bool TryGetValue<T>(IDataRecord record, string name, Func<int, T> accessor, out T value)
        {
            int ordinal = record.GetOrdinal(name);

            if (record.IsDBNull(ordinal))
            {
                value = default(T);
                return false;
            }

            value = accessor(ordinal);
            return true;
        }
    }
}