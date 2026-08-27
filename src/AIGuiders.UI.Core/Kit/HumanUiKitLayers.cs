namespace AIGuiders.UI.Core.Kit;

using AIGuiders.UI.Core.Html;

/// <summary>Cross-product Human UI kit layering — platform L0–L1 spine.</summary>
public static class HumanUiKitLayers
{
    /// <summary>L0 — escaped HTML atoms (<see cref="HumanUiHtml"/>).</summary>
    public static class Atoms
    {
        public const string Html = nameof(HumanUiHtml);
    }

    /// <summary>L1 — generic controls and layout slots (no domain semantics).</summary>
    public static class Primitives
    {
        public const string Table = nameof(HumanUiTable);
        public const string List = nameof(HumanUiList);
        public const string Select = nameof(HumanUiSelect);
        public const string KitControls = nameof(HumanUiKitControls);
        public const string Badge = nameof(HumanUiBadge);
        public const string Panel = nameof(HumanUiPanel);
        public const string Flash = nameof(HumanUiFlash);
    }
}
