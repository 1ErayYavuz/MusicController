using System.Windows.Input;

namespace MusicController.Models;

public record AppSettings
{
    public Dictionary<HotkeyAction, HotkeyBinding> Hotkeys { get; init; } = new()
    {
        { HotkeyAction.PlayPause, new HotkeyBinding(ModifierKeys.Control | ModifierKeys.Alt, Key.Space) },
        { HotkeyAction.NextTrack, new HotkeyBinding(ModifierKeys.Control | ModifierKeys.Alt, Key.Right) },
        { HotkeyAction.PreviousTrack, new HotkeyBinding(ModifierKeys.Control | ModifierKeys.Alt, Key.Left) }
    };
    
    public bool StartWithWindows { get; init; } = false;
    public bool StartMinimized { get; init; } = false;
    public int ToastDurationMs { get; init; } = 2000;
    public ToastPosition ToastPosition { get; init; } = ToastPosition.BottomRight;
    public string Language { get; init; } = "tr";
}
