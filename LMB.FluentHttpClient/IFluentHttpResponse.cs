using System.Net;
using System.Net.Http;

namespace LMB.FluentHttpClient
{
    /// <summary>
    /// Represents the response message from HTTP request to the specified Uri.
    /// </summary>
    /// <typeparam name="T">
    /// Type that defines the content for the HTTP response message
    /// inside the ResponseBody property.
    /// </typeparam>
    public interface IFluentHttpResponse<T>
    {
        /// <summary>
        /// The content of a HTTP response message if the response WAS NOT successful.
        /// </summary>
        string ErrorMessage { get; set; }

        /// <summary>
        /// The reason phrase which typically is sent by servers together with the status code.
        /// </summary>
        string ReasonPhrase { get; set; }

        /// <summary>
        /// The request message which led to this response message.
        /// </summary>
        HttpRequestMessage Request { get; set; }

        /// <summary>
        /// The content of a HTTP response message if the response WAS successful.
        /// </summary>
        T ResponseBody { get; set; }

        /// <summary>
        /// The status code of the HTTP response.
        /// </summary>
        HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// The response is successful if StatusCode was in the range 200-299; otherwise unsuccessful.
        /// </summary>
        bool IsSuccessStatusCode { get; }
    }
}
