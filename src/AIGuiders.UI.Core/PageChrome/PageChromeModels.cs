namespace AIGuiders.UI.Core.PageChrome;

public sealed record PageChromeTitleModel(string Title);

public sealed record PageChromeSectionModel(string Title);

public sealed record PageChromeSectionHomeModel(string Title);

public sealed record PageChromeSubtitleModel(string Text);

public sealed record PageChromeHomeIntroModel(string Text);

public sealed record PageChromeLogoModel(
    string HomeHref,
    string BrandText,
    string BrandMark = "⚒");

/// <summary>Muted footer / secondary action link in a <c>p.meta</c> row.</summary>
public sealed record PageChromeMetaLinkModel(
    string Href,
    string Label,
    bool OpenInNewTab = false,
    bool Active = false);

/// <summary>Dot-separated meta link row (PageChrome kit).</summary>
public sealed record PageChromeMetaLinksModel(
    IReadOnlyList<PageChromeMetaLinkModel> Links,
    string? TestId = null,
    string? Prefix = null,
    string? CssClass = null);
