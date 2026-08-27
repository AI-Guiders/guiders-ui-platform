using AIGuiders.UI.Core.Html;

namespace AIGuiders.UI.Core.Kit;

public enum HumanUiBadgeKind
{
    On,
    Off,
    Warn,
    Neutral,
}

/// <summary>Status / session badge primitive (L1).</summary>
public static class HumanUiBadge
{
    public static string Render(string label, HumanUiBadgeKind kind = HumanUiBadgeKind.Neutral) =>
        kind switch
        {
            HumanUiBadgeKind.On => HumanUiHtml.Span("badge on", HumanUiHtml.Text(label)),
            HumanUiBadgeKind.Off => HumanUiHtml.Span("badge off", HumanUiHtml.Text(label)),
            HumanUiBadgeKind.Warn => HumanUiHtml.Span("badge warn", HumanUiHtml.Text(label)),
            _ => HumanUiHtml.Span("badge", HumanUiHtml.Text(label)),
        };

    public static string Session(string label) =>
        HumanUiHtml.Span("badge", HumanUiHtml.Text(label));
}
