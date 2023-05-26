using Microsoft.AspNetCore.Components;

namespace BlazorApp.Store;

public class AppState
{
    private string _currentName = "";
    private int _availableImre;
    private int _availableKlara;


    public string CurrentName
    {
        get => _currentName;
        set
        {
            _currentName = value;
            NotifyStateChanged();
        } 
    }

    public int AvailableImre
    {
        get => _availableImre;
        set
        {
            _availableImre = value;
            NotifyStateChanged();
        }
    }

    public int AvailableKlara
    {
        get => _availableKlara;
        set
        {
            _availableKlara = value;
            NotifyStateChanged();
        }
    }

    public event Action? OnChange;

    private void NotifyStateChanged() => OnChange?.Invoke();

    public void SetCurrentNameFromUri(NavigationManager navigationManager)
    {
        var uri = new Uri(navigationManager.Uri);
        var path = uri.AbsolutePath;
        path = path.TrimStart('/');
        CurrentName = Char.ToUpper(path[0]) + path.Substring(1);
    }
}