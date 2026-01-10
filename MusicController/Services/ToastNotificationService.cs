using System.Windows.Threading;
using MusicController.Models;
using MusicController.Views;

namespace MusicController.Services;

public class ToastNotificationService : IToastNotificationService
{
    private ToastWindow? _toastWindow;
    private DispatcherTimer? _hideTimer;
    private ToastPosition _position = ToastPosition.BottomRight;
    private int _durationMs = 2000;

    public void Configure(ToastPosition position, int durationMs)
    {
        _position = position;
        _durationMs = durationMs;
    }

    public void ShowToast(ToastType type, MediaInfo? mediaInfo)
    {
        _hideTimer?.Stop();

        if (_toastWindow == null)
        {
            _toastWindow = new ToastWindow();
        }

        var actionText = GetActionText(type);
        _toastWindow.ShowMessage(actionText, mediaInfo?.Title, mediaInfo?.Artist, mediaInfo?.AppName, _position);

        _hideTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(_durationMs)
        };
        _hideTimer.Tick += (s, e) =>
        {
            _hideTimer.Stop();
            HideToast();
        };
        _hideTimer.Start();
    }

    public void HideToast()
    {
        _toastWindow?.HideWithAnimation();
    }

    public string GetActionText(ToastType type)
    {
        return type switch
        {
            ToastType.PlayPause => LocalizationManager.Get("ToastPlayPause"),
            ToastType.NextTrack => LocalizationManager.Get("ToastNextTrack"),
            ToastType.PreviousTrack => LocalizationManager.Get("ToastPrevTrack"),
            ToastType.VolumeUp => LocalizationManager.Get("ToastVolumeUp"),
            ToastType.VolumeDown => LocalizationManager.Get("ToastVolumeDown"),
            ToastType.Mute => LocalizationManager.Get("ToastMute"),
            ToastType.NoMedia => LocalizationManager.Get("ToastNoMedia"),
            _ => LocalizationManager.Get("ToastDefault")
        };
    }
}
