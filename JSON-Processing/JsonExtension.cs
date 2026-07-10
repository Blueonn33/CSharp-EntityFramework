using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace JSON_Processing
{
    ///<sumary>
    /// Методи за сериализация и десериализация на JSON
    /// </sumary>

    public static class JsonExtension
    {
        /// <sumary>
        /// Опции по подразбиране за сериализация на JSON
        /// </sumary>
        private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            WriteIndented = true
        };

        public static T? FromJson<T>(this string json) =>
            JsonSerializer.Deserialize<T>(json, _jsonOptions);
    }
}
