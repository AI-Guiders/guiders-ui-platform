using System.Net;
using System.Text;

namespace AIGuiders.UI.Core.Html;

/// <summary>Escaped HTML atoms for SSR kit primitives (L0).</summary>
public static partial class HumanUiHtml
{
    public static string H(string? value) => WebUtility.HtmlEncode(value ?? "");

    public static string Fragment(params ReadOnlySpan<string> parts)
    {
        if (parts.Length == 0)
            return "";
        if (parts.Length == 1)
            return parts[0];
        var sb = new StringBuilder();
        foreach (var part in parts)
            sb.Append(part);
        return sb.ToString();
    }

    public static string Raw(string html) => html;

    public static string Text(string? text) => H(text);

    public readonly record struct HumanUiHtmlAttr(string Name, string? Value = null);

    public static string Tag(string name, string? cssClass, string inner) =>
        Tag(name, cssClass, [inner]);

    public static string Tag(
        string name,
        string? cssClass,
        ReadOnlySpan<string> inner)
    {
        var sb = new StringBuilder();
        sb.Append('<').Append(name);
        if (!string.IsNullOrEmpty(cssClass))
            sb.Append(" class=\"").Append(H(cssClass)).Append('"');
        sb.Append('>');
        foreach (var part in inner)
            sb.Append(part);
        sb.Append("</").Append(name).Append('>');
        return sb.ToString();
    }

    public static string Tag(
        string name,
        string? cssClass,
        ReadOnlySpan<HumanUiHtmlAttr> attrs,
        ReadOnlySpan<string> inner)
    {
        var sb = new StringBuilder();
        sb.Append('<').Append(name);
        if (!string.IsNullOrEmpty(cssClass))
            sb.Append(" class=\"").Append(H(cssClass)).Append('"');
        foreach (var attr in attrs)
        {
            if (attr.Value is null)
                sb.Append(' ').Append(attr.Name);
            else if (attr.Value.Length > 0)
                sb.Append(' ').Append(attr.Name).Append("=\"").Append(H(attr.Value)).Append('"');
        }

        sb.Append('>');
        foreach (var part in inner)
            sb.Append(part);
        sb.Append("</").Append(name).Append('>');
        return sb.ToString();
    }

    public static HumanUiHtmlAttr TestId(string? value) =>
        new("data-testid", value ?? "");

    public static string Div(string? cssClass, string inner) => Tag("div", cssClass, inner);

    public static string Div(string? cssClass, params ReadOnlySpan<string> inner) =>
        Tag("div", cssClass, inner);

    public static string P(string? cssClass, string inner) => Tag("p", cssClass, inner);

    public static string P(string? cssClass, params ReadOnlySpan<string> inner) =>
        Tag("p", cssClass, inner);

    public static string Li(string? cssClass, string inner) => Tag("li", cssClass, inner);

    public static string H2(string text, string? cssClass = null) => Tag("h2", cssClass, Text(text));

    public static string Span(string? cssClass, params ReadOnlySpan<string> inner) =>
        Tag("span", cssClass, inner);

    public static string Ul(string cssClass, params ReadOnlySpan<string> items) =>
        Ul(cssClass, dataTestId: null, items);

    public static string Ul(string cssClass, string? dataTestId, params ReadOnlySpan<string> items) =>
        string.IsNullOrEmpty(dataTestId)
            ? Tag("ul", cssClass, items)
            : Tag("ul", cssClass, [TestId(dataTestId)], items);

    public static string Table(string? cssClass, params ReadOnlySpan<string> inner) =>
        Tag("table", cssClass, inner);

    public static string TableWithTestId(string? cssClass, string dataTestId, params ReadOnlySpan<string> inner) =>
        Tag("table", cssClass, [TestId(dataTestId)], inner);

    /// <summary>HTMX/view island wrapper — keeps forge-view JS contract (<c>data-forge-island</c>).</summary>
    public static string DivWithIsland(string islandId, string? cssClass, string inner) =>
        Tag(
            "div",
            cssClass,
            [
                new HumanUiHtmlAttr("id", $"forge-island-{islandId}"),
                new HumanUiHtmlAttr("data-forge-island", islandId),
            ],
            [inner]);

    public static string Thead(params ReadOnlySpan<string> inner) => Tag("thead", null, inner);

    public static string Tbody(params ReadOnlySpan<string> inner) => Tag("tbody", null, inner);

    public static string HeaderRow(params ReadOnlySpan<string> inner) => Tag("tr", null, inner);

    public static string Tr(string? cssClass, string? id, params ReadOnlySpan<string> inner)
    {
        var sb = new StringBuilder("<tr");
        if (!string.IsNullOrEmpty(cssClass))
            sb.Append(" class=\"").Append(H(cssClass)).Append('"');
        if (!string.IsNullOrEmpty(id))
            sb.Append(" id=\"").Append(H(id)).Append('"');
        sb.Append('>');
        foreach (var part in inner)
            sb.Append(part);
        sb.Append("</tr>");
        return sb.ToString();
    }

    public static string Td(string? cssClass, params ReadOnlySpan<string> inner)
    {
        var sb = new StringBuilder("<td");
        if (!string.IsNullOrEmpty(cssClass))
            sb.Append(" class=\"").Append(H(cssClass)).Append('"');
        sb.Append('>');
        foreach (var part in inner)
            sb.Append(part);
        sb.Append("</td>");
        return sb.ToString();
    }

    public static string Th(string text) => Tag("th", null, Text(text));

    public static string Th(string? cssClass, string text) => Tag("th", cssClass, Text(text));

    public static string Option(string value, string label, bool selected)
    {
        var sb = new StringBuilder();
        sb.Append("<option value=\"").Append(H(value)).Append('"');
        if (selected)
            sb.Append(" selected");
        sb.Append('>').Append(H(label)).Append("</option>");
        return sb.ToString();
    }
}
