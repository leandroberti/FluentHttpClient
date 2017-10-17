using System;
using System.Net;
using System.Net.Http;

namespace LMB.FluentHttpClient
{
    /// <summary>
    /// Represents the response message with paging information from HTTP request to the specified Uri.
    /// </summary>
    /// <typeparam name="T">
    /// Type that defines the content for the HTTP response message
    /// inside the ResponseBody property.
    /// </typeparam>
    public class FluentHttpPagedResponse<T> : IFluentHttpPagedResponse<T>
    {
        /// <summary>
        /// Represents an query that can be paged.
        /// </summary>
        internal sealed class Paged : IPaged
        {

            /// <summary>
            /// Total amount of posts for the query.
            /// </summary>
            public int TotalCount { get; set; }

            /// <summary>
            /// Amount of pages that exist for the given Page and PageSize combination.
            /// </summary>
            public int PageCount { get; set; }

            /// <summary>
            /// Current page.
            /// </summary>
            public int Page { get; set; }

            /// <summary>
            /// Amount of posts for the current page.
            /// </summary>
            public int PageSize { get; set; }
        }

        #region Attributes

        private readonly Paged _paged;

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

        /// <summary>
        /// The Paging information.
        /// </summary>
        public IPaged Paging { get { return _paged; } }

        #endregion

        #region Constants
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of FluentHttpPagedResponse class with paged information.
        /// </summary>
        /// <param name="totalCount">Total amount of posts for the query.</param>
        /// <param name="page">Current page.</param>
        /// <param name="pageSize">Amount of posts for the current page.</param>
        public FluentHttpPagedResponse(int totalCount, int page, int pageSize)
        {
            _paged = new Paged
            {
                TotalCount = totalCount,
                PageCount = (pageSize > 0 && totalCount > 0)
                    ? (int)Math.Ceiling(totalCount / (double)pageSize)
                    : 0,
                Page = page,
                PageSize = pageSize
            };
        }


        /// <summary>
        /// Creates a new instance of FluentHttpPagedResponse class with default paged information.
        /// </summary>
        public FluentHttpPagedResponse()
        {
            _paged = new Paged
            {
                TotalCount = 0,
                PageCount = 0,
                Page = 0,
                PageSize = 0
            };
        }

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

        /// <summary>
        /// Set the paged information.
        /// </summary>
        /// <param name="totalCount">Total amount of posts for the query.</param>
        /// <param name="page">Current page.</param>
        /// <param name="pageSize">Amount of posts for the current page.</param>
        public void SetPagedData(int totalCount, int page, int pageSize)
        {
            _paged.Page = page;
            _paged.PageCount = (pageSize > 0 && totalCount > 0)
                ? (int)Math.Ceiling(totalCount / (double)pageSize)
                : 0;
            _paged.PageSize = pageSize;
            _paged.TotalCount = totalCount;
        }

        #endregion

        #region Protected
        #endregion

        #endregion

        #region Events
        #endregion
    }
}
