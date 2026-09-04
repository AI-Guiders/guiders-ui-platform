using AIGuiders.UI.Core.Html;

namespace AIGuiders.UI.Core.Kit;

/// <summary>Single settings row — title, meta, action on the right (GH-style).</summary>
public static class HumanUiSettingsRow
{
    public static string Render(string title, string? meta, string actionHtml) =>
        HumanUiHtml.Div(
            "settings-row",
            HumanUiHtml.Div(
                "settings-row-main",
                HumanUiHtml.Div("settings-row-title", HumanUiHtml.Text(title)),
                string.IsNullOrWhiteSpace(meta)
                    ? ""
                    : HumanUiHtml.Div("settings-row-meta", HumanUiHtml.Text(meta))),
            HumanUiHtml.Div("settings-row-action", actionHtml));

    public static string RenderRich(
        string titleHtml,
        string? metaHtml,
        string actionHtml) =>
        HumanUiHtml.Div(
            "settings-row",
            HumanUiHtml.Div(
                "settings-row-main",
                HumanUiHtml.Div("settings-row-title", titleHtml),
                string.IsNullOrWhiteSpace(metaHtml)
                    ? ""
                    : HumanUiHtml.Div("settings-row-meta", metaHtml)),
            HumanUiHtml.Div("settings-row-action", actionHtml));
}
