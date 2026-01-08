using MusicController.Models;

namespace MusicController.Services;

public interface IMediaController
{
    Task<bool> PlayPauseAsync();
    Task<bool> NextTrackAsync();
    Task<bool> PreviousTrackAsync();
    Task<MediaInfo?> GetCurrentMediaInfoAsync();
}
