using System;
using System.Windows.Input;
using QuickLauncher.Helpers;
using Windows.System;

namespace QuickLauncher.Helpers;

/// <summary>
/// 快捷键解析工具：在 DisplayKey("Alt+Space") 与 Win32 修饰键/VK 之间转换。
/// </summary>
public static class HotkeyParser
{
    /// <summary>解析显示串为 (modifiers, virtualKey)。</summary>
    public static (uint modifiers, uint vk) Parse(string displayKey)
    {
        if (string.IsNullOrWhiteSpace(displayKey))
        {
            return (0, 0);
        }

        uint mods = 0;
        uint vk = 0;

        var parts = displayKey.Split('+', StringSplitOptions.RemoveEmptyEntries);
        foreach (var raw in parts)
        {
            var p = raw.Trim();
            if (p.Equals("Alt", StringComparison.OrdinalIgnoreCase)) mods |= NativeMethods.MOD_ALT;
            else if (p.Equals("Ctrl", StringComparison.OrdinalIgnoreCase) || p.Equals("Control", StringComparison.OrdinalIgnoreCase)) mods |= NativeMethods.MOD_CONTROL;
            else if (p.Equals("Shift", StringComparison.OrdinalIgnoreCase)) mods |= NativeMethods.MOD_SHIFT;
            else if (p.Equals("Win", StringComparison.OrdinalIgnoreCase) || p.Equals("Super", StringComparison.OrdinalIgnoreCase)) mods |= NativeMethods.MOD_WIN;
            else vk = KeyToVk(p);
        }

        return (mods, vk);
    }

    /// <summary>按键名转 VirtualKey 代码。</summary>
    public static uint KeyToVk(string key) => key.ToUpperInvariant() switch
    {
        "SPACE" => (uint)VirtualKey.Space,
        "ENTER" => (uint)VirtualKey.Enter,
        "ESC" or "ESCAPE" => (uint)VirtualKey.Escape,
        "TAB" => (uint)VirtualKey.Tab,
        "BACK" or "BACKSPACE" => (uint)VirtualKey.Back,
        "DELETE" or "DEL" => (uint)VirtualKey.Delete,
        "INSERT" => (uint)VirtualKey.Insert,
        "HOME" => (uint)VirtualKey.Home,
        "END" => (uint)VirtualKey.End,
        "PAGEUP" => (uint)VirtualKey.PageUp,
        "PAGEDOWN" => (uint)VirtualKey.PageDown,
        "LEFT" => (uint)VirtualKey.Left,
        "RIGHT" => (uint)VirtualKey.Right,
        "UP" => (uint)VirtualKey.Up,
        "DOWN" => (uint)VirtualKey.Down,
        "F1" => (uint)VirtualKey.F1,
        "F2" => (uint)VirtualKey.F2,
        "F3" => (uint)VirtualKey.F3,
        "F4" => (uint)VirtualKey.F4,
        "F5" => (uint)VirtualKey.F5,
        "F6" => (uint)VirtualKey.F6,
        "F7" => (uint)VirtualKey.F7,
        "F8" => (uint)VirtualKey.F8,
        "F9" => (uint)VirtualKey.F9,
        "F10" => (uint)VirtualKey.F10,
        "F11" => (uint)VirtualKey.F11,
        "F12" => (uint)VirtualKey.F12,
        _ when key.Length == 1 && char.IsDigit(key[0]) => (uint)VirtualKey.Number0 + (uint)(key[0] - '0'),
        _ when key.Length == 1 && char.IsLetter(key[0]) => (uint)VirtualKey.A + (uint)(char.ToUpper(key[0]) - 'A'),
        _ => 0,
    };

    /// <summary>VirtualKey 转显示名。</summary>
    public static string VkToName(uint vk) => ((VirtualKey)vk) switch
    {
        VirtualKey.Space => "Space",
        VirtualKey.Enter => "Enter",
        VirtualKey.Escape => "Esc",
        VirtualKey.Tab => "Tab",
        VirtualKey.Back => "Backspace",
        VirtualKey.Delete => "Delete",
        VirtualKey.Left => "Left",
        VirtualKey.Right => "Right",
        VirtualKey.Up => "Up",
        VirtualKey.Down => "Down",
        >= VirtualKey.F1 and <= VirtualKey.F12 => $"F{vk - (uint)VirtualKey.F1 + 1}",
        >= VirtualKey.Number0 and <= VirtualKey.Number9 => (vk - (uint)VirtualKey.Number0).ToString(),
        >= VirtualKey.A and <= VirtualKey.Z => ((char)('A' + (vk - (uint)VirtualKey.A))).ToString(),
        _ => $"Key{vk}",
    };
}
