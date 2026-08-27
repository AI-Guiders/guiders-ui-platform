using System.Reflection;

namespace AIGuiders.UI.Tokens;

public static class HumanUiTokensCss
{
    private static readonly Lazy<string> Css = new(Load);

    public static string Content => Css.Value;

    private static string Load()
    {
        var assembly = typeof(HumanUiTokensCss).Assembly;
        const string resourceName = "AIGuiders.UI.Tokens.wwwroot.aiguiders-ui-tokens.css";
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"{resourceName} embedded resource missing");
        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
