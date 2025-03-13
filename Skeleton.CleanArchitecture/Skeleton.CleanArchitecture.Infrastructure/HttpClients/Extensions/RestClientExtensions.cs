using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http.Headers;
using System.Text;

namespace Skeleton.CleanArchitecture.Infrastructure.HttpClients.Extensions;

public static class RestClientExtension
{
    /// <summary>
    /// Build the Request.
    /// </summary>
    /// <typeparam name="T">The type of the body of the request.</typeparam>
    /// <param name="content">The body of the request.</param>
    /// <param name="requestEndpoint">Endpoint to be called.</param>
    /// <param name="additionalHeaders">Additional headers list.</param>)
    /// <returns>HttpRequestMessage.</returns>
    public static HttpRequestMessage BuildPostRequest<T>(
        this T content,
        string requestEndpoint,
        IDictionary<string, string> additionalHeaders)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(content);

        var values = new Dictionary<string, string>();
        return BuildPostRequest(content, requestEndpoint, values, additionalHeaders);
    }

    /// <summary>
    /// Build the Request.
    /// </summary>
    /// <typeparam name="T">The type of the body of the request.</typeparam>
    /// <param name="content">The body of the request.</param>
    /// <param name="requestEndpoint">Endpoint to be called.</param>
    /// <param name="values">List of value to replace.</param>
    /// <param name="additionalHeaders">Additional headers list.</param>)
    /// <returns>HttpRequestMessage.</returns>
    public static HttpRequestMessage BuildPostRequest<T>(
        this T content,
        string requestEndpoint,
        IDictionary<string, string> values,
        IDictionary<string, string> additionalHeaders)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(values);
        ArgumentNullException.ThrowIfNull(content);

        var request = PrepareEnhancedRequestMessage(HttpMethod.Post, requestEndpoint, values, additionalHeaders);
        request.Content = content.JsonEncodeRequestObjectWithCamelCaseResolver();
        return request;
    }


    /// <summary>
    /// Build a Put Request.
    /// </summary>
    /// <typeparam name="T">The type of the body of the request.</typeparam>
    /// <param name="content">The body of the request.</param>
    /// <param name="requestEndpoint">Endpoint to be called.</param>
    /// <param name="additionalHeaders">Additional headers list.</param>)
    /// <returns>HttpRequestMessage.</returns>
    public static HttpRequestMessage BuildPutRequest<T>(
        this T content,
        string requestEndpoint,
        IDictionary<string, string> additionalHeaders)
        where T : class
    {
        ArgumentNullException.ThrowIfNull(content);

        var values = new Dictionary<string, string>();

        var request = PrepareEnhancedRequestMessage(HttpMethod.Put, requestEndpoint, values, additionalHeaders);
        request.Content = content.JsonEncodeRequestObjectWithCamelCaseResolver();
        return request;

    }

    /// <summary>
    /// Build the Request.
    /// </summary>
    /// <param name="path">The request path.</param>
    /// <param name="values">List of value to replace.</param>
    /// <returns>HttpRequestMessage.</returns>
    public static HttpRequestMessage BuildRequest(string path, IDictionary<string, string> values)
    {
        ArgumentNullException.ThrowIfNullOrWhiteSpace(path);

        var uri = values.Aggregate(path, (current, value) => current.Replace(value.Key, value.Value));
        var request = new HttpRequestMessage(HttpMethod.Get, uri);

        return request;
    }

    public static HttpRequestMessage PrepareEnhancedRequestMessage(HttpMethod method, string requestEndpoint)
        => PrepareEnhancedRequestMessage(method, requestEndpoint, new Dictionary<string, string>(), null);

    public static HttpRequestMessage PrepareEnhancedRequestMessage(HttpMethod method,
        string requestEndpoint,
        IDictionary<string, string> values)
        => PrepareEnhancedRequestMessage(method, requestEndpoint, values, null);

    /// <summary>
    /// Create a request message, complete of any additional not-default headers.
    /// </summary>
    /// <param name="method">HTTP Method.</param>
    /// <param name="requestEndpoint">Endpoint to be called.</param>
    /// <param name="values">List of value to replace.</param>
    /// <param name="additionalHeaders">Additional headers list.</param>
    /// <returns>The HttpRequestMessage to be called.</returns>
    public static HttpRequestMessage PrepareEnhancedRequestMessage(
        HttpMethod method,
        string requestEndpoint,
        IDictionary<string, string> values,
        IDictionary<string, string>? additionalHeaders)
    {
        ArgumentNullException.ThrowIfNull(values);

        var uri = values.Aggregate(requestEndpoint, (current, value) => current.Replace(value.Key, value.Value));
        var request = new HttpRequestMessage(method, uri);
        if (additionalHeaders != null && additionalHeaders.Any())
        {
            foreach (var (header, headerValue) in additionalHeaders)
            {
                request.Headers.Add(header, headerValue);
            }
        }

        return request;
    }

    /// <summary>
    /// Encode an object via JsonConvert util and return a StringContent for usage.
    /// </summary>
    /// <typeparam name="T">The data type to be serialized.</typeparam>
    /// <param name="envelope">The serializable data.</param>
    /// <returns>A StringContent object for usage.</returns>
    public static StringContent JsonEncodeRequestObjectWithCamelCaseResolver<T>(this T value)
    {
        ArgumentNullException.ThrowIfNull(value);

        var json = JsonConvert.SerializeObject(value, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        });
        return new StringContent(json, Encoding.UTF8, "application/json");
    }

    /// <summary>
    /// Encode and return an object as a JsonStreamContent.
    /// </summary>
    /// <typeparam name="T">The data type to be serialized.</typeparam>
    /// <param name="envelope">The serializable data.</param>
    /// <param name="mediaType">The mediaType</param>
    /// <returns>A StreamContent object for usage.</returns>
    public static HttpContent JsonStreamContentRequestObject<T>(this T value, string mediaType)
    {
        ArgumentNullException.ThrowIfNull(value);

        var envelopeJson = ToUtf8ByteArrayFromJsonSerializer(value);
        var envelopeStream = new MemoryStream(envelopeJson);
        var fileStreamContent = new StreamContent(envelopeStream);
        fileStreamContent.Headers.ContentType = new MediaTypeHeaderValue(mediaType);
        return fileStreamContent;
    }

    /// <summary>
    /// Encode to byte [] through Netwonsoft converter.
    /// </summary>
    /// <typeparam name="T">entity type.</typeparam>
    /// <param name="entity">entity to encode.</param>
    /// <returns>the byte array of the entity.</returns>
    private static byte[] ToUtf8ByteArrayFromJsonSerializer<T>(this T entity)
    {
        return Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(entity));
    }

    /// <summary>
    /// Deserialize a http response to an object, if the request is successful.
    /// </summary>
    /// <typeparam name="T">The data type to be deserialized.</typeparam>
    /// <param name="client">The http client we'll use.</param>
    /// <param name="endpoint">The Get endpoint.</param>
    /// <param name="readErrorManagement">A common Func to call on HttpResponse failure.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The deserialized object.</returns>
    public static async Task<T> GetAndReadAsAsync<T>(this HttpClient client,
        string endpoint,
        Func<HttpResponseMessage, Task<T>> readErrorManagement,
        CancellationToken cancellationToken)
        where T : new()
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(endpoint);
        ArgumentNullException.ThrowIfNull(readErrorManagement);

        using var responseMessage = await client.GetAsync(endpoint, cancellationToken);

        return await responseMessage.TryReadHttpResponseAsAsync(readErrorManagement, cancellationToken);
    }

    /// <summary>
    /// Deserialize a http response to an object, if the request is successful.
    /// </summary>
    /// <typeparam name="T">The data type to be deserialized.</typeparam>
    /// <param name="client">The http client we'll use.</param>
    /// <param name="requestMessage">The http request data to use.</param>
    /// <param name="readErrorManagement">A common Func to call on HttpResponse failure.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The deserialized object.</returns>
    public static async Task<T> SendAndReadAsAsync<T>(this HttpMessageInvoker client,
        HttpRequestMessage requestMessage,
        Func<HttpResponseMessage, Task<T>> readErrorManagement,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(client);
        ArgumentNullException.ThrowIfNull(requestMessage);
        ArgumentNullException.ThrowIfNull(readErrorManagement);

        var responseMessage = await client.SendAsync(requestMessage, cancellationToken);

        return await responseMessage.TryReadHttpResponseAsAsync(readErrorManagement, cancellationToken);
    }


    /// <summary>
    /// Read a response message, if it has a successful status code, otherwise use a Func to return an appropriate error-result item.
    /// </summary>
    /// <typeparam name="T">The data type to be deserialized.</typeparam>
    /// <param name="responseMessage">The response message.</param>
    /// <param name="readErrorManagement">A common Func to call on HttpResponse failure.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>T.</returns>
    public static async Task<T> TryReadHttpResponseAsAsync<T>(this HttpResponseMessage responseMessage,
        Func<HttpResponseMessage, Task<T>> readErrorManagement,
        CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(responseMessage);
        ArgumentNullException.ThrowIfNull(readErrorManagement);

        if (responseMessage.IsSuccessStatusCode)
        {
            return await responseMessage.Content.ReadAsAsync<T>(cancellationToken);
        }

        return await readErrorManagement(responseMessage);
    }
}
