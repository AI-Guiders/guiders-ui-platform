using AIGuiders.UI.Core.EmptyStates;
using AIGuiders.UI.Core.PageChrome;
using AIGuiders.UI.Web.HTMX.Rendering;

namespace AIGuiders.UI.Web.HTMX.Components;

public static class HumanUiPageChrome
{
    public static string RenderLogo(PageChromeLogoModel? model = null) =>
        HumanUiRazorBridgeHolder.RenderPartialStatic(
            HumanUiViewPaths.PageChrome.Logo,
            model ?? new PageChromeLogoModel("/", "Home"));

    public static string RenderTitle(string title) =>
        HumanUiRazorBridgeHolder.RenderPartialStatic(
            HumanUiViewPaths.PageChrome.Title,
            new PageChromeTitleModel(title));

    public static string RenderSubtitle(string text) =>
        HumanUiRazorBridgeHolder.RenderPartialStatic(
            HumanUiViewPaths.PageChrome.Subtitle,
            new PageChromeSubtitleModel(text));

    public static string RenderHomeIntro(string text) =>
        HumanUiRazorBridgeHolder.RenderPartialStatic(
            HumanUiViewPaths.PageChrome.HomeIntro,
            new PageChromeHomeIntroModel(text));

    public static string RenderSection(string title) =>
        HumanUiRazorBridgeHolder.RenderPartialStatic(
            HumanUiViewPaths.PageChrome.Section,
            new PageChromeSectionModel(title));

    public static string RenderSectionHome(string title) =>
        HumanUiRazorBridgeHolder.RenderPartialStatic(
            HumanUiViewPaths.PageChrome.SectionHome,
            new PageChromeSectionHomeModel(title));

    public static string RenderMetaLinks(PageChromeMetaLinksModel model) =>
        HumanUiRazorBridgeHolder.RenderPartialStatic(
            HumanUiViewPaths.PageChrome.MetaLinks,
            model);
}

public static class HumanUiEmptyStates
{
    public static string Render(string message) =>
        HumanUiRazorBridgeHolder.RenderPartialStatic(
            HumanUiViewPaths.EmptyStates.Message,
            new EmptyStateMessageModel(message));

    public static string RenderHomeCatalogEmpty(HomeCatalogEmptyModel? model = null) =>
        HumanUiRazorBridgeHolder.RenderPartialStatic(
            HumanUiViewPaths.EmptyStates.HomeCatalog,
            model ?? new HomeCatalogEmptyModel());

    public static string RenderCreateRepoHint(CreateRepoHintModel? model = null) =>
        HumanUiRazorBridgeHolder.RenderPartialStatic(
            HumanUiViewPaths.EmptyStates.CreateRepoHint,
            model ?? new CreateRepoHintModel());
}
