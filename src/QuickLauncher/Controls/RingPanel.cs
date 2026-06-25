using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace QuickLauncher.Controls;

/// <summary>
/// 环形布局面板。将子元素沿环形轨迹等距排列。
/// </summary>
public sealed class RingPanel : Panel
{
    /// <summary>环半径（像素）。</summary>
    public static readonly DependencyProperty RadiusProperty =
        DependencyProperty.Register(nameof(Radius), typeof(double), typeof(RingPanel),
            new PropertyMetadata(160.0, OnLayoutPropertyChanged));

    /// <summary>起始角度（度，0=右，90=下，逆时针为负）。默认从顶部 -90° 开始。</summary>
    public static readonly DependencyProperty StartAngleProperty =
        DependencyProperty.Register(nameof(StartAngle), typeof(double), typeof(RingPanel),
            new PropertyMetadata(-90.0, OnLayoutPropertyChanged));

    /// <summary>扫过角度（度）。默认 360 完整环。</summary>
    public static readonly DependencyProperty SweepAngleProperty =
        DependencyProperty.Register(nameof(SweepAngle), typeof(double), typeof(RingPanel),
            new PropertyMetadata(360.0, OnLayoutPropertyChanged));

    public double Radius
    {
        get => (double)GetValue(RadiusProperty);
        set => SetValue(RadiusProperty, value);
    }

    public double StartAngle
    {
        get => (double)GetValue(StartAngleProperty);
        set => SetValue(StartAngleProperty, value);
    }

    public double SweepAngle
    {
        get => (double)GetValue(SweepAngleProperty);
        set => SetValue(SweepAngleProperty, value);
    }

    private static void OnLayoutPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is RingPanel panel)
        {
            panel.InvalidateMeasure();
            panel.InvalidateArrange();
        }
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        double maxChildW = 0, maxChildH = 0;
        foreach (var child in Children)
        {
            child.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
            maxChildW = Math.Max(maxChildW, child.DesiredSize.Width);
            maxChildH = Math.Max(maxChildH, child.DesiredSize.Height);
        }

        // 所需尺寸 = 2 * (半径 + 最大子元素半边)
        var diameter = 2.0 * (Radius + Math.Max(maxChildW, maxChildH) / 2.0);
        var size = Math.Ceiling(diameter);
        return new Size(size, size);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        if (Children.Count == 0)
        {
            return finalSize;
        }

        var cx = finalSize.Width / 2.0;
        var cy = finalSize.Height / 2.0;
        var count = Children.Count;

        // 单个元素时直接放中心上方
        var sweep = count == 1 ? 0.0 : SweepAngle;
        var step = count > 1 ? sweep / (count - (SweepAngle >= 360 ? 0 : 1)) : 0.0;
        // 完整环时均匀分布（首尾不重叠）
        if (SweepAngle >= 360)
        {
            step = sweep / count;
        }

        for (int i = 0; i < count; i++)
        {
            var angleDeg = StartAngle + i * step;
            var angleRad = angleDeg * Math.PI / 180.0;
            var x = cx + Radius * Math.Cos(angleRad);
            var y = cy + Radius * Math.Sin(angleRad);

            var child = Children[i];
            var w = child.DesiredSize.Width;
            var h = child.DesiredSize.Height;
            // 以元素中心对齐到圆周点
            var rect = new Windows.Foundation.Rect(x - w / 2.0, y - h / 2.0, w, h);
            child.Arrange(rect);
        }

        return finalSize;
    }
}
