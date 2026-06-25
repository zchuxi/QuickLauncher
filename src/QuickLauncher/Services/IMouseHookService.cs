using QuickLauncher.Models;

namespace QuickLauncher.Services;

/// <summary>
/// 鼠标钩子服务接口：监听屏幕边缘手势。
/// </summary>
public interface IMouseHookService
{
    void Start();
    void Stop();
    bool IsRunning { get; }

    /// <summary>注册一个边缘手势。</summary>
    void RegisterEdge(EdgeGestureSetting gesture);

    /// <summary>取消某边缘注册。</summary>
    void UnregisterEdge(ScreenEdge edge);

    /// <summary>清空所有边缘手势。</summary>
    void ClearEdges();

    /// <summary>鼠标进入边缘阈值区域。</summary>
    event EventHandler<EdgeEnteredEventArgs>? EdgeEntered;
}

public sealed class EdgeEnteredEventArgs : EventArgs
{
    public ScreenEdge Edge { get; init; }
    public GestureAction Action { get; init; }
}
