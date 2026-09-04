using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;

namespace AIGuiders.UI.Web.HTMX.Rendering;

/// <summary>Renders embedded Razor views from AIGuiders.UI.Web.HTMX.</summary>
public class HumanUiRazorRenderService(
    IRazorViewEngine viewEngine,
    ITempDataProvider tempDataProvider,
    IHttpContextAccessor httpContextAccessor)
{
    public Task<string> RenderViewAsync(string viewPath, object? model, HttpContext? httpContext = null) =>
        RenderAsync(viewPath, model, isMainPage: true, httpContext);

    public Task<string> RenderPartialAsync(string partialPath, object? model, HttpContext? httpContext = null) =>
        RenderAsync(partialPath, model, isMainPage: false, httpContext);

    private async Task<string> RenderAsync(
        string viewPath,
        object? model,
        bool isMainPage,
        HttpContext? httpContext)
    {
        var http = httpContext ?? httpContextAccessor.HttpContext
            ?? throw new InvalidOperationException("HttpContext is required to render Human UI Razor views.");

        var normalized = NormalizeViewPath(viewPath);
        var actionContext = new ActionContext(http, http.GetRouteData() ?? new RouteData(), new ActionDescriptor());
        var viewResult = viewEngine.GetView(executingFilePath: null, viewPath: normalized, isMainPage);

        if (!viewResult.Success)
            viewResult = viewEngine.FindView(actionContext, normalized, isMainPage);

        if (!viewResult.Success)
            throw new InvalidOperationException(
                $"Razor view '{normalized}' was not found. Searched: {string.Join(", ", viewResult.SearchedLocations)}");

        await using var writer = new StringWriter();
        var viewData = new ViewDataDictionary(
            new EmptyModelMetadataProvider(),
            new ModelStateDictionary())
        {
            Model = model,
        };

        var viewContext = new ViewContext(
            actionContext,
            viewResult.View,
            viewData,
            new TempDataDictionary(http, tempDataProvider),
            writer,
            new HtmlHelperOptions { ClientValidationEnabled = false });

        await viewResult.View.RenderAsync(viewContext).ConfigureAwait(false);
        return writer.ToString();
    }

    private static string NormalizeViewPath(string viewPath)
    {
        var path = viewPath.Trim();
        if (!path.StartsWith('/'))
            path = "/" + path;
        if (!path.EndsWith(".cshtml", StringComparison.OrdinalIgnoreCase))
            path += ".cshtml";
        return path;
    }
}
