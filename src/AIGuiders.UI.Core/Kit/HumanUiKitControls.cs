using AIGuiders.UI.Core.Html;

namespace AIGuiders.UI.Core.Kit;

/// <summary>Reusable kit form controls (server-rendered).</summary>
public static class HumanUiKitControls
{
    public const string FormInputClass = "human-form-input";
    public const string FormSelectClass = "human-form-select";
    public const string FormCheckClass = "human-form-check";
    public const string FormCheckGroupClass = "human-form-check-group";

    public static string CheckboxRow(
        string id,
        string name,
        string label,
        string value,
        bool isChecked = false) =>
        HumanUiHtml.LabelWithClass(
            id,
            FormCheckClass,
            HumanUiHtml.Input(
                new HumanUiInputSpec(name, Type: "checkbox", Id: id, Value: value, Checked: isChecked)),
            HumanUiHtml.Text(label));

    public static string ScopeCheckboxGroup(
        bool readChecked = true,
        bool writeChecked = true,
        bool acceptMergeChecked = true) =>
        HumanUiHtml.Div(
            FormCheckGroupClass,
            CheckboxRow("scope-read", "scopes", "read", "read", readChecked),
            CheckboxRow("scope-write", "scopes", "write", "write", writeChecked),
            CheckboxRow("scope-accept", "scopes", "accept_merge", "accept_merge", acceptMergeChecked));

    public static string TextField(
        string id,
        string name,
        string label,
        string? placeholder = null,
        bool required = false,
        int? maxLength = null,
        string type = "text",
        string? value = null,
        string? autocomplete = null,
        string? inputMode = null) =>
        HumanUiHtml.Fragment(
            HumanUiHtml.Div(
                "human-form-field",
                HumanUiHtml.LabelWithClass(id, "human-form-label", HumanUiHtml.Text(label)),
                HumanUiHtml.Input(new HumanUiInputSpec(
                    name,
                    Type: type,
                    Id: id,
                    Value: value,
                    Placeholder: placeholder,
                    Autocomplete: autocomplete,
                    InputMode: inputMode,
                    Required: required,
                    MaxLength: maxLength,
                    CssClass: FormInputClass))));

    public static string PasswordField(
        string id,
        string name,
        string label,
        bool required = true,
        string? autocomplete = "off") =>
        TextField(id, name, label, required: required, type: "password", autocomplete: autocomplete);

    public static string CodeField(string id, string name, string label, bool required = true) =>
        TextField(
            id,
            name,
            label,
            required: required,
            autocomplete: "one-time-code",
            inputMode: "numeric");

    public static string TextFieldWithHint(
        string id,
        string name,
        string label,
        string hint,
        string? placeholder = null,
        bool required = false,
        string? value = null) =>
        HumanUiHtml.Div(
            "human-form-field",
            HumanUiHtml.LabelWithClass(id, "human-form-label", HumanUiHtml.Text(label)),
            HumanUiHtml.Input(new HumanUiInputSpec(
                name,
                Id: id,
                Value: value,
                Placeholder: placeholder,
                Required: required,
                CssClass: FormInputClass)),
            HumanUiHtml.P("human-form-hint", HumanUiHtml.Text(hint)));

    public static string SelectField(
        string id,
        string name,
        string label,
        IReadOnlyList<HumanUiSelectOption> options,
        string? hint = null,
        bool required = false) =>
        HumanUiHtml.Div(
            "human-form-field",
            HumanUiHtml.LabelWithClass(id, "human-form-label", HumanUiHtml.Text(label)),
            string.IsNullOrWhiteSpace(hint) ? "" : HumanUiHtml.P("human-form-hint", HumanUiHtml.Text(hint)),
            HumanUiSelect.Render(new HumanUiSelectSpec(
                id,
                name,
                options,
                CssClass: FormSelectClass,
                Required: required)));

    public static string ToggleField(string id, string name, string label, bool isChecked) =>
        HumanUiHtml.Div(
            "human-form-field",
            CheckboxRow(id, name, label, "true", isChecked));

    public static string HiddenField(string name, string value) =>
        HumanUiHtml.Input(new HumanUiInputSpec(name, Type: "hidden", Value: value));

    public static string Fieldset(string legend, params ReadOnlySpan<string> inner) =>
        HumanUiHtml.Tag(
            "fieldset",
            "human-form-fieldset",
            [
                HumanUiHtml.Tag("legend", "human-form-legend", HumanUiHtml.Text(legend)),
                ..inner,
            ]);

    public static string DangerSubmit(string label, string? testId = null) =>
        SubmitActions(label, "btn btn-danger", testId);

    private static string SubmitActions(string label, string buttonClass, string? testId = null)
    {
        var attrs = new List<HumanUiHtml.HumanUiHtmlAttr> { new("type", "submit") };
        if (!string.IsNullOrWhiteSpace(testId))
            attrs.Add(HumanUiHtml.TestId(testId));

        return HumanUiHtml.Tag(
            "div",
            "human-form-actions page-actions",
            HumanUiHtml.Tag(
                "button",
                buttonClass,
                attrs.ToArray(),
                [HumanUiHtml.Text(label)]));
    }

    public static string PrimarySubmit(string label = "Save", string? testId = null) =>
        SubmitActions(label, "btn btn-primary", testId);

    public static string RadioRow(
        string id,
        string name,
        string value,
        string label,
        string? hint,
        bool selected = false) =>
        HumanUiHtml.Tag(
            "label",
            "radio-row",
            [new HumanUiHtml.HumanUiHtmlAttr("for", id)],
            [
                HumanUiHtml.Input(new HumanUiInputSpec(name, Type: "radio", Id: id, Value: value, Checked: selected)),
                HumanUiHtml.Span("radio-label", HumanUiHtml.Text(label)),
                string.IsNullOrWhiteSpace(hint)
                    ? ""
                    : HumanUiHtml.Span("radio-hint", HumanUiHtml.Text(hint)),
            ]);

    public static string TokenRevealPanel(string plainToken, string? tokenName = null)
    {
        var nameLine = string.IsNullOrWhiteSpace(tokenName)
            ? ""
            : HumanUiHtml.P(
                "human-form-hint",
                "Name: ",
                HumanUiHtml.Tag("strong", null, HumanUiHtml.Text(tokenName)));

        return HumanUiPanel.Form(
            HumanUiHtml.Fragment(
                HumanUiHtml.H2("New token — copy now", "human-form-legend"),
                HumanUiHtml.P(
                    "human-form-hint",
                    "This is the only time the plain token is shown. Store it in your password manager or ",
                    HumanUiHtml.Tag("code", null, HumanUiHtml.Text("forge auth login")),
                    " flow."),
                nameLine,
                HumanUiHtml.Tag(
                    "pre",
                    "auth-panel-body",
                    HumanUiHtml.Tag("code", null, HumanUiHtml.Text(plainToken)))));
    }
}
