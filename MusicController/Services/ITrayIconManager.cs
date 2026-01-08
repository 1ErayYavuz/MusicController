namespace MusicController.Services;

public interface ITrayIconManager : IDisposable
{
    event EventHandler? ExitRequested;
    event EventHandler? ShowWindowRequested;
    
    void Initialize();
    void ShowBalloonTip(string title, string message);
}
