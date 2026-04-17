using Mobile.Services;
using Mobile.Models;
using System.Threading.Tasks;

namespace Mobile;

public partial class MainPage : ContentPage
{
    private DatabaseService _db;

    public MainPage()
    {
        InitializeComponent();
        _db = new DatabaseService();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadSnippets();
    }

    private async void LoadSnippets()
    {
        var snippets = await _db.GetSnippets();
        SnippetsList.ItemsSource = snippets;
    }

    private async void OnCreateClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateSnippetPage());
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var snippetToRemove = (Snippet)button.CommandParameter;

        var confirm = await DisplayAlert("Delete", "Delete this snippet?", "Delete", "Cancel");
        if (!confirm)
            return;

        await _db.DeleteSnippet(snippetToRemove);

        LoadSnippets();
    }

    private async void OnViewClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var selectedSnippet = (Snippet)button.CommandParameter;

        await Navigation.PushAsync(new SnippetDetailsPage(selectedSnippet));
    }
}