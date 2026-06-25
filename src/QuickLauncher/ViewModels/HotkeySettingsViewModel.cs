using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using QuickLauncher.Data;
using QuickLauncher.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace QuickLauncher.ViewModels;

/// <summary>
/// 快捷键设置视图模型。
/// </summary>
public partial class HotkeySettingsViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableCollection<HotkeyConfig> _hotkeys = new(MockData.Hotkeys);

    /// <summary>是否处于新增录入状态。</summary>
    [ObservableProperty]
    private bool _isAdding;

    /// <summary>录入中的按键显示串。</summary>
    [ObservableProperty]
    private string _newKeyDisplay = string.Empty;

    /// <summary>录入中选择的应用。</summary>
    [ObservableProperty]
    private AppInfo? _selectedApp;

    /// <summary>可选应用列表。</summary>
    public ObservableCollection<AppInfo> AvailableApps { get; } = new(MockData.Apps);

    /// <summary>可选动作列表。</summary>
    public ObservableCollection<HotkeyAction> AvailableActions { get; } = new()
    {
        HotkeyAction.ShowPanel,
        HotkeyAction.HidePanel,
        HotkeyAction.LaunchApp,
        HotkeyAction.SwitchCategory,
    };

    [ObservableProperty]
    private HotkeyAction _newAction = HotkeyAction.LaunchApp;

    public HotkeySettingsViewModel()
    {
    }

    /// <summary>显示新增录入区。</summary>
    [RelayCommand]
    private void StartAdd()
    {
        IsAdding = true;
        NewKeyDisplay = string.Empty;
        SelectedApp = null;
        NewAction = HotkeyAction.LaunchApp;
    }

    /// <summary>取消新增。</summary>
    [RelayCommand]
    private void CancelAdd() => IsAdding = false;

    /// <summary>确认新增快捷键。</summary>
    [RelayCommand]
    private void ConfirmAdd()
    {
        if (string.IsNullOrWhiteSpace(NewKeyDisplay))
        {
            return;
        }

        var config = new HotkeyConfig
        {
            DisplayKey = NewKeyDisplay,
            Action = NewAction,
            AppName = NewAction == HotkeyAction.LaunchApp ? SelectedApp?.Name : null,
            AppIconGlyph = SelectedApp?.IconGlyph ?? "\uE7B8",
        };
        Hotkeys.Add(config);
        IsAdding = false;
        NewKeyDisplay = string.Empty;
        SelectedApp = null;
    }

    /// <summary>删除快捷键。</summary>
    [RelayCommand]
    private void DeleteHotkey(HotkeyConfig? config)
    {
        if (config is not null)
        {
            Hotkeys.Remove(config);
        }
    }

    /// <summary>记录按键（由 View 的 KeyDown 调用）。</summary>
    public void RecordKey(string displayKey) => NewKeyDisplay = displayKey;
}
