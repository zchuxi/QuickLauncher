using Microsoft.UI.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using QuickLauncher.Models;
using Windows.System;

namespace QuickLauncher.Controls;

/// <summary>
/// 中心分类切换器：圆环 + 中心文字，支持鼠标滚轮切换分类。
/// </summary>
public sealed class CategoryRing : Control
{
    /// <summary>当前分类。双向绑定到 ViewModel。</summary>
    public static readonly DependencyProperty CategoryProperty =
        DependencyProperty.Register(nameof(Category), typeof(AppCategory), typeof(CategoryRing),
            new PropertyMetadata(AppCategory.Favorites, OnCategoryChanged));

    /// <summary>可选分类列表。</summary>
    public static readonly DependencyProperty CategoriesProperty =
        DependencyProperty.Register(nameof(Categories), typeof(object), typeof(CategoryRing),
            new PropertyMetadata(null, OnCategoryChanged));

    public AppCategory Category
    {
        get => (AppCategory)GetValue(CategoryProperty);
        set => SetValue(CategoryProperty, value);
    }

    /// <summary>分类集合（支持任意 IEnumerable&lt;AppCategory&gt;）。</summary>
    public object Categories
    {
        get => GetValue(CategoriesProperty);
        set => SetValue(CategoriesProperty, value);
    }

    private TextBlock? _iconPart;
    private TextBlock? _namePart;
    private System.Collections.Generic.IList<AppCategory>? _categoryList;

    public CategoryRing()
    {
        this.DefaultStyleKey = typeof(CategoryRing);
    }

    protected override void OnApplyTemplate()
    {
        base.OnApplyTemplate();
        _iconPart = GetTemplateChild("PART_Icon") as TextBlock;
        _namePart = GetTemplateChild("PART_Name") as TextBlock;
        RefreshCategoryList();
        UpdateVisual();
    }

    private static void OnCategoryChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is CategoryRing ring)
        {
            ring.RefreshCategoryList();
            ring.UpdateVisual();
        }
    }

    private void RefreshCategoryList()
    {
        _categoryList = Categories switch
        {
            System.Collections.Generic.IList<AppCategory> list => list,
            System.Collections.Generic.IEnumerable<AppCategory> en => new System.Collections.Generic.List<AppCategory>(en),
            _ => null,
        };
    }

    private void UpdateVisual()
    {
        if (_iconPart is not null)
        {
            _iconPart.Text = Category.GetIconGlyph();
        }
        if (_namePart is not null)
        {
            _namePart.Text = Category.GetDisplayName();
        }
    }

    /// <summary>滚轮切换分类。</summary>
    protected override void OnPointerWheelChanged(PointerRoutedEventArgs e)
    {
        base.OnPointerWheelChanged(e);
        if (_categoryList is null || _categoryList.Count == 0)
        {
            return;
        }

        var delta = e.GetCurrentPoint(this).Properties.MouseWheelDelta;
        var idx = _categoryList.IndexOf(Category);
        if (idx < 0) idx = 0;

        if (delta > 0)
        {
            idx = (idx + 1) % _categoryList.Count;
        }
        else
        {
            idx = (idx - 1 + _categoryList.Count) % _categoryList.Count;
        }

        Category = _categoryList[idx];
        e.Handled = true;
    }
}
