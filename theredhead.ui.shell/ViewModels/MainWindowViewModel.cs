using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using theredhead.ui.shell.Support;

namespace theredhead.ui.shell.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isPaneOpen = true;
    [ObservableProperty] private MenuItem _selectedMenuItem;
    [ObservableProperty] private ViewModelBase _currentPage;

    [RelayCommand]
    public void TogglePane()
    {
        IsPaneOpen = !IsPaneOpen;
    }

    partial void OnSelectedMenuItemChanged(MenuItem? value)
    {
        if (value != null)
        {
            CurrentPage = value.ViewModel;
        }
    }

    public ObservableCollection<MenuItem> MenuItems { get;  }  = new()
    {
        new MenuItem(new HomePageViewModel()),
        new MenuItem(new AboutPageViewModel()),
        new MenuItem(new SupportPageViewModel())
    };

    public MainWindowViewModel()
    {
        SelectedMenuItem = MenuItems[0];
    }
}
