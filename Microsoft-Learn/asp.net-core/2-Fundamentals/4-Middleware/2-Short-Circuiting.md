# Short-circuiting Middleware
1. When a delegate doesn't pass request to next delegate, it's called short-circuiting the request pipeline.
2. It is often desirable because it prevents unnecessary work. For example, **Static File Middleware** can act as a **Terminal Middleware** by processing a request for a static file and short-circuiting the rest of the pipeline.
3. Middleware added to the pipeline before the middleware that terminates further processing still processes code after their **next.Invoke** statement.
4. Below are the warnings related to attempting to write to a response that has already been sent.

> Don't call next.Invoke during or after the response has been sent to the client.
> After an **HttpResponse** has started, changes result in an exception.
> Writing to response body after calling next:
> - May cause a protocol violation, such as writing more than the started Content-Length.
> - May corrupt the body format, such as writing an HTML footer to a CSS file.
>>**HasStarted** is a useful hint to indicate if headers have been sent or the body has been written to.