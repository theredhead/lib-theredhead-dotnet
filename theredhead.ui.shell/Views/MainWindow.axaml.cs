using Avalonia.Controls;
using FluentAvalonia.UI.Windowing;

namespace theredhead.ui.shell.Views;

public partial class MainWindow : AppWindow
{
    public MainWindow()
    {
        InitializeComponent();

        TitleBar.ExtendsContentIntoTitleBar = true;
        TitleBar.TitleBarHitTestType = TitleBarHitTestType.Complex;
    }
}
