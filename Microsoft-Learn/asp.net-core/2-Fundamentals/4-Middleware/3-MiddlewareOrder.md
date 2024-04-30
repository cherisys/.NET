# Middleware Order
> ExceptionHandler -> ForwardedHeaders -> Hsts -> HttpsRedirection -> StaticFiles -> CookiePolicy -> Routing -> RateLimiter -> RequestLocalization -> Cors -> Authentication -> Authorization -> Session -> ResponseCompression -> ResponseCaching -> CustomMiddleware1 -> CustomMiddleware2 -> ... -> CustomMiddlewareN -> Endpoint

- **Endpoint** middleware executes **Filter Pipeline** for corresponding app type - MVC or Razor Pages.
- If **Routing** middleware is not called explicitly, it runs at the beginning of the pipeline by default.
- The order in which Middleware components are added in **Program.cs** file defines the order in which Middleware components are invoked on requests and reverse order on responses.
- Middleware order is critical for *security*, *performance*, and *functionality*.

# Scenario Based Ordering

## ResponseCaching, ResponseCompression
```c#
app.UseResponseCaching();
app.UseResponseCompression();
```
CPU usage could be reduced by caching the compressed response, but you might end up caching multiple representations of a resource using different compression algorithms - Gzip or Brotli.

## Routing, RateLimiter
- Routing must be called before RateLimiter, when rate-limiting endpoint specific APIs are used.
- When calling only global limiters, RateLimiter can be called before Routing.

## StaticFiles, ResponseCaching, ResponseCompression
```c#
app.UseResponseCaching();
app.UseResponseCompression();
app.UseStaticFiles();
```
Above ordering allow caching compressed static files.

## StaticFiles, Cors
- Typically, StaticFiles is called before Cors.
- For apps that use JavaScript to retrieve static files cross-site, Cors must be called before StaticFiles.