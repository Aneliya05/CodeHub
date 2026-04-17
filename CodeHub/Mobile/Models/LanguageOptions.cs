using System;
using System.Collections.Generic;
using System.Linq;

namespace Mobile.Models;

public static class LanguageOptions
{
    private static readonly Dictionary<string, string> _map = new(StringComparer.OrdinalIgnoreCase)
    {
        // C-family
        ["c#"] = "csharp",
        ["cs"] = "csharp",
        ["csharp"] = "csharp",
        ["c++"] = "cpp",
        ["cpp"] = "cpp",
        ["cplusplus"] = "cpp",
        ["c"] = "c",

        // JavaScript / TypeScript
        ["js"] = "javascript",
        ["javascript"] = "javascript",
        ["node"] = "javascript",
        ["ts"] = "typescript",
        ["typescript"] = "typescript",

        // Common languages
        ["python"] = "python",
        ["py"] = "python",
        ["java"] = "java",
        ["go"] = "go",
        ["golang"] = "go",
        ["rust"] = "rust",
        ["rs"] = "rust",
        ["html"] = "html",
        ["htm"] = "html",
        ["css"] = "css",

        // Others
        ["php"] = "php",
        ["swift"] = "swift",
        ["ruby"] = "ruby",
        ["rb"] = "ruby",
        ["kotlin"] = "kotlin",
        ["sql"] = "sql",
        ["r"] = "r",
        ["dart"] = "dart",
        ["bash"] = "bash",
        ["shell"] = "bash",
        ["sh"] = "bash",
        ["powershell"] = "powershell",
        ["ps1"] = "powershell",
        ["objective-c"] = "objectivec",
        ["objectivec"] = "objectivec",
        ["objc"] = "objectivec",
        ["perl"] = "perl",
        ["lua"] = "lua",
        ["matlab"] = "matlab",
        ["vb"] = "vbnet",
        ["vbnet"] = "vbnet",
        ["json"] = "json",
        ["yaml"] = "yaml",
        ["yml"] = "yaml",
        ["xml"] = "xml",
        ["markdown"] = "markdown",
        ["md"] = "markdown",
    };

    public static string Normalize(string? language)
    {
        if (string.IsNullOrWhiteSpace(language))
            return string.Empty;

        var key = language.Trim().ToLowerInvariant();

        if (_map.TryGetValue(key, out var mapped))
            return mapped;

        return key.Replace("+", "p").Replace(" ", "").Replace("#", "sharp");
    }

    public static List<string> SupportedLanguages => _map.Values.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
}
