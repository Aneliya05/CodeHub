using SQLite;
using Mobile.Models;

namespace Mobile.Services;

public class DatabaseService
{
    private SQLiteAsyncConnection _database;

    public async Task Init()
    {
        if (_database != null)
            return;

        var dbPath = Path.Combine(FileSystem.AppDataDirectory, "snippets.db");

        _database = new SQLiteAsyncConnection(dbPath);

        await _database.CreateTableAsync<Snippet>();
    }

    public async Task<List<Snippet>> GetSnippets()
    {
        await Init();
        return await _database.Table<Snippet>().ToListAsync();
    }

    public async Task AddSnippet(Snippet snippet)
    {
        await Init();
        await _database.InsertAsync(snippet);
    }   
    public async Task DeleteSnippet(Snippet snippet)
    {
        await Init();
        await _database.DeleteAsync(snippet);
    }
    public async Task UpdateSnippet(Snippet snippet)
    {
        await Init();
        await _database.UpdateAsync(snippet);
    }
}