using System.Globalization;

namespace Skeleton.CleanArchitecture.Application.Helpers.ISO8601;
public static class Iso8601FormatHelper
{
    private const string IsoFormatDateOnly = "yyyy-MM-dd";
    public static string DateTimeToIso8601(DateTime? dateTime) => dateTime.HasValue ? DateTimeToIso8601(dateTime.Value) : string.Empty;
    public static string DateTimeToIso8601(DateTime dateTime)
    {
        return dateTime.ToString("o");
    }

    public static string GetCurrentDateTimeIso8601Format()
    {
        return DateTime.Now.ToString("o");
    }

    public static string GetCurrentDateOnlyIso8601Format()
    {
        return DateTime.Now.ToString(IsoFormatDateOnly);
    }

    public static string DateOnlyToIso8601Format(DateTime? dateTime) =>
        dateTime.HasValue ? DateOnlyToIso8601Format(dateTime.Value) : string.Empty;
    public static string DateOnlyToIso8601Format(DateTime dateTime)
    {
        return dateTime.Date.ToString(IsoFormatDateOnly);
    }

    public static bool IsIso8601Format(string date) {
        return DateOnly.TryParseExact(
                date,
                IsoFormatDateOnly,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out _);
    }
}
