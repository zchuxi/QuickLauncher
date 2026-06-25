using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using QuickLauncher.ViewModels;
using Windows.System;

namespace QuickLauncher.Views;

/// <summary>
/// 快捷键设置对话框。
/// </summary>
public sealed partial class HotkeySettingsDialog : ContentDialog
{
    public HotkeySettingsViewModel ViewModel { get; }

    public HotkeySettingsDialog()
    {
        this.InitializeComponent();
        ViewModel = new HotkeySettingsViewModel();
    }

    /// <summary>录制按键：将按键组合转为显示串。</summary>
    private void KeyRecorderBox_KeyDown(object sender, KeyRoutedEventArgs e)
    {
        var key = e.Key;
        // 忽略纯修饰键
        if (key is VirtualKey.Shift or VirtualKey.Control or VirtualKey.Menu or VirtualKey.LeftWindows or VirtualKey.RightWindows)
        {
            return;
        }

        var parts = new System.Collections.Generic.List<string>();
        var ctrl = Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Control);
        var alt = Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Menu);
        var shift = Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.Shift);
        var win = Microsoft.UI.Input.InputKeyboardSource.GetKeyStateForCurrentThread(VirtualKey.LeftWindows);

        if (ctrl.HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down)) parts.Add("Ctrl");
        if (alt.HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down)) parts.Add("Alt");
        if (shift.HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down)) parts.Add("Shift");
        if (win.HasFlag(Windows.UI.Core.CoreVirtualKeyStates.Down)) parts.Add("Win");

        parts.Add(KeyToString(key));
        ViewModel.RecordKey(string.Join("+", parts));
        e.Handled = true;
    }

    private static string KeyToString(VirtualKey key) => key switch
    {
        >= VirtualKey.Number0 and <= VirtualKey.Number9 => ((int)key - (int)VirtualKey.Number0).ToString(),
        >= VirtualKey.NumberPad0 and <= VirtualKey.NumberPad9 => $"Num{key - VirtualKey.NumberPad0}",
        VirtualKey.Space => "Space",
        VirtualKey.Enter => "Enter",
        VirtualKey.Escape => "Esc",
        VirtualKey.Tab => "Tab",
        _ => key.ToString(),
    };
}
