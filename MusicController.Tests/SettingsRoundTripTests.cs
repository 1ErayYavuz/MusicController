using System.Text.Json;
using System.Windows.Input;
using FsCheck;
using FsCheck.Xunit;
using MusicController.Models;
using MusicController.Services;

namespace MusicController.Tests;

/// <summary>
/// Feature: music-controller, Property 3: Settings Round-Trip Consistency
/// Validates: Requirements 4.3, 5.2
/// </summary>
public class SettingsRoundTripTests
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true,
        Converters = { new HotkeyBindingConverter() }
    };

    // Generator for valid ModifierKeys combinations
    private static Gen<ModifierKeys> GenModifierKeys()
    {
        return Gen.Elements(
            ModifierKeys.None,
            ModifierKeys.Control,
            ModifierKeys.Alt,
            ModifierKeys.Shift,
            ModifierKeys.Control | ModifierKeys.Alt,
            ModifierKeys.Control | ModifierKeys.Shift,
            ModifierKeys.Alt | ModifierKeys.Shift,
            ModifierKeys.Control | ModifierKeys.Alt | ModifierKeys.Shift
        );
    }

    // Generator for valid Keys
    private static Gen<Key> GenKey()
    {
        return Gen.Elements(
            Key.Space, Key.Left, Key.Right, Key.Up, Key.Down,
            Key.A, Key.B, Key.C, Key.D, Key.E, Key.F, Key.G, Key.H,
            Key.D0, Key.D1, Key.D2, Key.D3, Key.D4, Key.D5,
            Key.F1, Key.F2, Key.F3, Key.F4, Key.F5, Key.F6
        );
    }

    // Generator for HotkeyBinding
    private static Gen<HotkeyBinding> GenHotkeyBinding()
    {
        return from modifiers in GenModifierKeys()
               from key in GenKey()
               select new HotkeyBinding(modifiers, key);
    }

    // Generator for ToastPosition
    private static Gen<ToastPosition> GenToastPosition()
    {
        return Gen.Elements(
            ToastPosition.TopLeft,
            ToastPosition.TopRight,
            ToastPosition.BottomLeft,
            ToastPosition.BottomRight
        );
    }

    // Generator for AppSettings
    private static Gen<AppSettings> GenAppSettings()
    {
        return from playPause in GenHotkeyBinding()
               from nextTrack in GenHotkeyBinding()
               from prevTrack in GenHotkeyBinding()
               from startWithWindows in Arb.Generate<bool>()
               from startMinimized in Arb.Generate<bool>()
               from toastDuration in Gen.Choose(500, 5000)
               from toastPosition in GenToastPosition()
               select new AppSettings
               {
                   Hotkeys = new Dictionary<HotkeyAction, HotkeyBinding>
                   {
                       { HotkeyAction.PlayPause, playPause },
                       { HotkeyAction.NextTrack, nextTrack },
                       { HotkeyAction.PreviousTrack, prevTrack }
                   },
                   StartWithWindows = startWithWindows,
                   StartMinimized = startMinimized,
                   ToastDurationMs = toastDuration,
                   ToastPosition = toastPosition
               };
    }

    /// <summary>
    /// Property 3: Settings Round-Trip Consistency
    /// For any valid AppSettings object, serializing then deserializing should produce an equivalent object.
    /// </summary>
    [Property(MaxTest = 100)]
    public Property SettingsRoundTrip_ShouldPreserveAllValues()
    {
        return Prop.ForAll(GenAppSettings().ToArbitrary(), settings =>
        {
            // Serialize
            var json = JsonSerializer.Serialize(settings, JsonOptions);
            
            // Deserialize
            var restored = JsonSerializer.Deserialize<AppSettings>(json, JsonOptions);
            
            // Verify equality
            return restored != null &&
                   restored.StartWithWindows == settings.StartWithWindows &&
                   restored.StartMinimized == settings.StartMinimized &&
                   restored.ToastDurationMs == settings.ToastDurationMs &&
                   restored.ToastPosition == settings.ToastPosition &&
                   HotkeysAreEqual(settings.Hotkeys, restored.Hotkeys);
        });
    }

    private static bool HotkeysAreEqual(
        Dictionary<HotkeyAction, HotkeyBinding> expected,
        Dictionary<HotkeyAction, HotkeyBinding> actual)
    {
        if (expected.Count != actual.Count) return false;
        
        foreach (var kvp in expected)
        {
            if (!actual.TryGetValue(kvp.Key, out var actualBinding))
                return false;
            if (actualBinding.Modifiers != kvp.Value.Modifiers || actualBinding.Key != kvp.Value.Key)
                return false;
        }
        
        return true;
    }
}
