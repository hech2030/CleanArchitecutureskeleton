namespace Skeleton.CleanArchitecture.Application.Helpers;
internal static class TimesHelper
{
    internal static int ConvertMinutesToDays(int minutes)
    {
        var timeSpan = TimeSpan.FromMinutes(minutes);
        double totalDays = timeSpan.TotalDays;
        return (int)Math.Round(totalDays, MidpointRounding.AwayFromZero);
    }
}