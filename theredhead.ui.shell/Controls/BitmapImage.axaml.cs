using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Media.Imaging;
using Avalonia.Platform;

namespace theredhead.ui.shell.Controls;

public class BitmapImage : TemplatedControl
{
    private string _asset = "theredhead.png";
    public string Asset
    {
        get => _asset;
        set
        {
            _asset = value;
            _bitmap = null;
        }
    }

    private Bitmap? _bitmap;
    public Bitmap Image
    {
        get
        {
            if (_bitmap == null)
            {
                var resourceUri = new Uri("avares://theredhead.ui.shell/Assets/" + Asset);
                _bitmap = new Bitmap(AssetLoader.Open(resourceUri));
            }

            return _bitmap;
        }
    }

    private int? _width;
    public int BitmapWidth
    {
        get => _width ?? Convert.ToInt32(Image.Size.Width);
        set => _width = value;
    }
    private int? _height;
    public int BitmapHeight
    {
        get => _height ?? Convert.ToInt32(Image.Size.Height);
        set => _height = value;
    }
}
