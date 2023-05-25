using Microsoft.AspNetCore.Components;

namespace BlazorApp.Store;

public class AppState
{
    public string CurrentName { get; set; } = "";

    public void SetCurrentNameFromUri(NavigationManager navigationManager)
    {
        var uri = new Uri(navigationManager.Uri);
        var path = uri.AbsolutePath;
        path = path.TrimStart('/');
        CurrentName = Char.ToUpper(path[0]) + path.Substring(1);
    }
}