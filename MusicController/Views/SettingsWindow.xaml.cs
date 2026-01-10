using System.Windows;
using System.Windows.Input;
using MusicController.Models;
using MusicController.Services;
using TextBox = System.Windows.Controls.TextBox;
using Color = System.Windows.Media.Color;
using SolidColorBrush = System.Windows.Media.SolidColorBrush;
using ComboBoxItem = System.Windows.Controls.ComboBoxItem;

namespace MusicController.Views;

public partial class SettingsWindow : Window
{
    private readonly Dictionary<HotkeyAction, HotkeyBinding> _hotkeys;
    private TextBox? _activeBox;
    private string _selectedLanguage;

    public Dictionary<HotkeyAction, HotkeyBinding> Hotkeys => _hotkeys;
    public string SelectedLanguage => _selectedLanguage;
    public bool Saved { get; private set; }

    public SettingsWindow(Dictionary<HotkeyAction, HotkeyBinding> currentHotkeys, string currentLanguage)
    {
        InitializeComponent();
        _hotkeys = new Dictionary<HotkeyAction, HotkeyBinding>(currentHotkeys);
        _selectedLanguage = currentLanguage;
        
        LoadLanguageCombo();
        LoadHotkeys();
        UpdateUILanguage();
    }

    private void LoadLanguageCombo()
    {
        foreach (var lang in LocalizationManager.AvailableLanguages)
        {
            var item = new ComboBoxItem
            {
                Content = LocalizationManager.GetLanguageDisplayName(lang),
                Tag = lang
            };
            LanguageCombo.Items.Add(item);
            
            if (lang == _selectedLanguage)
                LanguageCombo.SelectedItem = item;
        }
    }

    private void LoadHotkeys()
    {
        if (_hotkeys.TryGetValue(HotkeyAction.PlayPause, out var pp))
            PlayPauseBox.Text = pp.ToDisplayString();
        
        if (_hotkeys.TryGetValue(HotkeyAction.NextTrack, out var nt))
            NextTrackBox.Text = nt.ToDisplayString();
        
        if (_hotkeys.TryGetValue(HotkeyAction.PreviousTrack, out var pt))
            PrevTrackBox.Text = pt.ToDisplayString();
        
        if (_hotkeys.TryGetValue(HotkeyAction.VolumeUp, out var vu))
            VolumeUpBox.Text = vu.ToDisplayString();
        
        if (_hotkeys.TryGetValue(HotkeyAction.VolumeDown, out var vd))
            VolumeDownBox.Text = vd.ToDisplayString();
        
        if (_hotkeys.TryGetValue(HotkeyAction.Mute, out var m))
            MuteBox.Text = m.ToDisplayString();
    }

    private void UpdateUILanguage()
    {
        Title = LocalizationManager.Get("Settings");
        TitleText.Text = LocalizationManager.Get("HotkeySettings");
        PlayPauseLabel.Text = LocalizationManager.Get("PlayPause");
        NextTrackLabel.Text = LocalizationManager.Get("NextTrack");
        PrevTrackLabel.Text = LocalizationManager.Get("PreviousTrack");
        VolumeUpLabel.Text = LocalizationManager.Get("VolumeUp");
        VolumeDownLabel.Text = LocalizationManager.Get("VolumeDown");
        MuteLabel.Text = LocalizationManager.Get("Mute");
        HotkeyTipText.Text = LocalizationManager.Get("HotkeyTip");
        LanguageLabel.Text = LocalizationManager.Get("Language");
        SaveButton.Content = LocalizationManager.Get("Save");
        CancelButton.Content = LocalizationManager.Get("Cancel");
    }

    private void LanguageCombo_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (LanguageCombo.SelectedItem is ComboBoxItem item && item.Tag is string lang)
        {
            _selectedLanguage = lang;
            LocalizationManager.SetLanguage(lang);
            UpdateUILanguage();
        }
    }

    private void HotkeyBox_GotFocus(object sender, RoutedEventArgs e)
    {
        _activeBox = sender as TextBox;
        if (_activeBox != null)
        {
            _activeBox.Text = LocalizationManager.Get("PressKey");
            _activeBox.Background = new SolidColorBrush(Color.FromRgb(60, 60, 60));
        }
    }

    private void HotkeyBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        e.Handled = true;
        
        var box = sender as TextBox;
        if (box == null) return;

        if (e.Key == Key.LeftCtrl || e.Key == Key.RightCtrl ||
            e.Key == Key.LeftAlt || e.Key == Key.RightAlt ||
            e.Key == Key.LeftShift || e.Key == Key.RightShift ||
            e.Key == Key.LWin || e.Key == Key.RWin ||
            e.Key == Key.System)
            return;

        var modifiers = Keyboard.Modifiers;
        var key = e.Key == Key.System ? e.SystemKey : e.Key;

        var binding = new HotkeyBinding(modifiers, key);
        box.Text = binding.ToDisplayString();
        box.Background = new SolidColorBrush(Color.FromRgb(61, 61, 61));

        var action = box.Tag?.ToString() switch
        {
            "PlayPause" => HotkeyAction.PlayPause,
            "NextTrack" => HotkeyAction.NextTrack,
            "PreviousTrack" => HotkeyAction.PreviousTrack,
            "VolumeUp" => HotkeyAction.VolumeUp,
            "VolumeDown" => HotkeyAction.VolumeDown,
            "Mute" => HotkeyAction.Mute,
            _ => (HotkeyAction?)null
        };

        if (action.HasValue)
        {
            _hotkeys[action.Value] = binding;
        }

        Keyboard.ClearFocus();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        Saved = true;
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Saved = false;
        Close();
    }
}
