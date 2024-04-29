# Overview of Dependency Injection (DI)
1. A **dependency** is an object that another object depends on.
2. ASP.NET Core supports DI design pattern which is a technique for achieving **Inversion of Control (IoC)** between classes and their dependencies.

# Problems with Directly Instantiating Dependencies
```c#
public class IndexModel : PageModel
{
    private readonly MyDependency _dependency = new MyDependency();

    public void OnGet()
    {
        _dependency.WriteMessage("IndexModel.OnGet");
    }
}
```
1. To repalce **MyDependency** with a different implementation, **IndexModel** class need to be modified.
2. If **MyDependency** has dependencies, they must also be configured by **IndexModel** class.
3. In large project with multiple classes depending on **MyDependency**, configuration code becomes scattered across the app.
4. This implementation is difficult to Unit Test.

# Problems addressed by Dependency Injection
1. Use of an Interface or Base class to abstract the dependency implementation.
2. Registration of Dependency in Service Container. ASP.NET Core provides built-in service container, **IServiceProvider**.
3. Services are typically registered in the app's **Program.cs** file.
4. **Injection** of the service into the constructor of the class where it's used.
5. Framework takes on the responsibility of creating an instance of the dependency and disposing it when no more needed.
6. It's not unusual to use DI in chained fashion. Each requested dependency in turn requests its own dependencies. Container resolves the dependencies in the graph and returns the fully resolved service.
7. The collective set of dependencies that must be resolved is typically referred to as **dependency tree**, **dependency graph**, or **object graph**.

# Register Groups of Services using Extension Method
```c#
public static class MyServiceCollectionExtension
{
    public static IServiceCollection AddMyDependencyGroup(
            this IServiceCollection services)
    {
        services.AddScoped<IMyDependency, MyDependency>();
        services.AddScoped<IMyDependency2, MyDependency2>();
        return services;
    }
}

// In Program.cs
builder.Services.AddMyDependencyGroup();
```

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
# Constructor Injection Behavior
1. Services can be resolved by using: **IServiceProvider**, **ActivatorUtilities**.
2. **ActivatorUtilities** creates objects that aren't registered in the container. 
3. Constructors can accept arguments that aren't provided by dependency injection, but the arguments must assign default values.
4. When services are resolved, constructor injection requires a **public constructor**.
5. When services are resolved by **ActivatorUtilities**, constructor injection requires that only one applicable constructor exists.
6. Constructor overloads supported, but only one overload can exist whose arguments can all be fulfilled by DI.

# Entity Framework Contexts
1. By default, EF Context added to service container with **Scoped Lifetime**, because database ops are normally scoped to client request.
2. To use different lifetime, specify lifetime using an AddDbContext overload.
3. Services of a given lifetime shouldn't use a database context with a lifetime that's shorter than the services lifetime.

# Resolve a Service at App Startup
```c#
var app = builder.Build();

using (var serviceScope = app.Services.CreateScope())
{
    var services = serviceScope.ServiceProvider;
    var myDependency = services.GetRequiredService<IMyDependency>();
    myDependency.WriteMessage("Call services from main");
}
```

# Scope Validation


