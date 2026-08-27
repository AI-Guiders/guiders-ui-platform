using AIGuiders.UI.Core.Html;

namespace AIGuiders.UI.Core.Kit;

/// <summary>Flash / banner messages — settings dialect and admin dialect.</summary>
public static class HumanUiFlash
{
    public static string Info(string? message) =>
        string.IsNullOrWhiteSpace(message)
            ? ""
            : HumanUiHtml.P("human-flash human-flash-info", HumanUiHtml.Text(message));

    public static string Error(string? message) =>
        string.IsNullOrWhiteSpace(message)
            ? ""
            : HumanUiHtml.P("human-flash human-flash-error", HumanUiHtml.Text(message));

    public static string ErrorContent(params ReadOnlySpan<string> inner) =>
        HumanUiHtml.P("human-flash human-flash-error", inner);

    public static string AdminOk(string message) =>
        HumanUiHtml.Div("msg ok", HumanUiHtml.Text(message));

    public static string AdminError(string message) =>
        HumanUiHtml.Div("msg error", HumanUiHtml.Text(message));

    public static string AdminFromMap(
        string? code,
        IReadOnlyDictionary<string, (string Message, bool Ok)> map)
    {
        if (string.IsNullOrWhiteSpace(code) || !map.TryGetValue(code.Trim(), out var entry))
            return "";

        return entry.Ok ? AdminOk(entry.Message) : AdminError(entry.Message);
    }
}
