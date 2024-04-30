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