using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using QuickLauncher.Helpers;
using QuickLauncher.Models;

namespace QuickLauncher.Services;

/// <summary>
/// 基于 SetWindowsHookEx(WH_MOUSE_LL) 的全局鼠标钩子服务。
/// 监听光标位置，当停留在屏幕边缘阈值内时触发 EdgeEntered。
/// </summary>
public sealed class MouseHookService : IMouseHookService, IDisposable
{
    private readonly ConcurrentDictionary<ScreenEdge, EdgeGestureSetting> _edges = new();
    private NativeMethods.LowLevelMouseProc? _proc;
    private IntPtr _hook = IntPtr.Zero;
    private bool _disposed;

    public bool IsRunning => _hook != IntPtr.Zero;

    public event EventHandler<EdgeEnteredEventArgs>? EdgeEntered;

    public void Start()
    {
        if (IsRunning) return;
        _proc = HookCallback;
        _hook = NativeMethods.SetWindowsHookEx(
            NativeMethods.WH_MOUSE_LL,
            _proc,
            NativeMethods.GetModuleHandle(null),
            0);
    }

    public void Stop()
    {
        if (!IsRunning) return;
        NativeMethods.UnhookWindowsHookEx(_hook);
        _hook = IntPtr.Zero;
        _proc = null;
    }

    public void RegisterEdge(EdgeGestureSetting gesture)
    {
        _edges[gesture.Edge] = gesture;
    }

    public void UnregisterEdge(ScreenEdge edge) => _edges.TryRemove(edge, out _);

    public void ClearEdges() => _edges.Clear();

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
    {
        if (nCode >= 0 && wParam.ToInt32() == NativeMethods.WM_MOUSEMOVE)
        {
            var hookStruct = Marshal.PtrToStructure<NativeMethods.MSLLHOOKSTRUCT>(lParam);
            CheckEdge(hookStruct.pt.X, hookStruct.pt.Y);
        }
        return NativeMethods.CallNextHookEx(_hook, nCode, wParam, lParam);
    }

    private void CheckEdge(int x, int y)
    {
        if (_edges.IsEmpty) return;

        var screen = GetScreenBounds();
        foreach (var kv in _edges)
        {
            var edge = kv.Key;
            var g = kv.Value;
            if (!g.Enabled) continue;

            bool hit = edge switch
            {
                ScreenEdge.Left => x <= g.Threshold,
                ScreenEdge.Right => x >= screen.Width - g.Threshold,
                ScreenEdge.Top => y <= g.Threshold,
                ScreenEdge.Bottom => y >= screen.Height - g.Threshold,
                _ => false,
            };

            if (hit)
            {
                EdgeEntered?.Invoke(this, new EdgeEnteredEventArgs { Edge = edge, Action = g.Action });
            }
        }
    }

    private static (int Width, int Height) GetScreenBounds()
    {
        // 使用 Win32 GetSystemMetrics 获取主屏幕分辨率，避免引入 WinForms 依赖
        int w = GetSystemMetrics(SM_CXSCREEN);
        int h = GetSystemMetrics(SM_CYSCREEN);
        return (w, h);
    }

    private const int SM_CXSCREEN = 0;
    private const int SM_CYSCREEN = 1;

    [DllImport("user32.dll")]
    private static extern int GetSystemMetrics(int nIndex);

    public void Dispose()
    {
        if (_disposed) return;
        Stop();
        _disposed = true;
    }
}
