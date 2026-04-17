using Mobile.Models;
using System.Text;
using System.Text.Json;

namespace Mobile.Services;

public class AiService
{
    private readonly HttpClient _http = new();

    private const string ApiKey = "YOUR_API_KEY_HERE";
    private const string Model = "gemini-2.5-flash";

    private readonly string url;

    public AiService()
    {
        url = $"https://generativelanguage.googleapis.com/v1/models/{Model}:generateContent?key={ApiKey}";
    }

    public async Task<string> GenerateSnippet(string prompt, string language)
    {
        var requestBody = new
        {
            contents = new[]
            {
            new
            {
                parts = new[]
                {
                    new
                    {
                        text = $@"
                            You are a code generator.

                            Return ONLY code.
                            No explanations.
                            No markdown.
                            No JSON.

                            Language: {language}

                            Task:
                            {prompt}
                            "
                    }
                }
            }
        }
        };

        var json = JsonSerializer.Serialize(requestBody);

        var response = await _http.PostAsync(
            url,
            new StringContent(json, Encoding.UTF8, "application/json"));

        var result = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception($"API error: {response.StatusCode}\n{result}");
        }

        using var doc = JsonDocument.Parse(result);

        var text = doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString();

        return text ?? "ERROR: empty response";
    }

    public async Task<string> ExplainCode(string code)
    {
        var requestBody = new
        {
            contents = new[]
            {
                new
                {
                    parts = new[]
                    {
                        new
                        {
                            text = $@"
                                        You are a code explainer.

                                        IMPORTANT RULES:
                                        - Return ONLY plain text
                                        - Do NOT use markdown
                                        - Do NOT use **, __, -, or bullet points
                                        - Do NOT format text in any special way
                                        - Do NOT use code blocks
                                        - Write normal sentences only

                                        Explain this code in a simple way:

                                        {code}
                                        "
                        }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(requestBody);

        var response = await _http.PostAsync(
            url,
            new StringContent(json, Encoding.UTF8, "application/json"));

        var result = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return $"ERROR: {response.StatusCode}\n{result}";
        }

        using var doc = JsonDocument.Parse(result);

        return doc.RootElement
            .GetProperty("candidates")[0]
            .GetProperty("content")
            .GetProperty("parts")[0]
            .GetProperty("text")
            .GetString()
            ?? "ERROR: Empty response";
    }

}