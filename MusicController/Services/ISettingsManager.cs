using MusicController.Models;

namespace MusicController.Services;

public interface ISettingsManager
{
    AppSettings LoadSettings();
    void SaveSettings(AppSettings settings);
}
