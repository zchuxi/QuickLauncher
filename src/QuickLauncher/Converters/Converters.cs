using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using QuickLauncher.Models;

namespace QuickLauncher.Converters;

/// <summary>布尔转可见性。</summary>
public sealed class BoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
        => value is bool b && b ? Visibility.Visible : Visibility.Collapsed;

    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => value is Visibility v && v == Visibility.Visible;
}

/// <summary>布尔取反转可见性。</summary>
public sealed class NegationBoolToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
        => value is bool b && !b ? Visibility.Visible : Visibility.Collapsed;

    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => value is Visibility v && v != Visibility.Visible;
}

/// <summary>分类枚举转图标 glyph。</summary>
public sealed class CategoryToIconConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
        => value is Models.AppCategory c ? c.GetIconGlyph() : "\uE7B8";

    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => throw new System.NotImplementedException();
}

/// <summary>分类枚举转显示名。</summary>
public sealed class CategoryToNameConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
        => value is Models.AppCategory c ? c.GetDisplayName() : string.Empty;

    public object ConvertBack(object value, Type targetType, object parameter, string language)
        => throw new System.NotImplementedException();
}
