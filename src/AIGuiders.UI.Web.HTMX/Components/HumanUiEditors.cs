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
}
