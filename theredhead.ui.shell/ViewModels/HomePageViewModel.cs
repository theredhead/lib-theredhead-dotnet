using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace theredhead.ui.shell.ViewModels;

public partial class HomePageViewModel : ViewModelBase
{
    public HomePageViewModel()
    {
        Title = "Home";
        ShortTitle = "Home";
        IconName = "Home";
    }
}

