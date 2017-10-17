using LMB.GenericEntityBase;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace LMB.FluentHttpClient
{
    /// <summary>
    /// Represents the IFluentHttpResponse. Used for fluent interface construction.
    /// </summary>
    public interface ICanChangeContent
    {
        /// <summary>
        /// Set the HTTP Accept Header for application/xml content.
        /// </summary>
        /// <returns>The Fluent Http Client.</returns>
        ICanAddConfig WithXmlContent();

        /// <summary>
        /// Set the HTTP Accept Header for application/json content.
        /// </summary>
        /// <returns>The Fluent Http Client.</returns>
        ICanAddConfig WithJsonContent();
    }

    /// <summary>
    /// Represents the IFluentHttpResponse. Used for fluent interface construction.
    /// </summary>
    public interface ICanAddConfig
    {
        /// <summary>
        /// Adds a new header to the HTTP client.
        /// </summary>
        /// <param name="name">Header name.</param>
        /// <param name="values">Header values collection.</param>
        /// <returns>The Fluent Http Client.</returns>
        ICanAddConfig AddHeader(string name, IEnumerable<string> values);

        /// <summary>
        /// Adds a new header to the HTTP client.
        /// </summary>
        /// <param name="name">Header name.</param>
        /// <param name="value">Header value.</param>
        /// <returns>The Fluent Http Client.</returns>
        ICanAddConfig AddHeader(string name, string value);

        /// <summary>
        /// Set the new client timeout.
        /// Repleces the default 300 seconds timeoute value.
        /// </summary>
        /// <param name="seconds">Timeout seconds value.</param>
        /// <returns>The Fluent Http Client.</returns>
        ICanAddConfig SetTimeout(int seconds);

        /// <summary>
        /// Set the new client timeout.
        /// Repleces the default 300 seconds timeoute value.
        /// </summary>
        /// <param name="hours">Timeout hour value.</param>
        /// <param name="minutes">Timeout minuts value.</param>
        /// <param name="seconds">Timeout seconds value.</param>
        /// <returns>The Fluent Http Client.</returns>
        ICanAddConfig SetTimeout(int hours, int minutes, int seconds);

        /// <summary>
        /// Set the HTTP Authorization header with a Bearer token value.
        /// </summary>
        /// <param name="token">The token string.</param>
        /// <returns>The Fluent Http Client.</returns>
        ICanAddConfig WithAuthorization(string token);

        /// <summary>
        /// Set the current page value for a paged request.
        /// </summary>
        /// <param name="value">Current page value.</param>
        /// <returns>The Fluent Http Client.</returns>
        ICanSetPage FromPage(int value);

        /// <summary>
        /// Send a DELETE request to the specified Uri.
        /// </summary>
        /// <typeparam name="T">Type that defines the content for the HTTP response message.</typeparam>
        /// <returns>
        /// A new instance of FluentHttpResponse class containing:
        ///     HttpStatusCode: the status code of the HTTP response.
        ///     ReasonPhrase: the reason phrase which typically is sent by servers together with the status code.
        ///     Request: the request message which led to this response message.
        /// This instance may also contain:
        ///     ErrorMessage: the content of a HTTP response message if the response WAS NOT successful.
        ///     ResponseBody: the content of a HTTP response message if the response WAS successful.
        /// Note:
        /// The response is successful if System.Net.Http.HttpResponseMessage.StatusCode was in the range 200-299; otherwise unsuccessful.
        /// </returns>
        IFluentHttpResponse<T> Delete<T>();

        /// <summary>
        /// Send a DELETE request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">Type that defines the content for the HTTP response message.</typeparam>
        /// <returns>
        /// A task that, when completed, contains a new instance of FluentHttpResponse class containing:
        ///     HttpStatusCode: the status code of the HTTP response.
        ///     ReasonPhrase: the reason phrase which typically is sent by servers together with the status code.
        ///     Request: the request message which led to this response message.
        /// This instance may also contain:
        ///     ErrorMessage: the content of a HTTP response message if the response WAS NOT successful.
        ///     ResponseBody: the content of a HTTP response message if the response WAS successful.
        /// Note:
        /// The response is successful if System.Net.Http.HttpResponseMessage.StatusCode was in the range 200-299; otherwise unsuccessful.
        /// </returns>
        Task<IFluentHttpResponse<T>> DeleteAsync<T>();

        /// <summary>
        /// Send a GET request to the specified Uri.
        /// </summary>
        /// <typeparam name="T">Type that defines the content from the HTTP response message.</typeparam>
        /// <returns>
        /// A new instance of FluentHttpResponse class containing:
        ///     HttpStatusCode: the status code of the HTTP response.
        ///     ReasonPhrase: the reason phrase which typically is sent by servers together with the status code.
        ///     Request: the request message which led to this response message.
        /// This instance may also contain:
        ///     ErrorMessage: the content of a HTTP response message if the response WAS NOT successful.
        ///     ResponseBody: the content of a HTTP response message if the response WAS successful.
        /// Note:
        /// The response is successful if System.Net.Http.HttpResponseMessage.StatusCode was in the range 200-299; otherwise unsuccessful.
        /// </returns>
        IFluentHttpResponse<T> Get<T>();

        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">Type that defines the content for the HTTP response message.</typeparam>
        /// <returns>
        /// A task that, when completed, contains a new instance of FluentHttpResponse class containing:
        ///     HttpStatusCode: the status code of the HTTP response.
        ///     ReasonPhrase: the reason phrase which typically is sent by servers together with the status code.
        ///     Request: the request message which led to this response message.
        /// This instance may also contain:
        ///     ErrorMessage: the content of a HTTP response message if the response WAS NOT successful.
        ///     ResponseBody: the content of a HTTP response message if the response WAS successful.
        /// Note:
        /// The response is successful if System.Net.Http.HttpResponseMessage.StatusCode was in the range 200-299; otherwise unsuccessful.
        /// </returns>
        Task<IFluentHttpResponse<T>> GetAsync<T>();

        /// <summary>
        /// Send a POST request as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">Type that defines the content for the HTTP request and response messages.</typeparam>
        /// <param name="entity">The HTTP request content sent to the server.</param>
        /// <returns>
        /// A new instance of FluentHttpResponse class containing:
        ///     HttpStatusCode: the status code of the HTTP response.
        ///     ReasonPhrase: the reason phrase which typically is sent by servers together with the status code.
        ///     Request: the request message which led to this response message.
        /// This instance may also contain:
        ///     ErrorMessage: the content of a HTTP response message if the response WAS NOT successful.
        ///     ResponseBody: the content of a HTTP response message if the response WAS successful.
        /// Note:
        /// The response is successful if System.Net.Http.HttpResponseMessage.StatusCode was in the range 200-299; otherwise unsuccessful.
        /// </returns>
        IFluentHttpResponse<T> Post<T>(T entity);

        /// <summary>
        /// Send a POST request as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">Type that defines the content for the HTTP response message.</typeparam>
        /// <typeparam name="TSend">Type that defines the content for the HTTP request message.</typeparam>
        /// <param name="entity">The HTTP request content sent to the server.</param>
        /// <returns>
        /// A new instance of FluentHttpResponse class containing:
        ///     HttpStatusCode: the status code of the HTTP response.
        ///     ReasonPhrase: the reason phrase which typically is sent by servers together with the status code.
        ///     Request: the request message which led to this response message.
        /// This instance may also contain:
        ///     ErrorMessage: the content of a HTTP response message if the response WAS NOT successful.
        ///     ResponseBody: the content of a HTTP response message if the response WAS successful.
        /// Note:
        /// The response is successful if System.Net.Http.HttpResponseMessage.StatusCode was in the range 200-299; otherwise unsuccessful.
        /// </returns>
        IFluentHttpResponse<TResult> Post<TResult, TSend>(TSend entity);

        /// <summary>
        /// Send a POST request with a cancellation token as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">Type that defines the content for the HTTP response message.</typeparam>
        /// <param name="content">The HTTP request content sent to the server.</param>
        /// <returns>
        /// A task that, when completed, contains a new instance of FluentHttpResponse class containing:
        ///     HttpStatusCode: the status code of the HTTP response.
        ///     ReasonPhrase: the reason phrase which typically is sent by servers together with the status code.
        ///     Request: the request message which led to this response message.
        /// This instance may also contain:
        ///     ErrorMessage: the content of a HTTP response message if the response WAS NOT successful.
        ///     ResponseBody: the content of a HTTP response message if the response WAS successful.
        /// Note:
        /// The response is successful if System.Net.Http.HttpResponseMessage.StatusCode was in the range 200-299; otherwise unsuccessful.
        /// </returns>
        Task<IFluentHttpResponse<T>> PostAsContentAsync<T>(HttpContent content);

        /// <summary>
        /// Send a POST request as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">Type that defines the content for the HTTP request and response messages.</typeparam>
        /// <param name="entity">The HTTP request content sent to the server.</param>
        /// <returns>
        /// A task that, when completed, contains a new instance of FluentHttpResponse class containing:
        ///     HttpStatusCode: the status code of the HTTP response.
        ///     ReasonPhrase: the reason phrase which typically is sent by servers together with the status code.
        ///     Request: the request message which led to this response message.
        /// This instance may also contain:
        ///     ErrorMessage: the content of a HTTP response message if the response WAS NOT successful.
        ///     ResponseBody: the content of a HTTP response message if the response WAS successful.
        /// Note:
        /// The response is successful if System.Net.Http.HttpResponseMessage.StatusCode was in the range 200-299; otherwise unsuccessful.
        /// </returns>
        Task<IFluentHttpResponse<T>> PostAsync<T>(T entity);

        /// <summary>
        /// Send a POST request as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">Type that defines the content for the HTTP response message.</typeparam>
        /// <typeparam name="TSend">Type that defines the content for the HTTP request message.</typeparam>
        /// <param name="entity">The HTTP request content sent to the server.</param>
        /// <returns>
        /// A task that, when completed, contains a new instance of FluentHttpResponse class containing:
        ///     HttpStatusCode: the status code of the HTTP response.
        ///     ReasonPhrase: the reason phrase which typically is sent by servers together with the status code.
        ///     Request: the request message which led to this response message.
        /// This instance may also contain:
        ///     ErrorMessage: the content of a HTTP response message if the response WAS NOT successful.
        ///     ResponseBody: the content of a HTTP response message if the response WAS successful.
        /// Note:
        /// The response is successful if System.Net.Http.HttpResponseMessage.StatusCode was in the range 200-299; otherwise unsuccessful.
        /// </returns>
        Task<IFluentHttpResponse<TResult>> PostAsync<TResult, TSend>(TSend entity);

        /// <summary>
        /// Send a PUT request to the specified Uri.
        /// </summary>
        /// <typeparam name="T">Type that defines the content for the HTTP request and response messages.</typeparam>
        /// <param name="entity">The HTTP request content sent to the server.</param>
        /// <returns>
        /// A new instance of FluentHttpResponse class containing:
        ///     HttpStatusCode: the status code of the HTTP response.
        ///     ReasonPhrase: the reason phrase which typically is sent by servers together with the status code.
        ///     Request: the request message which led to this response message.
        /// This instance may also contain:
        ///     ErrorMessage: the content of a HTTP response message if the response WAS NOT successful.
        ///     ResponseBody: the content of a HTTP response message if the response WAS successful.
        /// Note:
        /// The response is successful if System.Net.Http.HttpResponseMessage.StatusCode was in the range 200-299; otherwise unsuccessful.
        /// </returns>
        IFluentHttpResponse<T> Put<T>(T entity);

        /// <summary>
        /// Send a PUT request to the specified Uri.
        /// </summary>
        /// <typeparam name="TResult">Type that defines the content for the HTTP response message.</typeparam>
        /// <typeparam name="TSend">Type that defines the content for the HTTP request message.</typeparam>
        /// <param name="entity">The HTTP request content sent to the server.</param>
        /// <returns>
        /// A new instance of FluentHttpResponse class containing:
        ///     HttpStatusCode: the status code of the HTTP response.
        ///     ReasonPhrase: the reason phrase which typically is sent by servers together with the status code.
        ///     Request: the request message which led to this response message.
        /// This instance may also contain:
        ///     ErrorMessage: the content of a HTTP response message if the response WAS NOT successful.
        ///     ResponseBody: the content of a HTTP response message if the response WAS successful.
        /// Note:
        /// The response is successful if System.Net.Http.HttpResponseMessage.StatusCode was in the range 200-299; otherwise unsuccessful.
        /// </returns>
        IFluentHttpResponse<TResult> Put<TResult, TSend>(TSend entity);

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">Type that defines the content for the HTTP request and response messages.</typeparam>
        /// <param name="entity">The HTTP request content sent to the server.</param>
        /// <returns>
        /// A task that, when completed, contains a new instance of FluentHttpResponse class containing:
        ///     HttpStatusCode: the status code of the HTTP response.
        ///     ReasonPhrase: the reason phrase which typically is sent by servers together with the status code.
        ///     Request: the request message which led to this response message.
        /// This instance may also contain:
        ///     ErrorMessage: the content of a HTTP response message if the response WAS NOT successful.
        ///     ResponseBody: the content of a HTTP response message if the response WAS successful.
        /// Note:
        /// The response is successful if System.Net.Http.HttpResponseMessage.StatusCode was in the range 200-299; otherwise unsuccessful.
        /// </returns>
        Task<IFluentHttpResponse<T>> PutAsync<T>(T entity);

        /// <summary>
        /// Send a PUT request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="TResult">Type that defines the content for the HTTP response message.</typeparam>
        /// <typeparam name="TSend">Type that defines the content for the HTTP request message.</typeparam>
        /// <param name="entity">The HTTP request content sent to the server.</param>
        /// <returns>
        /// A task that, when completed, contains a new instance of FluentHttpResponse class containing:
        ///     HttpStatusCode: the status code of the HTTP response.
        ///     ReasonPhrase: the reason phrase which typically is sent by servers together with the status code.
        ///     Request: the request message which led to this response message.
        /// This instance may also contain:
        ///     ErrorMessage: the content of a HTTP response message if the response WAS NOT successful.
        ///     ResponseBody: the content of a HTTP response message if the response WAS successful.
        /// Note:
        /// The response is successful if System.Net.Http.HttpResponseMessage.StatusCode was in the range 200-299; otherwise unsuccessful.
        /// </returns>
        Task<IFluentHttpResponse<TResult>> PutAsync<TResult, TSend>(TSend entity);
    }

    /// <summary>
    /// Represents the IFluentHttpResponse. Used for fluent interface construction.
    /// </summary>
    public interface ICanSetPageSize
    {
        /// <summary>
        /// Send a GET request to the specified Uri.
        /// </summary>
        /// <typeparam name="T">Type that defines the content from the HTTP response message.</typeparam>
        /// <returns>
        /// A new instance of FluentHttpPagedResponse class containing:
        ///     HttpStatusCode: the status code of the HTTP response.
        ///     ReasonPhrase: the reason phrase which typically is sent by servers together with the status code.
        ///     Request: the request message which led to this response message.
        /// This instance may also contain:
        ///     ErrorMessage: the content of a HTTP response message if the response WAS NOT successful.
        ///     ResponseBody: the content of a HTTP response message if the response WAS successful.
        ///     Paging: the page info from query with Total amount of posts, Amount of pages, Current page and Amount of posts for the current page.
        /// Note:
        /// The response is successful if System.Net.Http.HttpResponseMessage.StatusCode was in the range 200-299; otherwise unsuccessful.
        /// </returns>
        IFluentHttpPagedResponse<T> GetPaged<T>() where T : IPagedEntity;

        /// <summary>
        /// Send a GET request to the specified Uri as an asynchronous operation.
        /// </summary>
        /// <typeparam name="T">Type that defines the content for the HTTP response message.</typeparam>
        /// <returns>
        /// A task that, when completed, contains a new instance of FluentHttpPagedResponse class containing:
        ///     HttpStatusCode: the status code of the HTTP response.
        ///     ReasonPhrase: the reason phrase which typically is sent by servers together with the status code.
        ///     Request: the request message which led to this response message.
        /// This instance may also contain:
        ///     ErrorMessage: the content of a HTTP response message if the response WAS NOT successful.
        ///     ResponseBody: the content of a HTTP response message if the response WAS successful.
        ///     Paging: the page info from query with Total amount of posts, Amount of pages, Current page and Amount of posts for the current page.
        /// Note:
        /// The response is successful if System.Net.Http.HttpResponseMessage.StatusCode was in the range 200-299; otherwise unsuccessful.
        /// </returns>
        Task<IFluentHttpPagedResponse<T>> GetPagedAsync<T>() where T : IPagedEntity;
    }

    /// <summary>
    /// Represents the IFluentHttpResponse. Used for fluent interface construction.
    /// </summary>
    public interface ICanSetPage
    {
        /// <summary>
        /// Set the page size value for a paged request.
        /// </summary>
        /// <param name="value">Page size value.</param>
        /// <returns>The Fluent Http Client.</returns>
        ICanSetPageSize WithSize(int value);
    }

    /// <summary>
    /// Represents the IFluentHttpClient. Used for fluent interface construction.
    /// </summary>
    public interface IFluentHttpClient : ICanChangeContent, ICanAddConfig, ICanSetPageSize, ICanSetPage { }
}
