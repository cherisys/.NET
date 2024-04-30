# Build-in Middlewares

## Authentication
- **Description:** Provides authentication support.
- **Order:** Before *HttpContext.User* is needed. Terminal for Auth Callbacks.

## Authorization
- **Description:** Provides authorization support.
- **Order:** Immediately after authentication middleware.

## CookiePolicy
- **Description:** Tracks consent from users for storing personal information and enforces minimum standards for cookie fields, i.e. *secure*, and *SameSite*.
- **Order:** Before middleware that issues cookies. For example, Authentication, Session, MVC (TempData).

## Cors
- **Description:** Configure cross-origin resource sharing.
- **Order:** Before components that use Cors. *UseCors* currently must be used before *UseResponseCaching* due to a bug.

## DeveloperExceptionPage
- **Description:** Generates a page with error information that is intended for use only in Dev Env.
- **Order:** Before components that generate errors. Usually, first middleware in the pipeline.

## Diagnostics
- **Description:** Several separate middlewares. For example, DeveloperExceptionPage, ExceptionHandler, StatusCodePages, and default view page for new apps.
- **Order:** Before components that generate errors. Terminal for exceptions or serving default web page for new apps.

## ForwardedHeaders
- **Description:** Forwards proxied headers onto the current request.
- **Order:** Before components that consume the updated fields. For example, scheme, host, client IP, method.

## HealthCheck
- **Description:** Checks health of app and its dependencies. For example, database availability.
- **Order:** Terminal if a request matches a health check endpoint.

## HeaderPropagation
- **Description:** Propagates HTTP headers from incoming request to outgoing HTTP Client requests.

## HTTP Logging
- **Description:** Logs HTTP requests and responses.
- **Order:** At the beginning of middleware pipeline.

## HTTP Method Override
- **Description:** Allows an incoming POST request to override the method.
- **Order:** Before components that consumes the updated method.

## HTTPS Redirection
- **Description:** Redirects all requests to HTTPS.
- **Order:** Before components that consume the URL.

## HTTP Strict Transport Security (HSTS)
- **Description:** Security enhancement middleware that adds a special response header.
- **Order:** Before responses are sent and after components that modify requests. For example, ForwardedHeaders, UrlRewriting.

## MVC
- **Description:** Processes requests with MVC/Razor Pages.
- **Order:** Terminal if a request matches a route.

## OWIN
- **Description:** Interop with OWIN-based apps, servers, and middleware.
- **Order:** Terminal if OWIN Middleware fully processes the request.

## OutputCaching
- **Description:** Caching responses based on configuration. Fully controlled by server.
- **Order:** Before components that require caching. *UseRouting* and *UseCors* must come before *UseOutputCaching*. Output caching benefits UI Apps.

## ResponseCaching
- **Description:** Caching responses with the client participation.
- **Order:** Before components that require caching. *UseCors* must come before *UseResponseCaching*. It is typically not beneficial for UI Apps because browsers generally set request headers that prevent caching.

## RequestDecompression
- **Description:** Support for decompressing requests.
- **Order:** Before components that read the request body.

## ResponseCompression
- **Description:** Support for compressing responses.
- **Order:** Before components that require compression.

## RequestLocalization
- **Description:** Support for Localization.
- **Order:** Before localization sensitive components. Must come after *UseRouting* when using **RouteDataRequestCultureProvider**.

## RequestTimeouts
- **Description:** Support for configuring request timeouts, global and per endpoint.
- **Order:** Must come after *UseExceptionHandler*, *UseDeveloperExceptionPage*, and *UseRouting*.

## EndpointRouting
- **Description:** Defines and constrains request routes.
- **Order:** Terminal for matching routes.

## SPA
- **Description:** Handles all requests from this point in the middleware chain by returning the default page for the SPA.
- **Order:** Late in the chain, so that other middleware for serving static files, MVC actions, etc., takes precedence.

## Session
- **Description:** Support for managing user sessions.
- **Order:** Before components that require session.

## StaticFiles
- **Description:** Support for serving static files and directory browsing.
- **Order:** Terminal if a request matches a file.

## UrlRewrite
- **Description:** Support for rewriting URLs and redirecting requests.
- **Order:** Before components that consume the URLs.

## W3CLogging
- **Description:** Generate server access logs in the **W3C Extended Log File Format**.
- **Order:** At the beginning of the middleware pipeline.

## WebSockets
- **Description:** Enables WebSockets protocol.
- **Order:** Before components that are required to accept WebSocket requests.