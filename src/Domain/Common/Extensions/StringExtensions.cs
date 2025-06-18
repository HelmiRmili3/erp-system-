using System.Text.Json;

namespace Backend.Domain.Common.Extensions;
public static class StringExtensions
{
    public static bool IsJson(this string source)
    {
        if (source == null)
            return false;

        try
        {
            using JsonDocument doc = JsonDocument.Parse(source);
            doc.Dispose();
            return true;
        }
        catch (JsonException)
        {
            return false;
        }
    }
}
