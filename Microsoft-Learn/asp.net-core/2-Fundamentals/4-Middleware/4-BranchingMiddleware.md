# Branching the Middleware Pipeline
1. **Map** extensions are used as a convention for branching.

## Map 
Map branches the request pipeline based on matches of the given request path.
```c#
app.Map("/map1", HandleMapTest1);
app.Map("/map2", HandleMapTest2);
app.Run(async context =>
{
    await context.Response.WriteAsync("Hello from non-Map delegate.");
});
app.Run();

static void HandleMapTest1(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("Map Test 1");
    });
}

static void HandleMapTest2(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("Map Test 2");
    });
}
```

## Nesting Map
Map supports nesting as well as multiple segments at once.
```c#
// Option 1
app.Map("/level1", level1App => {
    level1App.Map("/level2a", level2AApp => {
        // "/level1/level2a" processing
    });
    level1App.Map("/level2b", level2BApp => {
        // "/level1/level2b" processing
    });
});

// Option 2
app.Map("/map1/seg1", HandleMultiSeg);
```

## MapWhen
Branches the request pipeline based on the result of the given predicate.
```c#
app.MapWhen(context => context.Request.Query.ContainsKey("branch"), HandleBranch);
```

## UseWhen
- Branches the request pipeline based on the result of the given predicate. 
- Unlike with MapWhen, this branch is rejoined to the main pipeline if it doesn't short-circuit or contain a terminal middleware.
```c#
app.UseWhen(context => context.Request.Query.ContainsKey("branch"),
                appBuilder => HandleBranchAndRejoin(appBuilder));

void HandleBranchAndRejoin(IApplicationBuilder app)
{
    var logger = app.ApplicationServices.GetRequiredService<ILogger<Program>>(); 

    app.Use(async (context, next) =>
    {
        var branchVer = context.Request.Query["branch"];
        logger.LogInformation("Branch used = {branchVer}", branchVer);

        // Do work that write to the Response.
        await next();
        // Do work that doesn't write to the Response.
    });
}
```