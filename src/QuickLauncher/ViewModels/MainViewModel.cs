using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickLauncher.Data;
using QuickLauncher.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace QuickLauncher.ViewModels;

/// <summary>
/// 主面板（环形启动器）视图模型。
/// </summary>
public partial class MainViewModel : ObservableObject
{
    /// <summary>可循环切换的分类列表。</summary>
    public ObservableCollection<AppCategory> Categories { get; } = new()
    {
        AppCategory.Favorites,
        AppCategory.Development,
        AppCategory.Design,
        AppCategory.Productivity,
        AppCategory.Communication,
        AppCategory.Entertainment,
        AppCategory.System,
        AppCategory.Other,
    };

    /// <summary>当前分类下的应用（外环图标）。</summary>
    [ObservableProperty]
    private ObservableCollection<AppInfo> _ringApps = new();

    [ObservableProperty]
    private AppCategory _currentCategory = AppCategory.Favorites;

    [ObservableProperty]
    private string _searchQuery = string.Empty;

    /// <summary>最近一次启动的应用名称（用于状态提示）。</summary>
    [ObservableProperty]
    private string _statusMessage = string.Empty;

    [ObservableProperty]
    private bool _isStatusVisible;

    public MainViewModel()
    {
        RefreshRingApps();
    }

    partial void OnCurrentCategoryChanged(AppCategory value) => RefreshRingApps();

    partial void OnSearchQueryChanged(string value) => RefreshRingApps();

    /// <summary>按当前分类与搜索词刷新外环应用。</summary>
    private void RefreshRingApps()
    {
        var source = MockData.GetByCategory(CurrentCategory);
        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            source = source.Where(a => a.Name.Contains(SearchQuery, System.StringComparison.OrdinalIgnoreCase));
        }
        // 外环最多显示 10 个
        RingApps = new ObservableCollection<AppInfo>(source.Take(10));
    }

    /// <summary>切换到下一个分类（顺时针）。</summary>
    [RelayCommand]
    private void NextCategory()
    {
        var idx = Categories.IndexOf(CurrentCategory);
        CurrentCategory = Categories[(idx + 1) % Categories.Count];
    }

    /// <summary>切换到上一个分类（逆时针）。</summary>
    [RelayCommand]
    private void PreviousCategory()
    {
        var idx = Categories.IndexOf(CurrentCategory);
        CurrentCategory = Categories[(idx - 1 + Categories.Count) % Categories.Count];
    }

    /// <summary>启动应用（原型级：仅更新状态提示）。</summary>
    [RelayCommand]
    private void LaunchApp(AppInfo? app)
    {
        if (app is null) return;
        app.LaunchCount++;
        app.LastUsed = System.DateTimeOffset.Now;
        StatusMessage = $"已启动 {app.Name}（原型）";
        IsStatusVisible = true;
    }
}
