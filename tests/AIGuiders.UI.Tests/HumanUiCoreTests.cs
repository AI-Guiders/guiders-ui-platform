using AIGuiders.UI.Core.Editors;
using AIGuiders.UI.Core.PageChrome;
using AIGuiders.UI.Tokens;
using AIGuiders.UI.Web.HTMX;
using Xunit;

namespace AIGuiders.UI.Tests;

public sealed class HumanUiCoreTests
{
    [Fact]
    public void PageChrome_models_round_trip()
    {
        var title = new PageChromeTitleModel("Agent Forge");
        Assert.Equal("Agent Forge", title.Title);
    }

    [Fact]
    public void View_paths_are_stable()
    {
        Assert.Equal("/Pages/Shared/PageChrome/Title", HumanUiViewPaths.PageChrome.Title);
        Assert.Equal("/Pages/Shared/PageChrome/MetaLinks", HumanUiViewPaths.PageChrome.MetaLinks);
        Assert.Equal("/Pages/Shared/EmptyStates/Message", HumanUiViewPaths.EmptyStates.Message);
        Assert.Equal("/Pages/Shared/Editors/FormatToolbar", HumanUiViewPaths.Editors.FormatToolbar);
    }

    [Fact]
    public void Editor_format_toolbar_defaults_match_slash_ids()
    {
        var ids = EditorFormatToolbarDefaults.MarkdownGhLike.Select(b => b.Id).ToList();
        Assert.Equal(["h1", "h2", "bold", "italic", "quote", "code", "link", "bul", "num"], ids);
    }

    [Fact]
    public void Tokens_css_includes_editor_kit_rules()
    {
        Assert.Contains(".editor-ccl-host", HumanUiTokensCss.Content, StringComparison.Ordinal);
        Assert.Contains(".editor-format-toolbar", HumanUiTokensCss.Content, StringComparison.Ordinal);
        Assert.Contains(".btn-ghost", HumanUiTokensCss.Content, StringComparison.Ordinal);
        Assert.Contains(".btn-primary", HumanUiTokensCss.Content, StringComparison.Ordinal);
    }
}
