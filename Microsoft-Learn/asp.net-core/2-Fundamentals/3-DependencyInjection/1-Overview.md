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