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
1. **CreateDefaultBuiler** sets **ServiceProviderOptions.ValidateScopes** to **true** if the app's env is development.
2. When ValidateScopes is set to true, default Service Provider perform checks to verify that:
    - Scoped services are not directly or indirectly resolved from the Root Service Provider.
    - Scoped services are not directly or indirectly injected into singletons.
3. Root Service Provider is created when **BuildServiceProvider** is called.
4. Root Service Provider's lifetime corresponds to app/server's lifetime.
5. Scoped services are disposed by the container that created them.
6. If a scoped service is created in the root container, the service's lifetime is effectively promoted to singleton.
7. Validating service scopes caches these situations when **BuildServiceProvider** is called.
8. To always Validate Scopes, including in Prod Env, configure **ServiceProviderOptions** with **UseDefaultServiceProvider** on the host builder:
```c#
WebHost.CreateDefaultBuilder(args)
    .UseDefaultServiceProvider((context, options) => {
        options.ValidateScopes = true;
    })
```

# Request Services
1. Services and their dependencies are exposed through **HttpContext.RequestServices**.
2. Framework creates a scope per request, and **RequestServices** exposes the scoped service provider.
3. All scoped services are valid for as long as the request is active.

# Design Services for DI
1. When designing services for DI:
    - Avoid stateful, static class and members.
    - Avoid creating global state by designing apps to use singleton services instead.
    - Avoid direct instantiation of dependent classes within services.
    - Make services small, well-factored, and easily tested.
2. Keep in mind that **Razor Pages Page Model Classes** and **MVC Controller Classes** should focus only on UI concerns.

# Disposal of Services
1. Service Container automatically calls **Dispose** for **IDisposable** types it creates.
2. Services resolved from the container should never be disposed by developer.
3. For the services not created by Service Container, developer is responsible for disposing services.
```c#
// Disposed by Service Container
builder.Services.AddScoped<Service1>();
builder.Services.AddSingleton<Service2>();

// Developer is responsible to dispose
builder.Services.AddSingleton(new Service1());
builder.Services.AddSingleton(new Service2());
```

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

# Anti-Patterns
## Disposable transient services captured by container can cause **Memory Leaks**.
```c#
static void TransientDisposablesWithoutDispose()
{
    var services = new ServiceCollection();
    services.AddTransient<ExampleDisposable>();
    ServiceProvider serviceProvider = services.BuildServiceProvider();

    for(int i=0; i<1000; ++i)
    {
        _ = serviceProvider.GetRequiredService<ExampleDisposable>();
    }

    serviceProvider.Dispose();
}
```
In preceding anti-pattern, 1000 **ExampleDisposable** objects are instantiated and rooted. They will not be disposed of until **ServiceProvider (Container) Instance** is disposed.

## Async DI Factories can cause **Deadlocks**.
- Term **DI Factories** refers to the overload methods that exist when calling Add{Lifetime}.
- There are overloads accepting a **Func<IServiceProvider, T>** where T is service being registered, and parameter is named **implementationFactory**.
- The **implementationFactory** can be provided as lambda expression, local function, or method.
- If the factory is asynchronous and you use **Task<TResult>.Result**, this will cause a deadlock.
```c#
static void DeadlockWithAsyncFactory()
{
    var services = new ServiceCollection();
    services.AddSingleton<Foo>(implementationFactory: provider => {
        Bar bar = GetBarAsync(provider).Result;
        return new Foo(bar);
    });

    services.AddSingleton<Bar>();
    
    using ServiceProvider serviceProvider = services.BuildServiceProvider();
    _ = serviceProvider.GetRequiredService<Foo>();
}

static async Task<Bar> GetBarAsync(IServiceProvider serviceProvider){
    await Task.Delay(1000);
    return serviceProvider.GetRequiredService<Bar>();
}
```
When you're running this anti-pattern and deadlock occurs, you can view two threads waiting from Visual Studio's Parallel Stack window.

## Captive Dependency
- Refers to the misconfiguration of Service Lifetimes, where a longer-lived service hold a shorter-lived service captive.
```c#
static void CaptiveDependency()
{
    var services = new ServiceCollection();
    services.AddSingleton<Foo>();
    services.AddScoped<Bar>();
    
    using ServiceProvider serviceProvider = services.BuildServiceProvider();

    //Eanble scope validation
    //using ServiceProvider serviceProvider = services.BuildServiceProvider(validateScopes: true); 
    _ = serviceProvider.GetRequiredService<Foo>();
}

//Implementation of Foo
public class Foo(Bar bar)
{
}
```
- **Foo** would only be instantiated once, and it would hold on to **Bar** for it's lifetime, which is longer than intended scoped lifetime of **Bar** - this is a misconfiguration.
- You should consider validating scopes by passing **validateScopes: true**. When you validate scopes, you'd get an **InvalidOperationException** with a message similar to **"Cannot consume scoped service 'Bar' from singleton 'Foo'."**

## Scoped Service as Singleton
- When you're creting scoped services, without creating scope - service becomes singleton.
```c#
static void ScopedServiceBecomeSingleton()
{
    var services = new ServiceCollection();
    services.AddScoped<Bar>();
    
    using ServiceProvider serviceProvider = services.BuildServiceProvider(validateScopes: true); 
    using (IServiceScope scope = serviceProvider.CreateScope())
    {
        // correctly scoped resolution
        Bar correct = scope.ServiceProvider.GetRequiredService<Bar>();
    }

    // not within scope, becomes a singleton
    Bar avoid = serviceProvider.GetRequiredService<Bar>();
}
```