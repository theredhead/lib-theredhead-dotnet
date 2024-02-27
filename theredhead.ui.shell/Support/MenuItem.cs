using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using theredhead.ui.shell.ViewModels;

namespace theredhead.ui.shell.Support;

public partial class MenuItem
{
    public MenuItem(ViewModelBase viewModel, string? label = null)
    {
        ViewModel = viewModel;
        Label = label ?? ViewModel.ShortTitle;
        Icon = viewModel.Icon;
    }
    public StreamGeometry Icon { get; }
    public string Label { get; }
    public ViewModelBase ViewModel { get; }

}
