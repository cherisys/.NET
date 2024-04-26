# Program.cs
1. Contains application startup code.
2. Configure services required by application.
3. Application request handling pipeline is defined as a series of Middleware Components.

# Dependency Injection
1. When **WebApplicationBuilder** is instantiated, many framework provided services are added by default, i.e. Configuration, Logging etc.
2. You can add additional services in **DI Container** with **WebApplicationBuilder.Services**.
3. Services are typically resolved from DI using Constructor Injection. DI framework then provide an instance of injected service at runtime.

# Middleware
1. Request handling pipeline is composed of a series of middleware components.
2. Each component perform operations on HttpContext and either invokes next middleware or terminates the request.

# Host
1. On Startup, an ASP.NET Core app builds a **host**.
2. **WebApplicationBuilder.Build** method configures a host with a set of default options.
3. Host encapsulates all of app resources, i.e. HTTP Server Implementation (Kestral, IIS etc), Middleware Components, Configuration, Logging, and DI Services.

# Servers
ASP.NET Core App uses an HTTP Server Implementation to listen to HTTP requests.
> ## Windows
> 1. **Kestral:** 
>       - Cross-platform web server, run in a reverse proxy configuration using **IIS**. 
>       - In ASP.NET Core 2.0 or later Kestral can run as public facing edge server exposed directly to Internet.
> 2. **IIS HTTP Server:**
>       - Server for Windows that uses IIS.
>       - With this server, ASP.NET Core app and IIS run in the same process.
> 3. **HTTP.sys:** 
>       - Server for Windows that is not used with IIS.
>>
> ## MacOS/Linux
> 1. **Kestral:**
>       - Cross-platform web server, run in a reverse proxy configuration with **Nginx** or **Apache**.
>       - In ASP.NET Core 2.0 or later Kestral can run as public facing edge server exposed directly to Internet.
>>

# Configuration
1. ASP.NET Core provides Configuration Framework, that gets settings as name-value pairs from an ordered set of configuration providers.
2. ASP.NET Core apps are configured to read from appsettings.json, environment variables, command-line, and more.
3. When app's configuration is loaded, values from environment variables override values from appsettings.json.
4. For managing confidential configuration data such as passwords, .NET Core provides **Secret Manager**.
5. For production secrets, **Azure Key Vault** is recommended.

# Environments
1. Execution environments, such as **Development**, **Staging**, and **Production** are available in ASP.NET Core.
2. Specify the environment as an app is running in by setting the **ASPNETCORE_ENVIRONMENT** variable.
3. ASP.NET read environment variable at application startup and stores the value in **IWebHostEnvironment** implementation.
4. This implementation is available anywhere in app via Dependency Injection.

# Logging
1. ASP.NET Core supports a logging API.
2. It works with a variety of built-in and 3rd party logging providers.
3. Available providers include: **Console**, **Debug**, **Event Tracing on Windows**, **Windows Event Log**, **Trace Source**, **Azure App Service**, **Azure Application Insights**.

# Routing
1. A **route** is a url pattern that is mapped to a handler.
2. Handler is typically, a Razor Page, an Action Method in MVC, or a Middleware.
3. ASP.NET Core routing gives control over the urls used by an app.

# Error Handling
1. Build-in features for Error Handling.
2. Developer Exception Page.
3. Custom Error Page.
4. Static Status Code Pages.
5. Startup Exception Handling.

# Make HTTP Requests
1. An implementation of **IHttpClientFactory** is available for creating **HttpClient** instances.
2. IHttpClientFactory providers:
> - Provides a central location for naming and configuring logical HttpClient instances.
> - Supports registration and chaining of multiple delegating handlers to build an outgoing request middleware pipeline. This pattern is similar to inbound middleware pipeline. Pattern provides a mechanism to manage cross-cutting concerns for HTTP requests, including caching, error handling, serialization, and logging.
> - Integrates with **Polly**, a popular 3rd party library for transient fault handling.
> - Manages the pooling and lifetime of underlying **HttpClientHandler** instances to avoid common DNS problems that occur when managing **HttpClient** lifetimes manually.
> - Adds a configurable logging experience via **ILogger** for all requests sent through clients created by the factory.

# Content Root
It is the base path for:
> - Executable hosting the app (.exe).
> - Compiled assemblies that make up the app (.dll).
> - Content files used by the app, such as:
>   1. Razor files (.cshtml, .razor)
>   2. Configuration files (.xml, .json)
>   3. Data files (.db)
> - The Web Root, typically **wwwroot** folder.

# Web Root
1. It is the base path for public, static resource files, such as: Stylesheets, JavaScript, Images.
2. By default, static files are only served from Web Root directory and from it's sub-directories.
3. Web Root path defaults to, **{Content Root}/wwwroot**.
4. You can specify a different Web Root by setting its path when building the host.
5. In Razor (.cshtml) files, **~/** points to the Web Root.