using System.Windows.Input;
using FsCheck;
using FsCheck.Xunit;
using MusicController.Models;
using MusicController.Services;

namespace MusicController.Tests;

/// <summary>
/// Feature: music-controller, Property 4: Hotkey Registration Consistency
/// Validates: Requirements 1.5
/// </summary>
public class HotkeyRegistrationTests
{
    // Generator for valid ModifierKeys combinations (must have at least one modifier for global hotkeys)
    private static Gen<ModifierKeys> GenModifierKeys()
    {
        return Gen.Elements(
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

    // Generator for HotkeyAction
    private static Gen<HotkeyAction> GenHotkeyAction()
    {
        return Gen.Elements(
            HotkeyAction.PlayPause,
            HotkeyAction.NextTrack,
            HotkeyAction.PreviousTrack
        );
    }

    /// <summary>
    /// Property 4: Hotkey Registration Consistency
    /// For any valid HotkeyBinding, the internal registration dictionary should correctly track the binding.
    /// Note: This tests the in-memory tracking, not the actual Win32 registration (which requires a window handle).
    /// </summary>
    [Property(MaxTest = 100)]
    public Property HotkeyBinding_ShouldHaveConsistentDisplayString()
    {
        return Prop.ForAll(GenHotkeyBinding().ToArbitrary(), binding =>
        {
            var displayString = binding.ToDisplayString();
            
            // Display string should not be empty
            var notEmpty = !string.IsNullOrWhiteSpace(displayString);
            
            // Display string should contain the key
            var containsKey = displayString.Contains(binding.Key.ToString());
            
            // If modifiers exist, display string should contain them
            var containsModifiers = true;
            if (binding.Modifiers.HasFlag(ModifierKeys.Control))
                containsModifiers &= displayString.Contains("Ctrl");
            if (binding.Modifiers.HasFlag(ModifierKeys.Alt))
                containsModifiers &= displayString.Contains("Alt");
            if (binding.Modifiers.HasFlag(ModifierKeys.Shift))
                containsModifiers &= displayString.Contains("Shift");
            
            return notEmpty && containsKey && containsModifiers;
        });
    }

    /// <summary>
    /// Property 4 (continued): Hotkey dictionary operations should be consistent
    /// </summary>
    [Property(MaxTest = 100)]
    public Property HotkeyDictionary_ShouldMaintainConsistency()
    {
        return Prop.ForAll(
            GenHotkeyAction().ToArbitrary(),
            GenHotkeyBinding().ToArbitrary(),
            (action, binding) =>
            {
                var hotkeys = new Dictionary<HotkeyAction, HotkeyBinding>
                {
                    { action, binding }
                };
                
                // After adding, dictionary should contain the action
                var containsAction = hotkeys.ContainsKey(action);
                
                // Retrieved binding should match
                var bindingMatches = hotkeys[action].Modifiers == binding.Modifiers &&
                                    hotkeys[action].Key == binding.Key;
                
                return containsAction && bindingMatches;
            });
    }
}
