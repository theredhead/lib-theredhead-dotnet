using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace theredhead.avalonia.ui;

public class PaneSplitter : TemplatedControl
{
    // Primary / Secondary content
    public static readonly StyledProperty<object?> PrimaryContentProperty =
        AvaloniaProperty.Register<PaneSplitter, object?>(nameof(PrimaryContent));

    public static readonly StyledProperty<object?> SecondaryContentProperty =
        AvaloniaProperty.Register<PaneSplitter, object?>(nameof(SecondaryContent));

    // Orientation
    public static readonly StyledProperty<Orientation> OrientationProperty =
        AvaloniaProperty.Register<PaneSplitter, Orientation>(nameof(Orientation), Orientation.Horizontal);

    // Ratio between 0.0 and 1.0 (0.5 = equal split)
    public static readonly StyledProperty<double> RatioProperty =
        AvaloniaProperty.Register<PaneSplitter, double>(nameof(Ratio), 0.5, coerce: (_, v) =>
        {
            if (double.IsNaN(v) || double.IsInfinity(v)) return 0.5;
            if (v < 0.0) return 0.0;
            if (v > 1.0) return 1.0;
            return v;
        });

    // Thickness of the draggable bar
    public static readonly StyledProperty<double> SplitterThicknessProperty =
        AvaloniaProperty.Register<PaneSplitter, double>(nameof(SplitterThickness), 4.0, coerce: (_, v) => v < 1 ? 1 : v);

    // Bindable GridLengths the template uses (computed from Ratio + SplitterThickness)
    public static readonly StyledProperty<GridLength> FirstLengthProperty =
        AvaloniaProperty.Register<PaneSplitter, GridLength>(nameof(FirstLength), new GridLength(1, GridUnitType.Star));

    public static readonly StyledProperty<GridLength> SecondLengthProperty =
        AvaloniaProperty.Register<PaneSplitter, GridLength>(nameof(SecondLength), new GridLength(1, GridUnitType.Star));

    public static readonly StyledProperty<GridLength> SplitterLengthProperty =
        AvaloniaProperty.Register<PaneSplitter, GridLength>(nameof(SplitterLength), new GridLength(4, GridUnitType.Pixel));

    private Grid? _grid;
    private bool _updatingFromGrid;

    static PaneSplitter()
    {
        // Recompute GridLengths when these change
        RatioProperty.Changed.AddClassHandler<PaneSplitter>((s, _) => s.UpdateLengthsFromRatio());
        SplitterThicknessProperty.Changed.AddClassHandler<PaneSplitter>((s, _) => s.UpdateLengthsFromRatio());
        OrientationProperty.Changed.AddClassHandler<PaneSplitter>((s, _) => s.UpdateLengthsFromRatio());
    }

    public object? PrimaryContent
    {
        get => GetValue(PrimaryContentProperty);
        set => SetValue(PrimaryContentProperty, value);
    }

    public object? SecondaryContent
    {
        get => GetValue(SecondaryContentProperty);
        set => SetValue(SecondaryContentProperty, value);
    }

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public double Ratio
    {
        get => GetValue(RatioProperty);
        set => SetValue(RatioProperty, value);
    }

    public double SplitterThickness
    {
        get => GetValue(SplitterThicknessProperty);
        set => SetValue(SplitterThicknessProperty, value);
    }

    public GridLength FirstLength
    {
        get => GetValue(FirstLengthProperty);
        private set => SetValue(FirstLengthProperty, value);
    }

    public GridLength SecondLength
    {
        get => GetValue(SecondLengthProperty);
        private set => SetValue(SecondLengthProperty, value);
    }

    public GridLength SplitterLength
    {
        get => GetValue(SplitterLengthProperty);
        private set => SetValue(SplitterLengthProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_grid is not null)
        {
            _grid.GetObservable(BoundsProperty).Subscribe(_ => { }); // detach if you had subscribed
            _grid = null;
        }

        _grid = e.NameScope.Find<Grid>("PART_Grid");
        if (_grid != null)
        {
            // Recompute ratio when user drags the GridSplitter
            _grid.LayoutUpdated += (_, __) => UpdateRatioFromActuals();
        }

        UpdateLengthsFromRatio();
    }

    private void UpdateLengthsFromRatio()
    {
        // Convert Ratio -> GridLength stars and apply splitter thickness
        var first = Ratio <= 0 ? 0 : Ratio;
        var second = 1 - first;

        FirstLength = new GridLength(first <= 0 ? 0 : first, GridUnitType.Star);
        SecondLength = new GridLength(second <= 0 ? 0 : second, GridUnitType.Star);
        SplitterLength = new GridLength(SplitterThickness, GridUnitType.Pixel);
    }

    private void UpdateRatioFromActuals()
    {
        if (_grid == null) return;
        if (_updatingFromGrid) return;

        try
        {
            _updatingFromGrid = true;

            if (Orientation == Orientation.Horizontal && _grid.ColumnDefinitions.Count >= 3)
            {
                var a = _grid.ColumnDefinitions[0].ActualWidth;
                var b = _grid.ColumnDefinitions[2].ActualWidth;
                var sum = a + b;
                if (sum > 0) Ratio = a / sum;
            }
            else if (Orientation == Orientation.Vertical && _grid.RowDefinitions.Count >= 3)
            {
                var a = _grid.RowDefinitions[0].ActualHeight;
                var b = _grid.RowDefinitions[2].ActualHeight;
                var sum = a + b;
                if (sum > 0) Ratio = a / sum;
            }
        }
        finally
        {
            _updatingFromGrid = false;
        }
    }
}
