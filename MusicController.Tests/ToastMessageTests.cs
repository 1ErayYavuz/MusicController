using FsCheck;
using FsCheck.Xunit;
using MusicController.Models;
using MusicController.Services;

namespace MusicController.Tests;

/// <summary>
/// Feature: music-controller, Property 2: Toast Message Completeness
/// Validates: Requirements 2.3
/// </summary>
public class ToastMessageTests
{
    private static Gen<ToastType> GenToastType()
    {
        return Gen.Elements(
            ToastType.PlayPause,
            ToastType.NextTrack,
            ToastType.PreviousTrack,
            ToastType.NoMedia
        );
    }

    /// <summary>
    /// Property 2: Toast Message Completeness
    /// For any ToastType enum value, the toast notification service should produce 
    /// a non-empty, descriptive message string.
    /// </summary>
    [Property(MaxTest = 100)]
    public Property ToastMessage_ShouldBeNonEmptyForAllTypes()
    {
        var service = new ToastNotificationService();
        
        return Prop.ForAll(GenToastType().ToArbitrary(), toastType =>
        {
            var message = service.GetActionText(toastType);
            
            // Message should not be null or empty
            var notEmpty = !string.IsNullOrWhiteSpace(message);
            
            // Message should have reasonable length (at least 3 chars)
            var hasContent = message.Length >= 3;
            
            return notEmpty && hasContent;
        });
    }

    /// <summary>
    /// Property 2 (continued): Each toast type should have a unique message
    /// </summary>
    [Fact]
    public void ToastMessages_ShouldBeUniqueForEachType()
    {
        var service = new ToastNotificationService();
        var messages = new HashSet<string>();
        
        foreach (ToastType type in Enum.GetValues<ToastType>())
        {
            var message = service.GetActionText(type);
            Assert.True(messages.Add(message), $"Duplicate message for {type}: {message}");
        }
    }
}
