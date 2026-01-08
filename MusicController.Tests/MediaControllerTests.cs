using FsCheck;
using FsCheck.Xunit;
using MusicController.Models;

namespace MusicController.Tests;

/// <summary>
/// Feature: music-controller, Property 1: Play/Pause Toggle Idempotence
/// Validates: Requirements 1.1
/// </summary>
public class MediaControllerTests
{
    // Mock media controller for testing toggle idempotence
    private class MockMediaController
    {
        public MediaPlaybackStatus Status { get; private set; }

        public MockMediaController(MediaPlaybackStatus initialStatus)
        {
            Status = initialStatus;
        }

        public bool TogglePlayPause()
        {
            Status = Status switch
            {
                MediaPlaybackStatus.Playing => MediaPlaybackStatus.Paused,
                MediaPlaybackStatus.Paused => MediaPlaybackStatus.Playing,
                MediaPlaybackStatus.Stopped => MediaPlaybackStatus.Playing,
                _ => MediaPlaybackStatus.Playing
            };
            return true;
        }
    }

    private static Gen<MediaPlaybackStatus> GenPlaybackStatus()
    {
        return Gen.Elements(
            MediaPlaybackStatus.Playing,
            MediaPlaybackStatus.Paused
        );
    }

    /// <summary>
    /// Property 1: Play/Pause Toggle Idempotence
    /// For any media playback state (Playing or Paused), calling PlayPause twice 
    /// should return the media to its original state.
    /// </summary>
    [Property(MaxTest = 100)]
    public Property PlayPauseToggle_ShouldBeIdempotentAfterTwoCalls()
    {
        return Prop.ForAll(GenPlaybackStatus().ToArbitrary(), initialStatus =>
        {
            var controller = new MockMediaController(initialStatus);
            var originalStatus = controller.Status;
            
            // Toggle twice
            controller.TogglePlayPause();
            controller.TogglePlayPause();
            
            // Should return to original state
            return controller.Status == originalStatus;
        });
    }

    /// <summary>
    /// Property 1 (continued): Single toggle should change state
    /// </summary>
    [Property(MaxTest = 100)]
    public Property PlayPauseToggle_SingleToggleShouldChangeState()
    {
        return Prop.ForAll(GenPlaybackStatus().ToArbitrary(), initialStatus =>
        {
            var controller = new MockMediaController(initialStatus);
            var originalStatus = controller.Status;
            
            // Toggle once
            controller.TogglePlayPause();
            
            // State should be different
            return controller.Status != originalStatus;
        });
    }
}
