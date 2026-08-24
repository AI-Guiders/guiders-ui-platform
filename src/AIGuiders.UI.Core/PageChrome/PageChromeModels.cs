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
