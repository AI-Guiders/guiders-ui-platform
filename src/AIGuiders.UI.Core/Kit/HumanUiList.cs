using AIGuiders.UI.Core.Html;

namespace AIGuiders.UI.Core.Kit;

/// <summary>Generic list primitive — <c>ul</c> chrome; item HTML from caller.</summary>
public static class HumanUiList
{
    public const string DefaultPanelClass = HumanUiPanel.CatalogPanelClass;

    public static string Render(
        IReadOnlyList<string> itemHtml,
        string? listClass = null,
        string? listId = null,
        string? itemClass = null,
        string? dataTestId = null)
    {
        if (itemHtml.Count == 0)
            return "";

        var items = itemClass is null
            ? itemHtml.Select(html => HumanUiHtml.Li(null, html)).ToArray()
            : itemHtml.Select(html => HumanUiHtml.Li(itemClass, html)).ToArray();

        if (!string.IsNullOrWhiteSpace(listId))
            return HumanUiHtml.Tag(
                "ul",
                listClass,
                [new HumanUiHtml.HumanUiHtmlAttr("id", listId)],
                items);

        return string.IsNullOrWhiteSpace(dataTestId)
            ? HumanUiHtml.Ul(listClass ?? "", items)
            : HumanUiHtml.Ul(listClass ?? "", dataTestId, items);
    }

    public static string RenderPanel(
        IReadOnlyList<string> itemHtml,
        string? listClass = null,
        string? listId = null,
        string? itemClass = null,
        string? panelClass = DefaultPanelClass,
        string? emptyMessage = null,
        string? dataTestId = null)
    {
        if (itemHtml.Count == 0)
        {
            var message = emptyMessage ?? "Nothing here yet.";
            var empty = string.IsNullOrWhiteSpace(panelClass)
                ? HumanUiHtml.P("meta", HumanUiHtml.Text(message))
                : HumanUiHtml.Div(panelClass, HumanUiHtml.P("meta", HumanUiHtml.Text(message)));
            return empty;
        }

        var list = Render(itemHtml, listClass, listId, itemClass, dataTestId);
        return string.IsNullOrWhiteSpace(panelClass)
            ? list
            : HumanUiHtml.Div(panelClass, list);
    }
}
