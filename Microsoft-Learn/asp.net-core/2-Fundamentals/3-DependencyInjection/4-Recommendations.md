# Recommendations
1. **async/await** and **Task** based service resolution is not supported.
2. Avoid storing data and configuration directly in the service container. Configuration should use **Option Pattern**.
3. Avoid **data holder** objects that only exist to allow access to another object. It's better to request the actual item via DI.
4. Avoid static access to services. For example, avoid capturing **IApplicationBuilder.ApplicationServices** as a static field or property for use elsewhere.
5. Keep **DI Factories** fast and synchronous. Async DI Factories can cause **deadlock**.
6. Avoid using **Service Locator Pattern**.
    - Don't invoke **GetService** to obtain a service instance when you can use DI instead.
    - Avoid injecting a factory that resolves dependencies at run time.
    - Both of these practices mix IoC strategies.
7. Avoid calls to **BuildServiceProvider** when configuring services. Instead use overload that includes **IServiceProvider**.
8. Disposable **Transient Services** are captured by container for disposal. This can turn into **memory leak** if resolved from the top-level container.
9. Enable **scope validation** to make sure the app doesn't have singletons that capture scoped services.
10. Avoid static access to HttpContext (e.g. IHttpContextAccessor.HttpContext).