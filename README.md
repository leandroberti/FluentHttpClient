# Fluent Http Client

[![Donate](https://img.shields.io/badge/Donate-PayPal.Me-green.svg)](http://paypal.me/leandroberti)

This is an HTTP client that can be used to consume HTTP APIs and was developed using the [Fluent Interface](https://martinfowler.com/bliki/FluentInterface.html) design pattern.

First, by using this pattern we make the code more readable, and easy to understand.
This makes it easier to make changes later on.

Second, the Fluent Interface can be used to force the programmer to perform certain steps before they perform others.
An object can have a method that uses data it expects to have been set in another method.

## How it works?

Using the power of interfaces, we can enforce “rules of grammar”, and make it that certain methods cannot be called, until all the required setup methods have been called.

Whe achieve that, by following these three steps to create the fluent interface:

1. Define all possible combinations of the natural language syntax.
2. Create the interfaces that enforce the grammar rules.
3. Build the class, implementing the interfaces.

Other important thing to consider here is that, when we are defining the syntax, we have different types of methods:

**Initiating/Instantiating**

_These are the methods we use to start the call._

Here, we only have two:

```C#
WithJsonContent()
WithXmlContent()
```

**Chaining/Continuing**

_These are the methods we call in the middle of the statement, and that let us call another method after them._

Here, we have a little bit more:

```C#
AddHeader(string name, IEnumerable<string> values)
AddHeader(string name, string value)
SetTimeout(int seconds)
SetTimeout(int hours, int minutes, int seconds)
WithAuthorization(string token)
FromPage(int value)
```
When any of these methods are called, you can keep on calling other methods. That’s known as _“method chaining”_.

**Executing/Ending**

_These are the methods that finally do some action, and end our statement._

Here, we have:

```C#
Delete<T>()
DeleteAsync<T>()
Get<T>()
GetAsync<T>()
Post<T>(T entity)
Post<TResult, TSend>(TSend entity)
PostAsContentAsync<T>(HttpContent content)
PostAsync<T>(T entity)
PostAsync<TResult, TSend>(TSend entity)
Put<T>(T entity)
Put<TResult, TSend>(TSend entity)
PutAsync<T>(T entity)
PutAsync<TResult, TSend>(TSend entity)
GetPaged<T>()
GetPagedAsync<T>()
```

## How to use this extension?

To use this extension we need:

1. Intantiate the client.
2. Configure the client.
3. Call the HTTP Service.

Let's see how to do this.

#### Instantiating the client

First, we need to define the namespace (`using LMB.FluentHttpClient`) and then use the static method `FromServiceApi` to instanciate the client.

The method `FromServiceApi` receives two parameters `serviceBaseUri` and `requestUri`:

* the `serviceBaseUri` parameter corresponds to the HTTP service base Uri.
* the `requestUri` parameter corresponds to the HTTP service request Uri.

```C#
FluentHttpClient
    .FromServiceApi("http://petstore.swagger.io", "v2/pet/findByStatus?status=available")
```

In this example we are calling an HTTP service at URL `http://petstore.swagger.io/v2/pet/findByStatus?status=available` where the `http://petstore.swagger.io` is the service domain used at `serviceBaseUri` parameter and the `v2/pet/findByStatus?status=available` is the API method used at `requestUri` parameter.


#### Configuring the content-type

After the method `FromServiceApi`, as we can see by the intellisense, only two options are available:

![alt text](https://raw.githubusercontent.com/leandroberti/FluentHttpClient/master/Images//Intellisense01.png "Intellisense after FromServiceApi method")

The `WithJsonContent()` method sets the `content-type` to `application/json` and the `WithXmlContent()` method sets the `content-type` to `application/xml`.

```C#
FluentHttpClient
    .FromServiceApi("http://petstore.swagger.io", "v2/pet/findByStatus?status=available")
    .WithJsonContent()
```

#### Configuring the client

After the method `WithJsonContent()`, we have a lot of options.

As _chaining methods_ we have:

* `AddHeader(string name, IEnumerable<string> values)` and 
  `AddHeader(string name, string value)` used to add a new header to the HTTP client.
* `SetTimeout(int seconds)` and 
  `SetTimeout(int hours, int minutes, int seconds)` use to set the timespan to wait before the request times out.
* `WithAuthorization(string token)` use to set the HTTP Authorization header with a Bearer token value.

And finally the chaining methods...

* `FromPage(int value)` used to set the current page value for a paged request.
* `WithSize(int value)` used to set the page size value for a paged request.
 
These two methods are separated from the others because it takes us to another _chaining_ or _executing_ methods than those mentioned above.

Intellisense for all methods allowed after we use the methods that are mentioned in the _"Configuring the content-type"_:

![alt text](https://raw.githubusercontent.com/leandroberti/FluentHttpClient/master/Images/Intellisense02.png "Intellisense after WithJsonContent method")

Intellisense for all methods allowed after we use the _chaining_ method `FromPage`:

![alt text](https://raw.githubusercontent.com/leandroberti/FluentHttpClient/master/Images/Intellisense03.png "Intellisense after FromPage method")

#### Calling the HTTP service

And now that we have everything configured using the _chaining methods_ the only thing we should do is call the service.

To do this, just use one of the _executing methods_:

* `Delete<T>()` or 
  `DeleteAsync<T>()` used to send a DELETE request to the specified Uri.
* `Get<T>()` or 
  `GetAsync<T>()` used to send a GET request to the specified Uri.
* `Post<T>(T entity)` or 
  `Post<TResult, TSend>(TSend entity)` or 
  `PostAsContentAsync<T>(HttpContent content)` or 
  `PostAsync<T>(T entity)` or 
  `PostAsync<TResult, TSend>(TSend entity)` used to send a POST request to the specified Uri.
* `Put<T>(T entity)` or 
  `Put<TResult, TSend>(TSend entity)` or 
  `PutAsync<T>(T entity)` or 
  `PutAsync<TResult, TSend>(TSend entity)` used to send a PUT request to the specified Uri.

These methods mentioned above will return an instance of the `FluentHttpResponse<T>` class that implements the `IFluentHttpResponse<T>` interface:

```C#
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
```

But, for the _chaining methods_ `FromPage(int value)` and `WithSize(int value)` we have the _executing methods_ `GetPaged<T>()` or `GetPagedAsync<T>()`.
These methods are used to send a GET request to the specified Uri and will return an instance of the `FluentHttpPagedResponse<T>` class that implements the `IFluentHttpPagedResponse<T>` and `IFluentHttpResponse<T>` interfaces:

```C#
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

/// <summary>
/// Represents an query that can be paged.
/// </summary>
public interface IPaged
{
    /// <summary>
    /// Total amount of posts for the query.
    /// </summary>
    int TotalCount { get; }

    /// <summary>
    /// Amount of pages that exist for the given Page and PageSize combination.
    /// </summary>
    int PageCount { get; }

    /// <summary>
    /// Current page.
    /// </summary>
    int Page { get; }

    /// <summary>
    /// Amount of posts for the current page.
    /// </summary>
    int PageSize { get; }
}
```

**IMPORTANT NOTE:** All the **`Async`** methods send a request to the specified Uri as an asynchronous operation so we have to use the [`async` and `await`](https://docs.microsoft.com/en-us/dotnet/csharp/async).

For the **Paged** _executing methods_ the content of the HTTP response message that will be returned into the `ResponseBody` property **must** implement the `LMB.GenericEntityBase.IPagedEntity` interface that can be found at [Nuget package LMB.GenericEntityBase](https://www.nuget.org/packages/LMB.GenericEntityBase/) or [GenericEntityBase GitHub repository](https://github.com/leandroberti/GenericEntityBase).

## Where to get this extension?

You can install this extension direct from:

[![Nuget](https://img.shields.io/badge/nuget-v1.0.1-blue.svg)](https://www.nuget.org/packages/LMB.FluentHttpClient/)

Or instal with Package Manager Console

```C#
Install-Package LMB.FluentHttpClient
```

Or you can download this github project and copy all the `.cs` files direct into your project.

# Donations

**If you enjoy this work, please consider supporting me for developing and maintaining this (and others) templates.**

[![paypal](https://www.paypalobjects.com/en_US/i/btn/btn_donateCC_LG.gif)](https://www.paypal.com/cgi-bin/webscr?cmd=_donations&business=26TY9QLTDWDSE&lc=US&item_name=leandroberti&item_number=github&currency_code=USD&bn=PP%2dDonationsBF%3abtn_donate_SM%2egif%3aNonHosted)
[![Donate](https://img.shields.io/badge/Donate-PayPal.Me-green.svg)](http://paypal.me/leandroberti)
