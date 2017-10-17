using System;
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
    public class FluentHttpResponse<T> : IFluentHttpResponse<T>
    {
        #region Attributes
        #endregion

        #region Properties

        /// <summary>
        /// The request message which led to this response message.
        /// </summary>
        public HttpRequestMessage Request { get; set; }

        /// <summary>
        /// The content of a HTTP response message if the response WAS NOT successful.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// The reason phrase which typically is sent by servers together with the status code.
        /// </summary>
        public string ReasonPhrase { get; set; }

        /// <summary>
        /// The status code of the HTTP response.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// The content of a HTTP response message if the response WAS successful.
        /// </summary>
        public T ResponseBody { get; set; }

        #endregion

        #region Constants
        #endregion

        #region Constructors
        #endregion

        #region Methods

        #region Private
        #endregion

        #region Public

        /// <summary>
        /// The response is successful if StatusCode was in the range 200-299; otherwise unsuccessful.
        /// </summary>
        public bool IsSuccessStatusCode
        {
            get
            {
                try
                {
                    return ((int)StatusCode >= 200) && ((int)StatusCode <= 299);
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        #endregion

        #region Protected
        #endregion

        #endregion

        #region Events
        #endregion
    }
}
