using System.Collections.Concurrent;
using QuickLauncher.Helpers;
using QuickLauncher.Models;
using Microsoft.UI.Xaml;

namespace QuickLauncher.Services;

/// <summary>
/// 基于 Win32 RegisterHotKey 的全局快捷键服务。
/// 通过窗口消息泵接收 WM_HOTKEY。
/// </summary>
public sealed class HotkeyService : IHotkeyService, IDisposable
{
    private readonly ConcurrentDictionary<int, HotkeyConfig> _byId = new();
    private readonly ConcurrentDictionary<string, int> _idToAtom = new();
    private int _counter = 0xC000;
    private HotkeyMessageWindow? _window;
    private bool _disposed;

    public IReadOnlyList<HotkeyConfig> Registered => _byId.Values.ToList();

    public event EventHandler<HotkeyTriggeredEventArgs>? HotkeyTriggered;

    public HotkeyService()
    {
        // 延迟创建消息窗口，需在 UI 线程
    }

    /// <summary>在 UI 线程初始化消息窗口。</summary>
    public void EnsureInitialized()
    {
        _window ??= new HotkeyMessageWindow(this);
    }

    public bool Register(HotkeyConfig config)
    {
        EnsureInitialized();
        if (string.IsNullOrEmpty(config.Id)) return false;

        // 若已存在同名先取消
        if (_idToAtom.TryGetValue(config.Id, out var oldAtom))
        {
            NativeMethods.UnregisterHotKey(_window!.Handle, oldAtom);
            _byId.TryRemove(oldAtom, out _);
            _idToAtom.TryRemove(config.Id, out _);
        }

        var (mods, vk) = HotkeyParser.Parse(config.DisplayKey);
        if (vk == 0 && config.Action != Models.HotkeyAction.HidePanel)
        {
            // Space 的 vk 非零，OK
        }

        var atom = System.Threading.Interlocked.Increment(ref _counter);
        var ok = NativeMethods.RegisterHotKey(_window!.Handle, atom, mods | NativeMethods.MOD_NOREPEAT, vk);
        if (!ok) return false;

        _byId[atom] = config;
        _idToAtom[config.Id] = atom;
        return true;
    }

    public bool Unregister(string id)
    {
        if (_window is null) return false;
        if (!_idToAtom.TryRemove(id, out var atom)) return false;
        var ok = NativeMethods.UnregisterHotKey(_window.Handle, atom);
        _byId.TryRemove(atom, out _);
        return ok;
    }

    public void UnregisterAll()
    {
        if (_window is null) return;
        foreach (var atom in _byId.Keys)
        {
            NativeMethods.UnregisterHotKey(_window.Handle, atom);
        }
        _byId.Clear();
        _idToAtom.Clear();
    }

    internal void OnWmHotkey(int atom)
    {
        if (_byId.TryGetValue(atom, out var config))
        {
            HotkeyTriggered?.Invoke(this, new HotkeyTriggeredEventArgs { Config = config });
        }
    }

    public void Dispose()
    {
        if (_disposed) return;
        UnregisterAll();
        _window?.Dispose();
        _disposed = true;
    }
}
