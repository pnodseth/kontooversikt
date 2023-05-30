using BlazorApp.Data.Models;
using Microsoft.AspNetCore.Components;

namespace BlazorApp.Store;

public class AppState
{
    private Children _currentChild;
    private int _availableImre;
    private int _availableKlara;
    private bool _loading;


    public Children CurrentChild
    {
        get => _currentChild;
        set
        {
            _currentChild = value;
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

    public bool Loading
    {
        get => _loading;
        set
        {
            _loading = value;
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
        var name = Char.ToUpper(path[0]) + path.Substring(1);
        if (name == "Imre")
        {
            CurrentChild = Children.Imre;
        } else if (name == "Klara")
        {
            CurrentChild = Children.Klara;
        }
    }

    public string GetNameString()
    {
        return CurrentChild == Children.Imre ? "Imre" : "Klara";
    }

    public Children GetChildFromName(string name)
    {
        if (name == "Imre")
        {
            return Children.Imre;
        }

        return Children.Klara;
    }
}