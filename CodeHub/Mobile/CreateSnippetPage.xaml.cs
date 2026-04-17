using Mobile.Services;
using Mobile.Models;

namespace Mobile;

public partial class CreateSnippetPage : ContentPage
{
    private DatabaseService _db;
    private AiService _ai = new AiService();

    private string _currentLanguage = "csharp";

    public CreateSnippetPage()
    {
        InitializeComponent();
        _db = new DatabaseService();

        LanguagePicker.ItemsSource = LanguageOptions.SupportedLanguages;
        LanguagePicker.SelectedItem = "csharp";
    }

    private async void OnGenerateClicked(object sender, EventArgs e)
    {
        var prompt = PromptEntry.Text;

        if (string.IsNullOrWhiteSpace(prompt))
            return;

        GenerateButton.IsEnabled = false;

        try
        {
            var result = await _ai.GenerateSnippet(prompt, LanguagePicker.SelectedItem as string);

            ContentEditor.Text = result;

            //_currentLanguage = LanguageOptions.Normalize(result.Language.ToLower());
            //if (LanguagePicker.ItemsSource.Contains(_currentLanguage))
            //{
            //    LanguagePicker.SelectedItem = _currentLanguage;
            //}
        }
        catch
        {
            await DisplayAlert("Error", "Generation Failed. Try again.", "OK");
        }
        finally
        {
            GenerateButton.IsEnabled = true;
        }
    }
    private void OnLanguageChanged(object sender, EventArgs e)
    {
        if (LanguagePicker.SelectedItem is string lang)
        {
            _currentLanguage = LanguageOptions.Normalize(lang);
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        var code = ContentEditor.Text;

        if (string.IsNullOrWhiteSpace(TitleEntry.Text) || string.IsNullOrWhiteSpace(code))
        {
            await DisplayAlert("Error", "Title and content cannot be empty.", "OK");
            return;
        }

        if (ContentEditor.Text.Contains("ERROR"))
        {
            await DisplayAlert("Error", "Code generation failed.", "OK");
            return;
        }
    

        var snippet = new Snippet
        {
            Title = TitleEntry.Text,
            Content = code,
            Language = _currentLanguage
        };

        await _db.AddSnippet(snippet);

        await DisplayAlert("Success", "Snippet saved!", "OK");

        await Navigation.PopAsync();
    }

    private async void RemoveCreatePageClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}