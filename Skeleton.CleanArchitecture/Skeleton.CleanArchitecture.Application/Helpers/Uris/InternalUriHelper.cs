using Microsoft.AspNetCore.Http;
using System.Text;

namespace Skeleton.CleanArchitecture.Application.Helpers.Uris;
public static class InternalUriHelper
{
    private static readonly string SchemeDelimiter = Uri.SchemeDelimiter;

    public static string GetDisplayUrl(this HttpRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);
        static string SafeValue(string? value) => value ?? string.Empty;

        return new StringBuilder()
            .Append(SafeValue(request.Scheme))
            .Append(string.IsNullOrEmpty(SafeValue(request.Scheme)) ? string.Empty : SchemeDelimiter)
            .Append(SafeValue(request.Host.Value))
            .Append(SafeValue(request.PathBase.Value))
            .Append(SafeValue(request.Path.Value))
            .Append(SafeValue(request.QueryString.Value))
            .ToString();
    }
}

