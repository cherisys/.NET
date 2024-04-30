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