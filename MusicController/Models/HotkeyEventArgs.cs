namespace MusicController.Models;

public class HotkeyEventArgs : EventArgs
{
    public HotkeyAction Action { get; init; }
}
