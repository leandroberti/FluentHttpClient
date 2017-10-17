namespace LMB.FluentHttpClient
{
    /// <summary>
    /// Represents the response message with paging information from HTTP request to the specified Uri.
    /// </summary>
    /// <typeparam name="T">
    /// Type that defines the content for the HTTP response message
    /// inside the ResponseBody property.
    /// </typeparam>
    public interface IFluentHttpPagedResponse<T> : IFluentHttpResponse<T>
    {
        /// <summary>
        /// The Paging information.
        /// </summary>
        IPaged Paging { get; }
    }
}
