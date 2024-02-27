using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace theredhead.ui.shell.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    [ObservableProperty] private string _title = "Untitled ViewModel";
    [ObservableProperty] private string _shortTitle = "Untitled ViewModel";
    [ObservableProperty] private StreamGeometry _icon;

    public string IconName
    {
        set => SetIconByResourceName(value);
    }

    protected void SetIconByResourceName(string name)
    {
        if (Application.Current.TryFindResource(name, out var icon))
        {
            if (icon is StreamGeometry geometry)
            {
                Icon = geometry;
            }
        }
    }

    public ViewModelBase()
    {
        IconName = "Home";
    }
}
