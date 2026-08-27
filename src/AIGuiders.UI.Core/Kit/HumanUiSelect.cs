using AIGuiders.UI.Core.Html;

namespace AIGuiders.UI.Core.Kit;

public readonly record struct HumanUiSelectOption(string Value, string Label, bool Selected = false);

public sealed record HumanUiSelectSpec(
    string Id,
    string Name,
    IReadOnlyList<HumanUiSelectOption> Options,
    string? Label = null,
    string? Hint = null,
    string CssClass = "human-form-input",
    bool Required = false,
    string? Form = null,
    string? OnChange = null);

public static class HumanUiSelect
{
    public const string TableSelectClass = "catalog-table-select";
    public const string FormSelectClass = "human-form-input";

    public static string Render(HumanUiSelectSpec spec)
    {
        var options = spec.Options
            .Select(o => HumanUiHtml.Option(o.Value, o.Label, o.Selected))
            .ToArray();
        return HumanUiHtml.Select(
            spec.Id,
            spec.Name,
            spec.CssClass,
            spec.Required,
            spec.Form,
            spec.OnChange,
            options);
    }

    public static string RenderField(HumanUiSelectSpec spec)
    {
        var select = Render(spec);
        if (string.IsNullOrWhiteSpace(spec.Label))
            return select;

        var label = HumanUiHtml.LabelWithClass(spec.Id, "human-form-label", HumanUiHtml.Text(spec.Label));
        if (string.IsNullOrWhiteSpace(spec.Hint))
            return HumanUiHtml.Fragment(label, select);

        return HumanUiHtml.Fragment(
            label,
            HumanUiHtml.P("human-form-hint", HumanUiHtml.Text(spec.Hint)),
            select);
    }

    public static string RenderTableSelect(
        string id,
        string name,
        IReadOnlyList<HumanUiSelectOption> options,
        bool required = false,
        string? form = null,
        string? onChange = null) =>
        Render(new HumanUiSelectSpec(
            id,
            name,
            options,
            CssClass: TableSelectClass,
            Required: required,
            Form: form,
            OnChange: onChange));
}
