using System;
using System.Runtime.InteropServices;
using QuickLauncher.Helpers;

namespace QuickLauncher.Services;

/// <summary>
/// 用于接收 WM_HOTKEY 的隐藏 Win32 消息窗口。
/// 在 WinUI 3 unpackaged 应用中，App 主窗口的 WndProc 不易拦截，
/// 因此创建独立隐藏窗口接收全局快捷键消息。
/// </summary>
internal sealed class HotkeyMessageWindow : IDisposable
{
    private const string ClassName = "QuickLauncher_HotkeyWnd";
    private readonly HotkeyService _owner;
    private IntPtr _hwnd;
    private WNDPROC? _wndProc;
    private bool _disposed;

    public IntPtr Handle => _hwnd;

    public HotkeyMessageWindow(HotkeyService owner)
    {
        _owner = owner;
        Create();
    }

    private void Create()
    {
        _wndProc = WndProc;

        var wc = new WNDCLASSEXW
        {
            cbSize = Marshal.SizeOf<WNDCLASSEXW>(),
            lpfnWndProc = _wndProc,
            hInstance = NativeMethods.GetModuleHandle(null),
            lpszClassName = ClassName,
        };
        ushort atom = RegisterClassEx(ref wc);
        if (atom == 0)
        {
            // 类可能已注册，忽略错误继续
        }

        _hwnd = CreateWindowEx(
            0, ClassName, "QuickLauncher Hotkey",
            0, 0, 0, 0, 0,
            IntPtr.Zero, IntPtr.Zero, wc.hInstance, IntPtr.Zero);

        if (_hwnd == IntPtr.Zero)
        {
            throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
        }
    }

    private IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        if (msg == NativeMethods.WM_HOTKEY)
        {
            int atom = wParam.ToInt32();
            _owner.OnWmHotkey(atom);
            return IntPtr.Zero;
        }
        return DefWindowProc(hWnd, msg, wParam, lParam);
    }

    public void Dispose()
    {
        if (_disposed) return;
        if (_hwnd != IntPtr.Zero)
        {
            DestroyWindow(_hwnd);
            _hwnd = IntPtr.Zero;
        }
        _disposed = true;
    }

    // ---- Win32 ----
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    private struct WNDCLASSEXW
    {
        public int cbSize;
        public uint style;
        public WNDPROC lpfnWndProc;
        public int cbClsExtra;
        public int cbWndExtra;
        public IntPtr hInstance;
        public IntPtr hIcon;
        public IntPtr hCursor;
        public IntPtr hbrBackground;
        public string? lpszMenuName;
        public string lpszClassName;
        public IntPtr hIconSm;
    }

    private delegate IntPtr WNDPROC(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern ushort RegisterClassEx(ref WNDCLASSEXW lpwcx);

    [DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern IntPtr CreateWindowEx(
        uint dwExStyle, string lpClassName, string lpWindowName,
        uint dwStyle, int x, int y, int nWidth, int nHeight,
        IntPtr hWndParent, IntPtr hMenu, IntPtr hInstance, IntPtr lpParam);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool DestroyWindow(IntPtr hWnd);

    [DllImport("user32.dll")]
    private static extern IntPtr DefWindowProc(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
}
