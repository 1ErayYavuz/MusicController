using System.Text.Json;
using System.Text.Json.Serialization;
using System.Windows.Input;
using MusicController.Models;

namespace MusicController.Services;

public class HotkeyBindingConverter : JsonConverter<HotkeyBinding>
{
    public override HotkeyBinding? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;
        
        var modifiersStr = root.GetProperty("modifiers").GetString() ?? "";
        var keyStr = root.GetProperty("key").GetString() ?? "";
        
        var modifiers = ParseModifiers(modifiersStr);
        var key = Enum.TryParse<Key>(keyStr, true, out var k) ? k : Key.None;
        
        return new HotkeyBinding(modifiers, key);
    }

    public override void Write(Utf8JsonWriter writer, HotkeyBinding value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("modifiers", FormatModifiers(value.Modifiers));
        writer.WriteString("key", value.Key.ToString());
        writer.WriteEndObject();
    }

    private static ModifierKeys ParseModifiers(string str)
    {
        var result = ModifierKeys.None;
        var parts = str.Split('+', StringSplitOptions.TrimEntries);
        
        foreach (var part in parts)
        {
            if (part.Equals("Ctrl", StringComparison.OrdinalIgnoreCase))
                result |= ModifierKeys.Control;
            else if (part.Equals("Alt", StringComparison.OrdinalIgnoreCase))
                result |= ModifierKeys.Alt;
            else if (part.Equals("Shift", StringComparison.OrdinalIgnoreCase))
                result |= ModifierKeys.Shift;
            else if (part.Equals("Win", StringComparison.OrdinalIgnoreCase))
                result |= ModifierKeys.Windows;
        }
        
        return result;
    }

    private static string FormatModifiers(ModifierKeys modifiers)
    {
        var parts = new List<string>();
        
        if (modifiers.HasFlag(ModifierKeys.Control)) parts.Add("Ctrl");
        if (modifiers.HasFlag(ModifierKeys.Alt)) parts.Add("Alt");
        if (modifiers.HasFlag(ModifierKeys.Shift)) parts.Add("Shift");
        if (modifiers.HasFlag(ModifierKeys.Windows)) parts.Add("Win");
        
        return string.Join("+", parts);
    }
}
