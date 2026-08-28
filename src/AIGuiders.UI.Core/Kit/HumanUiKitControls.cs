using AIGuiders.UI.Core.Html;

namespace AIGuiders.UI.Core.Kit;

/// <summary>Reusable kit form controls (server-rendered).</summary>
public static class HumanUiKitControls
{
    public static string CheckboxRow(
        string id,
        string name,
        string label,
        string value,
        bool isChecked = false) =>
        HumanUiHtml.LabelWithClass(
            id,
            "radio-row",
            HumanUiHtml.Input(
                new HumanUiInputSpec(name, Type: "checkbox", Id: id, Value: value, Checked: isChecked)),
            HumanUiHtml.Span("radio-label", HumanUiHtml.Text(label)));

    public static string ScopeCheckboxGroup(
        bool readChecked = true,
        bool writeChecked = true,
        bool acceptMergeChecked = true) =>
        HumanUiHtml.Fragment(
            CheckboxRow("scope-read", "scopes", "read", "read", readChecked),
            CheckboxRow("scope-write", "scopes", "write", "write", writeChecked),
            CheckboxRow("scope-accept", "scopes", "accept_merge", "accept_merge", acceptMergeChecked));

    public static string TextField(
        string id,
        string name,
        string label,
        string? placeholder = null,
        bool required = false,
        int? maxLength = null) =>
        HumanUiHtml.Fragment(
            HumanUiHtml.Div(
                "human-form-field",
                HumanUiHtml.LabelWithClass(id, "human-form-label", HumanUiHtml.Text(label)),
                HumanUiHtml.Input(new HumanUiInputSpec(
                    name,
                    Id: id,
                    Placeholder: placeholder,
                    Required: required,
                    MaxLength: maxLength))));

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

    public static string PrimarySubmit(string label = "Save") =>
        HumanUiHtml.Tag(
            "div",
            "human-form-actions page-actions",
            HumanUiHtml.Tag(
                "button",
                "btn btn-primary",
                [new HumanUiHtml.HumanUiHtmlAttr("type", "submit")],
                [HumanUiHtml.Text(label)]));

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
