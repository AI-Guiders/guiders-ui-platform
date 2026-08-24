using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace AIGuiders.UI.Web.HTMX.Rendering;

public class HumanUiRazorBridge(
    IHttpContextAccessor httpContextAccessor,
    HumanUiRazorRenderService razor)
{
    public string RenderPartial(string partialPath, object? model, HttpContext? httpContext = null) =>
        RenderPartialAsync(partialPath, model, httpContext).GetAwaiter().GetResult();

    public Task<string> RenderPartialAsync(string partialPath, object? model, HttpContext? httpContext = null)
    {
        var http = httpContext ?? httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("HttpContext is required to render Human UI Razor partials.");
        return razor.RenderPartialAsync(partialPath, model, http);
    }
}

public sealed class HumanUiRazorBridgeHolder
{
    private static IHttpContextAccessor? _accessor;

    public HumanUiRazorBridgeHolder(IHttpContextAccessor httpContextAccessor) =>
        _accessor = httpContextAccessor;

    public static string RenderPartialStatic(string partialPath, object? model)
    {
        var http = _accessor?.HttpContext
            ?? throw new InvalidOperationException("HttpContext is required to render Human UI Razor partials.");
        var bridge = http.RequestServices.GetRequiredService<HumanUiRazorBridge>();
        return bridge.RenderPartial(partialPath, model, http);
    }

    public static string RenderViewStatic(string viewPath, object? model)
    {
        var http = _accessor?.HttpContext
            ?? throw new InvalidOperationException("HttpContext is required to render Human UI Razor views.");
        var razor = http.RequestServices.GetRequiredService<HumanUiRazorRenderService>();
        return razor.RenderViewAsync(viewPath, model, http).GetAwaiter().GetResult();
    }

    public static void InitializeForTests(HttpContext http, IHttpContextAccessor accessor)
    {
        _accessor = accessor;
        if (accessor is HttpContextAccessor concrete)
            concrete.HttpContext = http;
    }

    public static bool IsHttpContextAvailable => _accessor?.HttpContext is not null;
}
