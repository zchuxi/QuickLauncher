using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickLauncher.Data;
using QuickLauncher.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace QuickLauncher.ViewModels;

/// <summary>
/// 应用浏览器视图模型。
/// </summary>
public partial class AppBrowserViewModel : ObservableObject
{
    private readonly ObservableCollection<AppInfo> _allApps = new(MockData.Apps);

    /// <summary>筛选后的应用列表。</summary>
    [ObservableProperty]
    private ObservableCollection<AppInfo> _filteredApps = new(MockData.Apps);

    [ObservableProperty]
    private AppCategory _selectedCategory = AppCategory.All;

    [ObservableProperty]
    private string _searchQuery = string.Empty;

    /// <summary>分类标签集合。</summary>
    public ObservableCollection<AppCategory> CategoryTabs { get; } = new()
    {
        AppCategory.All,
        AppCategory.Development,
        AppCategory.Design,
        AppCategory.Productivity,
        AppCategory.Communication,
        AppCategory.Entertainment,
        AppCategory.System,
        AppCategory.Other,
    };

    /// <summary>当前选中的应用（用于"选择"按钮）。</summary>
    [ObservableProperty]
    private AppInfo? _selectedApp;

    public AppBrowserViewModel()
    {
        ApplyFilter();
    }

    partial void OnSelectedCategoryChanged(AppCategory value) => ApplyFilter();

    partial void OnSearchQueryChanged(string value) => ApplyFilter();

    /// <summary>应用搜索 + 分类筛选。</summary>
    private void ApplyFilter()
    {
        var source = _allApps.AsEnumerable();
        if (SelectedCategory != AppCategory.All)
        {
            source = SelectedCategory switch
            {
                AppCategory.Favorites => source.Where(a => a.IsFavorite),
                AppCategory.Recent => source.OrderByDescending(a => a.LastUsed),
                _ => source.Where(a => a.Category == SelectedCategory),
            };
        }

        if (!string.IsNullOrWhiteSpace(SearchQuery))
        {
            source = source.Where(a => a.Name.Contains(SearchQuery, System.StringComparison.OrdinalIgnoreCase));
        }

        FilteredApps = new ObservableCollection<AppInfo>(source.ToList());
    }

    /// <summary>切换分类标签。</summary>
    [RelayCommand]
    private void SelectCategory(AppCategory category) => SelectedCategory = category;
}
