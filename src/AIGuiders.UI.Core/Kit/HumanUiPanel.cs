using AIGuiders.UI.Core.Html;

namespace AIGuiders.UI.Core.Kit;

/// <summary>Panel chrome primitive — catalog/settings surfaces.</summary>
public static class HumanUiPanel
{
    public const string CatalogPanelClass = "panel catalog-panel";
    public const string FormPanelClass = "panel catalog-panel human-settings-form-panel";

    public static string Render(string inner, string? panelClass = CatalogPanelClass) =>
        string.IsNullOrWhiteSpace(panelClass)
            ? inner
            : HumanUiHtml.Div(panelClass, inner);

    public static string Form(string inner) => Render(inner, FormPanelClass);

    public static string WithIsland(string? islandId, string inner) =>
        string.IsNullOrWhiteSpace(islandId)
            ? inner
            : HumanUiHtml.DivWithIsland(islandId.Trim(), "forge-view-island", inner);
}
