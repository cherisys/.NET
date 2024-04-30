# Service Lifetimes
1. **Singleton:** 
> - Singleton lifetime services are created at the time when they are requested first time.
> - Every subsequent request of the service implemention from DI container uses same instance.
> - Singleton service must be thread-safe and are often used in stateless services.
> - Singleton services are disposed when the **ServiceProvider** is disposed on application shutdown.
2. **Scoped:**
> - Service is created once per client request (connection).
> - Scoped services are disposed at the end of the request.
3. **Transient:**
> - Service is constructed and resolved each time it is requested from Service Container.
> - Transient services are disposed at the end of the request.

- By default, in Dev Env, resolving a service from another service with a longer lifetime throws an exception.
- Singleton > Scoped > Transient
- Resolve a singleton service from a scoped or transient service.
- Resolve a scoped service from another scoped or transient service.

# To use Scoped Service in Middleware, use One of these approaches
1. Inject service into the Middleware's **Invoke** or **InvokeAsync** method.
**Note:** Using **Constructor Injection** throws a runtime exception because it forces the scoped service to behave like a singleton.
2. Use **Factory-based Middleware**. Middleware registered using this method is activated per-client request (connection), which allows scoped services to be injected into the middleware's constructor.

# Service Registration Methods
| Method                                               | Automatic Object Disposal | Multiple Implementations | Pass Arguments |
|------------------------------------------------------|---------------------------|--------------------------|----------------|
| Add{Lifetime}<{Service},{Implementation}>()          | Yes                       | Yes                      | No             |
| Add{Lifetime}<{Service}>(sp => new {Implementation}) | Yes                       | Yes                      | Yes            |
| Add{Lifetime}<{Implementation}>()                    | Yes                       | No                       | No             |
| AddSingleton<{Service}>(new {Implementation})        | No                        | Yes                      | Yes            |
| AddSingleton(new {Implementation})                   | No                        | No                       | Yes            |

1. Above service registration methods can be used to register multiple instances of same service type.
2. Services appear in the order they were registered when resolved via **IEnumerable<{Service}>**.
3. Framework provide **TryAdd{Lifetime}** extension methods, which register service only if there isn't already an implementation registered.

# Keyed Services
1. Mechanism for registering and retrieving DI Services using keys.
2. Service is associated with a key by calling **AddKeyed{Lifetime}** to register it.
3. Access a registered service by specifying the key with the **[FromKeyedServices]** attribute.

```c#
builder.Services.AddKeyedSingleton<ICache, BigCache>("big");
builder.Services.AddKeyedSingleton<ICache, SmallCache>("small");

app.MapGet("/big", ([FromKeyedServices("big")] ICache bigCache) => bigCache.Get("date"));
app.MapGet("/small", ([FromKeyedServices("small")] ICache smallCache) => smallCache.Get("date"));
```