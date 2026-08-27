namespace AIGuiders.UI.Core.Editors;

/// <summary>Toolbar button — <see cref="Id"/> must match forge-slash <c>formatCatalog</c> entry id.</summary>
public sealed record EditorFormatToolbarButtonModel(
    string Id,
    string Label,
    string Symbol,
    bool DividerAfter = false);

/// <summary>Markdown format toolbar above a composer textarea (Human UI kit).</summary>
public sealed record EditorFormatToolbarModel(
    IReadOnlyList<EditorFormatToolbarButtonModel> Buttons,
    string? TargetTextareaId = null,
    string? TestId = "forge-editor-format-toolbar");

public static class EditorFormatToolbarDefaults
{
    /// <summary>GH New Issue toolbar subset — ids align with ViewShell <c>formatCatalog</c>.</summary>
    public static IReadOnlyList<EditorFormatToolbarButtonModel> MarkdownGhLike { get; } = new[]
    {
        new EditorFormatToolbarButtonModel("h2", "Heading", "H", DividerAfter: true),
        new EditorFormatToolbarButtonModel("bold", "Bold", "B"),
        new EditorFormatToolbarButtonModel("italic", "Italic", "I", DividerAfter: true),
        new EditorFormatToolbarButtonModel("quote", "Quote", "“"),
        new EditorFormatToolbarButtonModel("code", "Code", "<>"),
        new EditorFormatToolbarButtonModel("link", "Link", "[]", DividerAfter: true),
        new EditorFormatToolbarButtonModel("bul", "Bullet list", "•"),
        new EditorFormatToolbarButtonModel("num", "Numbered list", "1."),
    };
}

public sealed record EditorCclModel(
    string? TargetTextareaId = null,
    string? TestId = "forge-editor-ccl",
    string? Placeholder = null);

public sealed record EditorLineGutterModel(
    string? TargetTextareaId = null,
    string? TestId = "forge-editor-line-gutter");

public sealed record EditorMarkdownHostModel(
    string TextareaId,
    string? ToolbarTestId = null,
    bool ShowLineGutter = true,
    bool ShowCcl = true);
