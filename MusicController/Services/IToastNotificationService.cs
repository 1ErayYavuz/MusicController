using MusicController.Models;

namespace MusicController.Services;

public interface IToastNotificationService
{
    void ShowToast(ToastType type, MediaInfo? mediaInfo);
    void HideToast();
    string GetActionText(ToastType type);
}
