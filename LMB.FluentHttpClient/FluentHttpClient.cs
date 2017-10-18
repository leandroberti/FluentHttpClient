using LMB.GenericEntityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace LMB.FluentHttpClient
{
    /// <summary>
    /// Fluent API for sending HTTP requests and receiving HTTP responses
    /// from a resource identified by a URI.
    /// </summary>
    public class FluentHttpClient : IDisposable, IFluentHttpClient
    {
        #region Attributes

        private readonly string _requestUri;
        private readonly HttpClient _httpClient;
        private int _currentPage;
        private int _pageSize;

        #endregion

        #region Properties
        #endregion

        #region Constants
        #endregion

        #region Constructors

        /// <summary>
        /// Private constructor, to force object instantiation from the fluent method(s)
        /// </summary>
        /// <param name="serviceBaseUri">HTTP Rest service base Uri.</param>
        /// <param name="requestUri">HTTP Rest service request Uri.</param>
        private FluentHttpClient(string serviceBaseUri, string requestUri)
        {
            _requestUri = requestUri;
            _httpClient = new HttpClient();
            _currentPage = 0;
            _pageSize = 0;

            InitializeClient(_httpClient, serviceBaseUri);
        }

        #endregion

        #region Destructors

        /// <summary>
        /// Destroys the ApiCaller class instance.
        /// </summary>
        ~FluentHttpClient()
        {
            Dispose(false);
        }

        #endregion

        #region Methods

        #region Private

        /// <summary>
        /// The class for sending HTTP requests and receiving HTTP responses
        /// from a resource identified by a URI.
        /// </summary>
        /// <param name="httpClient">The instance of the System.Net.Http.HttpClient class.</param>
        /// <param name="serviceBaseUri">HTTP Rest service base Uri.</param>
        private void InitializeClient(HttpClient httpClient, string serviceBaseUri)
        {
            _httpClient.BaseAddress = new Uri(serviceBaseUri);
            _httpClient.Timeout = new TimeSpan(0, 0, 300);
        }

        /// <summary>
        /// Verifies the HTTP Client configuration and adds the default data if something nedded is missing.
        /// </summary>
        private void Verify()
        {
            if (!_httpClient.DefaultRequestHeaders.Any(h => h.Key.Equals("Accept-Language", StringComparison.InvariantCultureIgnoreCase)))
            {
                var culture = (Thread.CurrentThread.CurrentCulture.Name != null &&
                               !string.IsNullOrEmpty(Thread.CurrentThread.CurrentCulture.Name))
                    ? Thread.CurrentThread.CurrentCulture.Name
                    : "pt-BR";

                _httpClient.DefaultRequestHeaders.Add("Accept-Language", culture);
            }
        }

        /// <summary>
        /// Verifies the HTTP Client configuration and adds the default data if something nedded is missing.
        /// </summary>
        private void VerifyPaged()
        {
            Verify();

            if (_pageSize < 1)
            {
                _pageSize = 1;
            }

            if (_currentPage < 1)
            {
                _currentPage = 1;
            }
        }

        /// <summary>
        /// Returns the default HTTP response entity from provided HttpResponseMessage.
        /// </summary>
        /// <typeparam name="T">Type of response body data from HttpResponseMessage.</typeparam>
        /// <param name="response">The HTTP response message including the status code and data.</param>
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
        private static IFluentHttpResponse<T> GetFluentHttpResponse<T>(HttpResponseMessage response)
        {
            var httpResponse = new FluentHttpResponse<T>
            {
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                Request = response.RequestMessage
            };

            if (!response.IsSuccessStatusCode)
            {
                httpResponse.ErrorMessage = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                httpResponse.ResponseBody = (typeof(T) == typeof(String))
                    ? (T)Convert.ChangeType(response.Content.ReadAsStringAsync().Result, typeof(T))
                    : response.Content.ReadAsAsync<T>().Result;
            }

            return httpResponse;
        }

        /// <summary>
        /// Returns the default HTTP response entity from provided HttpResponseMessage.
        /// </summary>
        /// <typeparam name="T">Type of response body data from HttpResponseMessage.</typeparam>
        /// <param name="response">The HTTP response message including the status code and data.</param>
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
        private async static Task<IFluentHttpResponse<T>> GetFluentHttpResponseAsync<T>(HttpResponseMessage response)
        {
            var httpResponse = new FluentHttpResponse<T>
            {
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                Request = response.RequestMessage
            };

            if (!response.IsSuccessStatusCode)
            {
                httpResponse.ErrorMessage = await response.Content.ReadAsStringAsync();
            }
            else
            {
                httpResponse.ResponseBody = (typeof(T) == typeof(String))
                    ? (T)Convert.ChangeType(await response.Content.ReadAsStringAsync(), typeof(T))
                    : await response.Content.ReadAsAsync<T>();
            }

            return httpResponse;
        }

        /// <summary>
        /// Returns the default HTTP response entity from provided HttpResponseMessage.
        /// </summary>
        /// <typeparam name="T">Type of response body data from HttpResponseMessage.</typeparam>
        /// <param name="response">The HTTP response message including the status code and data.</param>
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
        private IFluentHttpPagedResponse<T> GetFluentHttpPagedResponse<T>(HttpResponseMessage response) where T : IPagedEntity
        {
            var httpResponse = new FluentHttpPagedResponse<T>
            {
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                Request = response.RequestMessage
            };

            if (!response.IsSuccessStatusCode)
            {
                httpResponse.ErrorMessage = response.Content.ReadAsStringAsync().Result;
            }
            else
            {
                httpResponse.ResponseBody = (typeof(T) == typeof(String))
                    ? (T)Convert.ChangeType(response.Content.ReadAsStringAsync().Result, typeof(T))
                    : response.Content.ReadAsAsync<T>().Result;

                var totalCount = typeof(T).GetInterfaces()
                    .Any(i => i.IsGenericType &&
                              (i.GetGenericTypeDefinition() == typeof(ICollection<>) ||
                               i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                    ? ((IEnumerable<T>)httpResponse.ResponseBody).FirstOrDefault()?.TotalCount ?? 0
                    : httpResponse.ResponseBody.TotalCount ?? 0;

                httpResponse.SetPagedData(totalCount, _currentPage, _pageSize);
            }

            return httpResponse;
        }

        /// <summary>
        /// Returns the default HTTP response entity from provided HttpResponseMessage.
        /// </summary>
        /// <typeparam name="T">Type of response body data from HttpResponseMessage.</typeparam>
        /// <param name="response">The HTTP response message including the status code and data.</param>
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
        private async Task<IFluentHttpPagedResponse<T>> GetFluentHttpPagedResponseAsync<T>(HttpResponseMessage response) where T : IPagedEntity
        {
            var httpResponse = new FluentHttpPagedResponse<T>
            {
                StatusCode = response.StatusCode,
                ReasonPhrase = response.ReasonPhrase,
                Request = response.RequestMessage
            };

            if (!response.IsSuccessStatusCode)
            {
                httpResponse.ErrorMessage = await response.Content.ReadAsStringAsync();
            }
            else
            {
                httpResponse.ResponseBody = (typeof(T) == typeof(String))
                    ? (T)Convert.ChangeType(await response.Content.ReadAsStringAsync(), typeof(T))
                    : await response.Content.ReadAsAsync<T>();

                var totalCount = typeof(T).GetInterfaces()
                    .Any(i => i.IsGenericType &&
                              (i.GetGenericTypeDefinition() == typeof(ICollection<>) ||
                               i.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
                    ? ((IEnumerable<T>)httpResponse.ResponseBody).FirstOrDefault()?.TotalCount ?? 0
                    : httpResponse.ResponseBody.TotalCount ?? 0;

                httpResponse.SetPagedData(totalCount, _currentPage, _pageSize);
            }

            return httpResponse;
        }

        #endregion

        #region Public

        /// <summary>
        /// Releases allocated resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Create a new instance of the Fluent Http Client.
        /// </summary>
        /// <param name="serviceBaseUri">HTTP Rest service base Uri.</param>
        /// <param name="requestUri">HTTP Rest service request Uri.</param>
        public static ICanChangeContent FromServiceApi(string serviceBaseUri, string requestUri)
        {
            return new FluentHttpClient(serviceBaseUri, requestUri);
        }

        /// <summary>
        /// Set the HTTP Accept Header for application/xml content.
        /// </summary>
        /// <returns>The Fluent Http Client.</returns>
        public ICanAddConfig WithXmlContent()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

            return this;
        }

        /// <summary>
        /// Set the HTTP Accept Header for application/json content.
        /// </summary>
        /// <returns>The Fluent Http Client.</returns>
        public ICanAddConfig WithJsonContent()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return this;
        }

        /// <summary>
        /// Set the HTTP Authorization header with a Bearer token value.
        /// </summary>
        /// <param name="token">The token string.</param>
        /// <returns>The Fluent Http Client.</returns>
        public ICanAddConfig WithAuthorization(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            return this;
        }

        /// <summary>
        /// Adds a new header to the HTTP client.
        /// </summary>
        /// <param name="name">Header name.</param>
        /// <param name="value">Header value.</param>
        /// <returns>The Fluent Http Client.</returns>
        public ICanAddConfig AddHeader(string name, string value)
        {
            _httpClient.DefaultRequestHeaders.Add(name, value);
            return this;
        }

        /// <summary>
        /// Adds a new header to the HTTP client.
        /// </summary>
        /// <param name="name">Header name.</param>
        /// <param name="values">Header values collection.</param>
        /// <returns>The Fluent Http Client.</returns>
        public ICanAddConfig AddHeader(string name, IEnumerable<string> values)
        {
            _httpClient.DefaultRequestHeaders.Add(name, values);
            return this;
        }

        /// <summary>
        /// Set the new client timeout.
        /// Repleces the default 300 seconds timeoute value.
        /// </summary>
        /// <param name="hours">Timeout hour value.</param>
        /// <param name="minutes">Timeout minuts value.</param>
        /// <param name="seconds">Timeout seconds value.</param>
        /// <returns>The Fluent Http Client.</returns>
        public ICanAddConfig SetTimeout(int hours, int minutes, int seconds)
        {
            _httpClient.Timeout = new TimeSpan(hours, minutes, seconds);
            return this;
        }

        /// <summary>
        /// Set the new client timeout.
        /// Repleces the default 300 seconds timeoute value.
        /// </summary>
        /// <param name="seconds">Timeout seconds value.</param>
        /// <returns>The Fluent Http Client.</returns>
        public ICanAddConfig SetTimeout(int seconds)
        {
            _httpClient.Timeout = new TimeSpan(0, 0, seconds);
            return this;
        }

        /// <summary>
        /// Set the current page value for a paged request.
        /// </summary>
        /// <param name="value">Current page value.</param>
        /// <returns>The Fluent Http Client.</returns>
        public ICanSetPage FromPage(int value)
        {
            _currentPage = value;
            return this;
        }

        /// <summary>
        /// Set the page size value for a paged request.
        /// </summary>
        /// <param name="value">Page size value.</param>
        /// <returns>The Fluent Http Client.</returns>
        public ICanSetPageSize WithSize(int value)
        {
            _pageSize = value;
            return this;
        }

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
        public IFluentHttpResponse<T> Get<T>()
        {
            Verify();
            var response = _httpClient.GetAsync(_requestUri).Result;

            return GetFluentHttpResponse<T>(response);
        }

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
        public IFluentHttpPagedResponse<T> GetPaged<T>() where T : IPagedEntity
        {
            VerifyPaged();
            var response = _httpClient.GetAsync(_requestUri).Result;

            return GetFluentHttpPagedResponse<T>(response);
        }

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
        public IFluentHttpResponse<TResult> Post<TResult, TSend>(TSend entity)
        {
            Verify();
            var response = _httpClient.PostAsJsonAsync(_requestUri, entity).Result;

            return GetFluentHttpResponse<TResult>(response);
        }

        /// <summary>
        /// Send a POST request to the specified Uri.
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
        public IFluentHttpResponse<T> Post<T>(T entity)
        {
            Verify();
            var response = _httpClient.PostAsJsonAsync(_requestUri, entity).Result;

            return GetFluentHttpResponse<T>(response);
        }

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
        public IFluentHttpResponse<TResult> Put<TResult, TSend>(TSend entity)
        {
            Verify();
            var response = _httpClient.PutAsJsonAsync(_requestUri, entity).Result;

            return GetFluentHttpResponse<TResult>(response);
        }

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
        public IFluentHttpResponse<T> Put<T>(T entity)
        {
            Verify();
            var response = _httpClient.PutAsJsonAsync(_requestUri, entity).Result;

            return GetFluentHttpResponse<T>(response);
        }

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
        public IFluentHttpResponse<T> Delete<T>()
        {
            Verify();
            var response = _httpClient.DeleteAsync(_requestUri).Result;

            return GetFluentHttpResponse<T>(response);
        }

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
        public async Task<IFluentHttpResponse<T>> GetAsync<T>()
        {
            Verify();

            var response = await _httpClient.GetAsync(_requestUri);

            return await GetFluentHttpResponseAsync<T>(response);
        }

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
        public async Task<IFluentHttpPagedResponse<T>> GetPagedAsync<T>() where T : IPagedEntity
        {
            VerifyPaged();
            var response = await _httpClient.GetAsync(_requestUri);

            return await GetFluentHttpPagedResponseAsync<T>(response);
        }

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
        public async Task<IFluentHttpResponse<TResult>> PostAsync<TResult, TSend>(TSend entity)
        {
            Verify();
            var response = await _httpClient.PostAsJsonAsync(_requestUri, entity);

            return await GetFluentHttpResponseAsync<TResult>(response);
        }

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
        public async Task<IFluentHttpResponse<T>> PostAsync<T>(T entity)
        {
            Verify();
            var response = await _httpClient.PostAsJsonAsync(_requestUri, entity);

            return await GetFluentHttpResponseAsync<T>(response);
        }

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
        public async Task<IFluentHttpResponse<T>> PostAsContentAsync<T>(HttpContent content)
        {
            Verify();
            var response = await _httpClient.PostAsync(_requestUri, content);

            return await GetFluentHttpResponseAsync<T>(response);
        }

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
        public async Task<IFluentHttpResponse<TResult>> PutAsync<TResult, TSend>(TSend entity)
        {
            Verify();
            var response = await _httpClient.PutAsJsonAsync(_requestUri, entity);

            return await GetFluentHttpResponseAsync<TResult>(response);
        }

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
        public async Task<IFluentHttpResponse<T>> PutAsync<T>(T entity)
        {
            Verify();
            var response = await _httpClient.PutAsJsonAsync(_requestUri, entity);

            return await GetFluentHttpResponseAsync<T>(response);
        }

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
        public async Task<IFluentHttpResponse<T>> DeleteAsync<T>()
        {
            Verify();
            var response = await _httpClient.DeleteAsync(_requestUri);

            return await GetFluentHttpResponseAsync<T>(response);
        }

        #endregion

        #region Protected

        /// <summary>
        /// Free all allocated resources.
        /// </summary>
        /// <param name="disposing">Indicates whether manageable resources will be released or not.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free all Managed Resources
                _httpClient?.Dispose();
            }

            // Free all Native Resources
        }

        #endregion

        #endregion

        #region Events
        #endregion
    }
}
