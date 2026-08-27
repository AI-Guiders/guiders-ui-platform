using AIGuiders.UI.Core.Editors;
using AIGuiders.UI.Web.HTMX.Rendering;

namespace AIGuiders.UI.Web.HTMX.Components;

public static class HumanUiEditors
{
    public static string RenderFormatToolbar(EditorFormatToolbarModel? model = null) =>
        HumanUiRazorBridgeHolder.RenderPartialStatic(
            HumanUiViewPaths.Editors.FormatToolbar,
            model ?? new EditorFormatToolbarModel(EditorFormatToolbarDefaults.MarkdownGhLike));

    public static string RenderFormatToolbarForTextarea(
        string textareaId,
        IReadOnlyList<EditorFormatToolbarButtonModel>? buttons = null) =>
        RenderFormatToolbar(new EditorFormatToolbarModel(
            buttons ?? EditorFormatToolbarDefaults.MarkdownGhLike,
            TargetTextareaId: textareaId));

    public static string RenderCcl(EditorCclModel? model = null) =>
        HumanUiRazorBridgeHolder.RenderPartialStatic(
            HumanUiViewPaths.Editors.Ccl,
            model ?? new EditorCclModel());

    public static string RenderCclForTextarea(string textareaId) =>
        RenderCcl(new EditorCclModel(TargetTextareaId: textareaId));

    public static string RenderLineGutter(EditorLineGutterModel? model = null) =>
        HumanUiRazorBridgeHolder.RenderPartialStatic(
            HumanUiViewPaths.Editors.LineGutter,
            model ?? new EditorLineGutterModel());

    public static string RenderLineGutterForTextarea(string textareaId) =>
        RenderLineGutter(new EditorLineGutterModel(TargetTextareaId: textareaId));
}
