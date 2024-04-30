# Overview of Middleware
1. Middleware is a software that's assembled into an app pipeline to handle requests and responses. Each component:
    - chooses whether to pass the request to next component in pipeline.
    - can perform work before and after the next component in pipeline.
2. Request delegates are used to build the request pipeline, these handle each HTTP request.
3. Request delegates are configured using **Run**, **Map**, **Use** extension methods.
4. Each middleware component in request pipeline is responsible for **invoking** next component in pipeline or **short-circuiting** the pipeline.
5. When a middleware short-circuits, it's called a **terminal middleware** because it prevents further middleware from processing the request.
6. Exception-handling delegates should be called early in the pipeline, so they can catch exceptions that occur in later stages of the pipeline.
7. Chain multiple request delegates together with **Use**. You can short-circuit pipeline by not calling the next parameter.
8. You can typically perform actions both before and after next delegate.
```c#
app.Use(async (context, next) =>
{
    // Do work that can write to the Response.
    await next.Invoke();
    // Do logging or other work that doesn't write to the Response.
});
```