using AIGuiders.UI.Core.Html;

namespace AIGuiders.UI.Core.Kit;

/// <summary>GitHub-style settings section — card with titled rows.</summary>
public static class HumanUiSettingsSection
{
    public static string Render(string title, params ReadOnlySpan<string> rows) =>
        Render(title, subtitle: null, headerActionHtml: null, badgeHtml: null, rows);

    public static string Render(
        string title,
        string? subtitle,
        string? headerActionHtml,
        string? badgeHtml,
        params ReadOnlySpan<string> rows) =>
        HumanUiHtml.Div(
            "settings-section panel catalog-panel",
            HumanUiHtml.Div(
                "settings-section-header",
                HumanUiHtml.Div(
                    "settings-section-header-main",
                    HumanUiHtml.Div(
                        "settings-section-title-row",
                        HumanUiHtml.H2(title, "settings-section-title"),
                        badgeHtml ?? ""),
                    string.IsNullOrWhiteSpace(subtitle)
                        ? ""
                        : HumanUiHtml.P("settings-section-subtitle", HumanUiHtml.Text(subtitle))),
                string.IsNullOrWhiteSpace(headerActionHtml)
                    ? ""
                    : HumanUiHtml.Div("settings-section-header-action", headerActionHtml)),
            HumanUiHtml.Div("settings-section-body", HumanUiHtml.Fragment(rows)));

    public static string Footnote(string text) =>
        HumanUiHtml.P("settings-section-footnote", HumanUiHtml.Text(text));

    public static string FootnoteHtml(params ReadOnlySpan<string> inner) =>
        HumanUiHtml.P("settings-section-footnote", inner);
}
