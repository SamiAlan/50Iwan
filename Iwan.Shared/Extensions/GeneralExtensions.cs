using Iwan.Shared.Constants;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Iwan.Shared.Extensions
{
    /// <summary>
    /// General-purpose extensions
    /// </summary>
    public static class GeneralExtensions
    {
        /// <summary>
        /// Converts a string to utf8 bytes
        /// </summary>
        public static byte[] ToUTF8Bytes(this string value) => Encoding.UTF8.GetBytes(value ?? string.Empty);

        /// <summary>
        /// Trims an object by trimming all string properties or returns a trimmed object if it was a string
        /// </summary>
        public static T Trim<T>(this T obj) where T : class
        {
            // Get object type
            var objType = typeof(T);

            // If it was string
            if (objType == typeof(string))
            {
                // Trim and return it
                return obj.ToString().Trim() as T;
            }

            // Get readable and writable and string properties
            var propertiesOfTypeString = objType.GetProperties()
                .Where(p => p.PropertyType == typeof(string) && p.CanWrite && p.CanRead);

            // Loop over all properties
            foreach (var property in propertiesOfTypeString)
            {
                // Get the propery value
                var propertyValue = (string)property.GetValue(obj);

                // If it is not null, set the trimmed version
                if (!(propertyValue is null))
                    property.SetValue(obj, propertyValue.Trim());
            }

            // Return the object
            return obj;
        }

        /// <summary>
        /// Returns if the string value is null or empty of whitespace
        /// </summary>
        public static bool IsNullOrEmptyOrWhiteSpaceSafe(this string value)
            => string.IsNullOrEmpty(value) || string.IsNullOrWhiteSpace(value);

        /// <summary>
        /// Converts an integer to its hexa-decimal form
        /// </summary>
        public static string ToHexadecimal(this int value) => value.ToString("X");

        /// <summary>
        /// Returns where the path is an image filename path
        /// </summary>
        //public static bool IsImageFileName(this string path)
        //    => SupportedFileExtensions.Images.Contains(Path.GetExtension(path).ToLower());

        /// <summary>
        /// Converts the object to json
        /// </summary>
        public static string ToJson<T>(this T obj) where T : class
            => obj is null ? throw new NullReferenceException("Passed object is null") : JsonSerializer.Serialize(obj);

        /// <summary>
        /// Converts the json content to an insance of the <typeparamref name="T"/> class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T ToObject<T>(this string json) where T : class, new()
            => string.IsNullOrEmpty(json) ? throw new ArgumentException("Passed json is empty") :
                JsonSerializer.Deserialize<T>(json);

        public static void SetIfNotEqual<TSource, TProperty>(this TSource obj, Expression<Func<TSource, TProperty>> propertyLambda, TProperty newValue)
        {
            if (propertyLambda.Body is not MemberExpression member)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    propertyLambda.ToString()));

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    propertyLambda.ToString()));

            var currentValue = propInfo.GetValue(obj);
            if (currentValue != null && !currentValue.Equals(newValue))
                propInfo.SetValue(obj, newValue);
            else if (newValue != null && !newValue.Equals(currentValue))
                propInfo.SetValue(obj, newValue);
        }

        public static bool EqualsIgnoreCase(this string value1, string value2)
        {
            if (value1 == null || value2 == null) return false;

            return value1.Equals(value2, StringComparison.InvariantCultureIgnoreCase);
        }

        public static string ToMimeType(this string fileExtension)
        {
            return fileExtension switch
            {
                ".jpeg" => "image/jpeg",
                ".jpg" => "image/jpeg",
                ".png" => "image/png",
                ".webp" => "image/webp",
                ".svg" => "image/svg+xml",
                ".pdf" => "application/png",
                ".json" => "application/json",
                ".gif" => "image/gif",
                ".jfif" => "image/jpeg",
                ".rar" => "application/vnd.rar",
                _ => $"image/{fileExtension.Replace(".", "")}"
            };
        }

        public static bool IsHtmlColor(this string colorValue)
        {
            if (colorValue.IsNullOrEmptyOrWhiteSpaceSafe()) return false;
            try { ColorTranslator.FromHtml(colorValue); } catch { return false; } return true;
        }

        public static bool IsArabic(this CultureInfo culture) => culture.Name == AppLanguages.Arabic.Culture;

        public static string ReplaceRouteParameters(this string urlWithParameters, params object[] values)
        {
            var openingCurlyIndices = new List<int>();
            var closingCurlyIndices = new List<int>();

            for (int i = 0; i < urlWithParameters.Length; i++)
            {
                if (urlWithParameters[i] == '{') openingCurlyIndices.Add(i);
                else if (urlWithParameters[i] == '}') closingCurlyIndices.Add(i);
            }

            for (int i = 0; i < openingCurlyIndices.Count; i++)
            {
                var routeParameter = urlWithParameters.Substring(openingCurlyIndices[i], closingCurlyIndices[i] - openingCurlyIndices[i] + 1);
                urlWithParameters = urlWithParameters.Replace(routeParameter, values[i].ToString());
            }

            return urlWithParameters;
        }

        public static string GetPropertyPath<T, TProperty>(this T obj, Expression<Func<T, TProperty>> expression)
        {
            MemberExpression me;
            switch (expression.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    me = ((expression.Body is UnaryExpression ue) ? ue.Operand : null) as MemberExpression;
                    break;
                default:
                    me = expression.Body as MemberExpression;
                    break;
            }

            var path = new List<string>();

            while (me != null)
            {
                path.Add(me.Member.Name);

                me = me.Expression as MemberExpression;
            }

            path.Reverse();
            return string.Join(".", path);
        }

        public static (object, string) GetPropertyValueAndRelatedObject(this object obj, string path)
        {
            var propertyNames = path.Split('.');

            if (propertyNames.Length == 1)
                return (obj, propertyNames[0]);

            var value = obj.GetType().GetProperty(propertyNames[0]).GetValue(obj, null);
            
            return GetPropertyValueAndRelatedObject(value, path.Replace(propertyNames[0] + ".", ""));
        }

        public static bool EqualsEnum<TEnum>(this int value, TEnum enumValue)
            where TEnum : struct, Enum => value == Convert.ToInt32(enumValue);

        public static T[] GetColumn<T>(this T[,] table, int columnIndex)
        {
            var column = new T[table.GetLength(0)];
            for (int i = 0; i < column.Length; i++)
            {
                column[i] = (T)table[i, columnIndex];
            }

            return column;
        }

        public static bool ContainsValues<T>(this T[] source, params object[] values)
        {
            foreach (var value in values)
            {
                if (!source.Any(v => v.Equals(value))) return false;
            }

            return true;
        }

        public static T ParseOrDefault<T>(this string value)
            where T : struct
        {
            try
            {
                var valueToReturn = new object();

                if (typeof(T) == typeof(int)) valueToReturn = Convert.ToInt32(value);
                if (typeof(T) == typeof(double)) valueToReturn = Convert.ToDouble(value);
                if (typeof(T) == typeof(float)) valueToReturn = Convert.ToSingle(value);
                if (typeof(T) == typeof(decimal)) valueToReturn = Convert.ToDecimal(value);

                return (T)valueToReturn;
            }
            catch
            {
                return default;
            }
        }
    }
}
