# ASP.NET Core Application Startup
1. **Program.cs** file contains application startup code.
2. Program.cs file is supported by following apps: Razor Pages, MVC, Web API, Minimal API.
3. Apps using **EventSource** can measure the startup time to understand and optimize startup performance.
4. The **ServerReady** event in **Microsoft.AspNetCore.Hosting** represents the point where the server is ready to respond to requests.

# Extend Startup with Filters
1. Use **IStartupFilter** to configure middleware at the beginning or end of an app's middleware pipeline without an explicit call to **Use{Middleware}**.
2. IStartupFilter allows a different component to call Use{Middleware} on behalf of app author.
3. Create a pipeline of Configure methods. **IStartupFilter.Configure** can set a middleware to run before or after middleware added by libraries.
4. Each IStartupFilter can add one or more middlewares in the request pipeline.

# Add Configuration at Startup from an External Assembly
An **IHostingStartup** implementation allows adding enhancements to an app at startup from an external assembly outside of the app's Program.cs file.