using System.Text;

namespace AIGuiders.UI.Core.Html;

public sealed record HumanUiInputSpec(
    string Name,
    string Type = "text",
    string? Id = null,
    string? Value = null,
    string? Placeholder = null,
    string? Autocomplete = null,
    string? InputMode = null,
    bool Required = false,
    string? Form = null,
    bool Checked = false,
    int? MaxLength = null);

public static partial class HumanUiHtml
{
    public static string LabelWithClass(string inputId, string? labelCssClass, params ReadOnlySpan<string> inner)
    {
        var sb = new StringBuilder("<label for=\"").Append(H(inputId)).Append('"');
        if (!string.IsNullOrEmpty(labelCssClass))
            sb.Append(" class=\"").Append(H(labelCssClass)).Append('"');
        sb.Append('>');
        foreach (var part in inner)
            sb.Append(part);
        sb.Append("</label>");
        return sb.ToString();
    }

    public static string Input(HumanUiInputSpec spec)
    {
        var sb = new StringBuilder("<input");
        AppendAttr(sb, "id", spec.Id ?? spec.Name);
        AppendAttr(sb, "name", spec.Name);
        AppendAttr(sb, "type", spec.Type);
        AppendAttr(sb, "value", spec.Value);
        AppendAttr(sb, "placeholder", spec.Placeholder);
        AppendAttr(sb, "autocomplete", spec.Autocomplete);
        AppendAttr(sb, "inputmode", spec.InputMode);
        AppendAttr(sb, "form", spec.Form);
        if (spec.MaxLength is > 0)
            sb.Append(" maxlength=\"").Append(spec.MaxLength.Value).Append('"');
        if (spec.Required)
            sb.Append(" required");
        if (spec.Checked)
            sb.Append(" checked");
        sb.Append('>');
        return sb.ToString();
    }

    public static string Select(
        string id,
        string name,
        string? cssClass,
        bool required,
        string? formId,
        string? onChange,
        params ReadOnlySpan<string> options)
    {
        var sb = new StringBuilder("<select");
        AppendAttr(sb, "id", id);
        AppendAttr(sb, "name", name);
        AppendAttr(sb, "class", cssClass);
        AppendAttr(sb, "form", formId);
        AppendAttr(sb, "onchange", onChange);
        if (required)
            sb.Append(" required");
        sb.Append('>');
        foreach (var option in options)
            sb.Append(option);
        sb.Append("</select>");
        return sb.ToString();
    }

    public static string Form(string method, string action, string? cssClass, params ReadOnlySpan<string> inner)
    {
        var sb = new StringBuilder("<form");
        AppendAttr(sb, "method", method);
        AppendAttr(sb, "action", action);
        AppendAttr(sb, "class", cssClass);
        sb.Append('>');
        foreach (var part in inner)
            sb.Append(part);
        sb.Append("</form>");
        return sb.ToString();
    }

    private static void AppendAttr(StringBuilder sb, string name, string? value)
    {
        if (string.IsNullOrEmpty(value))
            return;
        sb.Append(' ').Append(name).Append("=\"").Append(H(value)).Append('"');
    }
}
