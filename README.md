# Fluent Http Client

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

#### Instantiating the client

First, we need to define the namespace (`using LMB.FluentHttpClient`) and then use the static method `FromServiceApi` to instanciate the client.

The method `FromServiceApi` receives two parameters `serviceBaseUri` and `requestUri`:

* the `serviceBaseUri` parameter corresponds to the HTTP service base Uri.
* the `requestUri` parameter corresponds to the HTTP service request Uri.

```C#
FluentHttpClient
    .FromServiceApi("http://MyService", "api/GetApplicationUser")
```

In this example we are calling an HTTP service at URL `http://MyService/api/GetApplicationUser` where the `http://MyService` is the service domain used at `serviceBaseUri` parameter and the `api/GetApplicationUser` is the API method used at `requestUri` parameter.


#### Configuring the content-type

After the method `FromServiceApi`, as we can see by the intellisense, only two options are available:

![alt text](https://raw.githubusercontent.com/leandroberti/FluentHttpClient/master/Images//Intellisense01.png "Intellisense after FromServiceApi method")

The `WithJsonContent()` method sets the `content-type` to `application/json` and the `WithXmlContent()` method sets the `content-type` to `application/xml`.

```C#
FluentHttpClient
    .FromServiceApi("http://MyService", "api/GetApplicationUser")
    .WithJsonContent()
```

#### Configuring the client

After the method `WithJsonContent()`, we have a lot of options.

As _chaining methods_ we have:

* `AddHeader(string name, IEnumerable<string> values)` and `AddHeader(string name, string value)` used to add a new header to the HTTP client.
* `SetTimeout(int seconds)` and `SetTimeout(int hours, int minutes, int seconds)` use to set the timespan to wait before the request times out.
* `WithAuthorization(string token)` use to set the HTTP Authorization header with a Bearer token value.

And finally the chaining methods

* `FromPage(int value)` used to set the current page value for a paged request.
* `WithSize(int value)`used to set the page size value for a paged request.
 
These two methods are separated from the others because it takes us to another _chaining_ or _executing_ methods than those mentioned above.

Intellisense for all methods allowed after we use the methods that are mentioned in the _"Configuring the content-type"_:

![alt text](https://raw.githubusercontent.com/leandroberti/FluentHttpClient/master/Images/Intellisense02.png "Intellisense after WithJsonContent method")

Intellisense for all methods allowed after we use the _chaining_ method `FromPage`:

![alt text](https://raw.githubusercontent.com/leandroberti/FluentHttpClient/master/Images/Intellisense03.png "Intellisense after FromPage method")

## Where to get this extension?

You can install this extension direct from Nuget:

```C#
Install-Package LMB.FluentHttpClient
```

Or you can download this github project and copy all the `.cs` files direct into your project.