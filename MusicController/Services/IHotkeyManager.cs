using MusicController.Models;

namespace MusicController.Services;

public interface IHotkeyManager : IDisposable
{
    event EventHandler<HotkeyEventArgs>? HotkeyPressed;
    
    bool RegisterHotkey(HotkeyAction action, HotkeyBinding binding);
    bool UnregisterHotkey(HotkeyAction action);
    IReadOnlyDictionary<HotkeyAction, HotkeyBinding> GetRegisteredHotkeys();
    void RegisterAllHotkeys(Dictionary<HotkeyAction, HotkeyBinding> hotkeys);
    void UnregisterAllHotkeys();
}
