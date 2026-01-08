using MusicController.Models;
using Windows.Media.Control;

namespace MusicController.Services;

public class MediaController : IMediaController
{
    private GlobalSystemMediaTransportControlsSessionManager? _sessionManager;

    public async Task InitializeAsync()
    {
        _sessionManager = await GlobalSystemMediaTransportControlsSessionManager.RequestAsync();
    }

    public async Task<bool> PlayPauseAsync()
    {
        var session = GetCurrentSession();
        if (session == null) return false;

        try
        {
            return await session.TryTogglePlayPauseAsync();
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> NextTrackAsync()
    {
        var session = GetCurrentSession();
        if (session == null) return false;

        try
        {
            return await session.TrySkipNextAsync();
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> PreviousTrackAsync()
    {
        var session = GetCurrentSession();
        if (session == null) return false;

        try
        {
            return await session.TrySkipPreviousAsync();
        }
        catch
        {
            return false;
        }
    }

    public async Task<MediaInfo?> GetCurrentMediaInfoAsync()
    {
        var session = GetCurrentSession();
        if (session == null) return null;

        try
        {
            var mediaProperties = await session.TryGetMediaPropertiesAsync();
            if (mediaProperties == null) return null;

            var playbackInfo = session.GetPlaybackInfo();
            var status = ConvertPlaybackStatus(playbackInfo?.PlaybackStatus);

            return new MediaInfo(
                mediaProperties.Title ?? "Unknown",
                mediaProperties.Artist ?? "Unknown",
                session.SourceAppUserModelId ?? "Unknown",
                status
            );
        }
        catch
        {
            return null;
        }
    }

    private GlobalSystemMediaTransportControlsSession? GetCurrentSession()
    {
        return _sessionManager?.GetCurrentSession();
    }

    private static MediaPlaybackStatus ConvertPlaybackStatus(
        GlobalSystemMediaTransportControlsSessionPlaybackStatus? status)
    {
        return status switch
        {
            GlobalSystemMediaTransportControlsSessionPlaybackStatus.Playing => MediaPlaybackStatus.Playing,
            GlobalSystemMediaTransportControlsSessionPlaybackStatus.Paused => MediaPlaybackStatus.Paused,
            GlobalSystemMediaTransportControlsSessionPlaybackStatus.Stopped => MediaPlaybackStatus.Stopped,
            _ => MediaPlaybackStatus.Unknown
        };
    }
}
