using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickLauncher.Data;
using QuickLauncher.Models;
using System.Collections.ObjectModel;

namespace QuickLauncher.ViewModels;

/// <summary>
/// 设置面板视图模型。
/// </summary>
public partial class SettingsViewModel : ObservableObject
{
    /// <summary>设置数据（直接绑定 UI）。</summary>
    [ObservableProperty]
    private SettingsModel _settings = CloneSettings();

    /// <summary>当前选中的导航项。</summary>
    [ObservableProperty]
    private string _selectedSection = "外观";

    /// <summary>导航项集合（显示名）。</summary>
    public ObservableCollection<string> Sections { get; } = new()
    {
        "外观", "快捷键", "鼠标手势", "高级", "关于",
    };

    public SettingsViewModel()
    {
    }

    // 各分组可见性（跟随 SelectedSection）
    public bool IsAppearance => SelectedSection == "外观";
    public bool IsHotkey => SelectedSection == "快捷键";
    public bool IsMouseGesture => SelectedSection == "鼠标手势";
    public bool IsAdvanced => SelectedSection == "高级";
    public bool IsAbout => SelectedSection == "关于";

    partial void OnSelectedSectionChanged(string value)
    {
        OnPropertyChanged(nameof(IsAppearance));
        OnPropertyChanged(nameof(IsHotkey));
        OnPropertyChanged(nameof(IsMouseGesture));
        OnPropertyChanged(nameof(IsAdvanced));
        OnPropertyChanged(nameof(IsAbout));
    }

    /// <summary>切换导航分组。</summary>
    [RelayCommand]
    private void SelectSection(string section) => SelectedSection = section;

    /// <summary>重置为默认设置。</summary>
    [RelayCommand]
    private void ResetToDefault() => Settings = CloneSettings();

    private static SettingsModel CloneSettings() => new()
    {
        Theme = MockData.Settings.Theme,
        PanelOpacity = MockData.Settings.PanelOpacity,
        AnimationsEnabled = MockData.Settings.AnimationsEnabled,
        TriggerHotkey = MockData.Settings.TriggerHotkey,
        StartWithWindows = MockData.Settings.StartWithWindows,
        MinimizeToTray = MockData.Settings.MinimizeToTray,
        ShowInTaskbar = MockData.Settings.ShowInTaskbar,
        AppVersion = MockData.Settings.AppVersion,
        EdgeGestures = new(System.Linq.Enumerable.Select(
            MockData.Settings.EdgeGestures, e => new EdgeGestureSetting
            {
                Edge = e.Edge,
                Enabled = e.Enabled,
                Threshold = e.Threshold,
                Action = e.Action,
            })),
    };
}
