# Blazor Architecture
1. Blazor is a client-side web UI framework.
2. Blazor is not based on request-reply model.
3. User interactions are handled as events that aren't in the context of any particular HTTP request.
4. Blazor components are .NET Classes that represent a reusable piece of UI.
5. Each component maintains it's own state and specifies it's own rendering logic.
6. Components don't render directly to DOM.
7. Components render to an in-memory representation of the DOM called a **RenderTree** so that blazor can track the changes.
8. Not the whole UI, but the calculated UI difference from the RenderTree is then rendered to the DOM.
9. Blazor uses a **SynchronizationContext** to enforce a single logical thread of execution.
10. A component's lifecycle methods and any event callbacks that are raised by blazor are executed on **SynchronizationContext**.