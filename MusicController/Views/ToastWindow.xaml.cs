using System.Windows;
using System.Windows.Media.Animation;
using MusicController.Models;
using MusicController.Services;
using WpfColor = System.Windows.Media.Color;
using SolidColorBrush = System.Windows.Media.SolidColorBrush;

namespace MusicController.Views;

public partial class ToastWindow : Window
{
    public ToastWindow()
    {
        InitializeComponent();
    }

    public void ShowMessage(string action, string? songTitle, string? artist, string? appName, ToastPosition position)
    {
        ActionText.Text = action;
        SongText.Text = string.IsNullOrEmpty(songTitle) ? LocalizationManager.Get("UnknownTitle") : songTitle;
        ArtistText.Text = string.IsNullOrEmpty(artist) ? LocalizationManager.Get("UnknownArtist") : artist;
        
        // Platform gösterimi
        var (platformName, platformColor) = GetPlatformInfo(appName);
        PlatformText.Text = platformName;
        PlatformText.Foreground = new SolidColorBrush(platformColor);
        
        PositionWindow(position);
        
        var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(150));
        BeginAnimation(OpacityProperty, fadeIn);
        
        Show();
    }

    private static (string Name, WpfColor Color) GetPlatformInfo(string? appName)
    {
        if (string.IsNullOrEmpty(appName))
            return ("Unknown", WpfColor.FromRgb(136, 136, 136));

        var lowerName = appName.ToLowerInvariant();
        
        return lowerName switch
        {
            var n when n.Contains("spotify") => ("Spotify", WpfColor.FromRgb(29, 185, 84)),
            var n when n.Contains("chrome") => ("Chrome", WpfColor.FromRgb(66, 133, 244)),
            var n when n.Contains("firefox") => ("Firefox", WpfColor.FromRgb(255, 149, 0)),
            var n when n.Contains("edge") => ("Edge", WpfColor.FromRgb(0, 120, 212)),
            var n when n.Contains("opera") => ("Opera", WpfColor.FromRgb(255, 28, 75)),
            var n when n.Contains("brave") => ("Brave", WpfColor.FromRgb(251, 84, 43)),
            var n when n.Contains("youtube") || n.Contains("ytmusic") => ("YouTube", WpfColor.FromRgb(255, 0, 0)),
            var n when n.Contains("vlc") => ("VLC", WpfColor.FromRgb(255, 136, 0)),
            var n when n.Contains("itunes") || n.Contains("apple") => ("Apple Music", WpfColor.FromRgb(252, 60, 68)),
            var n when n.Contains("deezer") => ("Deezer", WpfColor.FromRgb(254, 234, 59)),
            var n when n.Contains("tidal") => ("Tidal", WpfColor.FromRgb(0, 0, 0)),
            var n when n.Contains("amazon") => ("Amazon Music", WpfColor.FromRgb(37, 209, 218)),
            var n when n.Contains("soundcloud") => ("SoundCloud", WpfColor.FromRgb(255, 85, 0)),
            var n when n.Contains("foobar") => ("foobar2000", WpfColor.FromRgb(255, 165, 0)),
            var n when n.Contains("winamp") => ("Winamp", WpfColor.FromRgb(255, 204, 0)),
            var n when n.Contains("groove") || n.Contains("zune") => ("Groove", WpfColor.FromRgb(107, 71, 200)),
            var n when n.Contains("mediamonkey") => ("MediaMonkey", WpfColor.FromRgb(255, 204, 0)),
            _ => (GetCleanAppName(appName), WpfColor.FromRgb(136, 136, 136))
        };
    }

    private static string GetCleanAppName(string appName)
    {
        var name = appName;
        
        if (name.Contains('.'))
        {
            var parts = name.Split('.');
            name = parts.Length > 1 ? parts[^2] : parts[0];
        }
        
        if (name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
            name = name[..^4];
        
        if (name.Length > 0)
            name = char.ToUpper(name[0]) + name[1..];
        
        return name.Length > 12 ? name[..12] + "…" : name;
    }

    public void HideWithAnimation()
    {
        var fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(150));
        fadeOut.Completed += (s, e) => Hide();
        BeginAnimation(OpacityProperty, fadeOut);
    }

    private void PositionWindow(ToastPosition position)
    {
        var workArea = SystemParameters.WorkArea;
        const double margin = 20;

        Left = position switch
        {
            ToastPosition.TopLeft or ToastPosition.BottomLeft => workArea.Left + margin,
            _ => workArea.Right - Width - margin
        };

        Top = position switch
        {
            ToastPosition.TopLeft or ToastPosition.TopRight => workArea.Top + margin,
            _ => workArea.Bottom - Height - margin
        };
    }
}
