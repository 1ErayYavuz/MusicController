namespace MusicController.Models;

public record MediaInfo(
    string Title,
    string Artist,
    string AppName,
    MediaPlaybackStatus Status
);
