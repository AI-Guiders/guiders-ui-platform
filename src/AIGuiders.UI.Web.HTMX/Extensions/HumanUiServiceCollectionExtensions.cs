using AIGuiders.UI.Web.HTMX.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AIGuiders.UI.Web.HTMX.Extensions;

public static class HumanUiServiceCollectionExtensions
{
    public static IMvcBuilder AddHumanUiWebHtmx(this IMvcBuilder mvc)
    {
        mvc.AddApplicationPart(typeof(HumanUiViewMarker).Assembly);
        return mvc;
    }

    public static IServiceCollection AddHumanUiRazor(this IServiceCollection services)
    {
        services.AddScoped<HumanUiRazorRenderService>();
        services.AddScoped<HumanUiRazorBridge>();
        services.AddSingleton<HumanUiRazorBridgeHolder>();
        return services;
    }
}
