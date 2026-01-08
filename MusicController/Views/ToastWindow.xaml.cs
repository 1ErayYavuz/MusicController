using System.Windows;
using System.Windows.Media.Animation;
using MusicController.Models;

namespace MusicController.Views;

public partial class ToastWindow : Window
{
    public ToastWindow()
    {
        InitializeComponent();
    }

    public void ShowMessage(string action, string? songTitle, string? artist, ToastPosition position)
    {
        ActionText.Text = action;
        SongText.Text = string.IsNullOrEmpty(songTitle) ? "Bilinmeyen Şarkı" : songTitle;
        ArtistText.Text = string.IsNullOrEmpty(artist) ? "Bilinmeyen Sanatçı" : artist;
        
        PositionWindow(position);
        
        // Fade in
        var fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(150));
        BeginAnimation(OpacityProperty, fadeIn);
        
        Show();
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
