using MusicController.Models;

namespace MusicController.Services;

public class AppController
{
    private readonly HotkeyManager _hotkeyManager;
    private readonly MediaController _mediaController;
    private readonly ToastNotificationService _toastService;
    private readonly ISettingsManager _settingsManager;
    private AppSettings _settings;

    public AppController(
        HotkeyManager hotkeyManager,
        MediaController mediaController,
        ToastNotificationService toastService,
        ISettingsManager settingsManager)
    {
        _hotkeyManager = hotkeyManager;
        _mediaController = mediaController;
        _toastService = toastService;
        _settingsManager = settingsManager;
        _settings = settingsManager.LoadSettings();
    }

    public AppSettings Settings => _settings;

    public async Task InitializeAsync()
    {
        await _mediaController.InitializeAsync();
        _toastService.Configure(_settings.ToastPosition, _settings.ToastDurationMs);
        _hotkeyManager.HotkeyPressed += OnHotkeyPressed;
    }

    public void RegisterHotkeys()
    {
        _hotkeyManager.RegisterAllHotkeys(_settings.Hotkeys);
    }

    public void UpdateHotkeys(Dictionary<HotkeyAction, HotkeyBinding> newHotkeys)
    {
        _hotkeyManager.UnregisterAllHotkeys();
        
        _settings = _settings with { Hotkeys = newHotkeys };
        _settingsManager.SaveSettings(_settings);
        
        _hotkeyManager.RegisterAllHotkeys(_settings.Hotkeys);
    }

    public void UpdateLanguage(string language)
    {
        _settings = _settings with { Language = language };
        _settingsManager.SaveSettings(_settings);
        LocalizationManager.SetLanguage(language);
    }

    private async void OnHotkeyPressed(object? sender, HotkeyEventArgs e)
    {
        await HandleHotkeyAsync(e.Action);
    }

    public async Task HandleHotkeyAsync(HotkeyAction action)
    {
        bool success;
        ToastType toastType;

        switch (action)
        {
            case HotkeyAction.PlayPause:
                success = await _mediaController.PlayPauseAsync();
                toastType = ToastType.PlayPause;
                break;
            case HotkeyAction.NextTrack:
                success = await _mediaController.NextTrackAsync();
                toastType = ToastType.NextTrack;
                break;
            case HotkeyAction.PreviousTrack:
                success = await _mediaController.PreviousTrackAsync();
                toastType = ToastType.PreviousTrack;
                break;
            default:
                return;
        }

        if (success)
        {
            var mediaInfo = await GetMediaInfoWithRetryAsync(action);
            _toastService.ShowToast(toastType, mediaInfo);
        }
        else
        {
            _toastService.ShowToast(ToastType.NoMedia, null);
        }
    }

    private async Task<MediaInfo?> GetMediaInfoWithRetryAsync(HotkeyAction action)
    {
        // Play/Pause için kısa bekleme
        if (action == HotkeyAction.PlayPause)
        {
            await Task.Delay(150);
            return await _mediaController.GetCurrentMediaInfoAsync();
        }

        // Next/Previous için birkaç deneme yap
        for (int i = 0; i < 6; i++)
        {
            await Task.Delay(400);
            var info = await _mediaController.GetCurrentMediaInfoAsync();
            
            if (info != null && 
                !string.IsNullOrWhiteSpace(info.Title) && 
                info.Title != "Unknown" &&
                !string.IsNullOrWhiteSpace(info.Artist) &&
                info.Artist != "Unknown")
            {
                return info;
            }
        }

        return await _mediaController.GetCurrentMediaInfoAsync();
    }

    public void Shutdown()
    {
        _hotkeyManager.UnregisterAllHotkeys();
        _hotkeyManager.Dispose();
    }
}
