using System.ComponentModel;
using System.Windows;
using System.Windows.Interop;
using MusicController.Helpers;
using MusicController.Models;
using MusicController.Services;

namespace MusicController.Views;

public partial class MainWindow : Window
{
    private readonly AppController _appController;
    private readonly TrayIconManager _trayIconManager;
    private readonly HotkeyManager _hotkeyManager;
    private readonly ISettingsManager _settingsManager;
    private bool _isExiting;
    private bool _startMinimized;

    public MainWindow()
    {
        InitializeComponent();
        
        _settingsManager = new SettingsManager();
        _hotkeyManager = new HotkeyManager();
        var mediaController = new MediaController();
        var toastService = new ToastNotificationService();
        var audioManager = new AudioManager();
        
        _appController = new AppController(_hotkeyManager, mediaController, toastService, _settingsManager, audioManager);
        _trayIconManager = new TrayIconManager();
        
        _startMinimized = _appController.Settings.StartMinimized;
        
        LocalizationManager.SetLanguage(_appController.Settings.Language);
        LocalizationManager.LanguageChanged += (s, e) => UpdateUILanguage();
        
        _trayIconManager.ShowWindowRequested += (s, e) => ShowWindow();
        _trayIconManager.ExitRequested += (s, e) => ExitApplication();
        
        UpdateHotkeyDisplay();
        UpdateUILanguage();
        
        StartupCheckbox.IsChecked = StartupManager.IsStartupEnabled();
        
        Loaded += MainWindow_Loaded;
    }

    private void UpdateUILanguage()
    {
        Title = LocalizationManager.Get("AppTitle");
        AppTitleText.Text = "🎵 " + LocalizationManager.Get("AppTitle");
        HotkeysLabel.Text = LocalizationManager.Get("CurrentHotkeys");
        PlayPauseLabel.Text = LocalizationManager.Get("PlayPause");
        NextTrackLabel.Text = LocalizationManager.Get("NextTrack");
        PrevTrackLabel.Text = LocalizationManager.Get("PreviousTrack");
        VolumeUpLabel.Text = LocalizationManager.Get("VolumeUp");
        VolumeDownLabel.Text = LocalizationManager.Get("VolumeDown");
        MuteLabel.Text = LocalizationManager.Get("Mute");
        StartupCheckbox.Content = LocalizationManager.Get("StartWithWindows");
        MinimizeInfoText.Text = LocalizationManager.Get("MinimizeInfo");
    }

    private void StartupCheckbox_Changed(object sender, RoutedEventArgs e)
    {
        StartupManager.SetStartupEnabled(StartupCheckbox.IsChecked == true);
    }

    private void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
        var settingsWindow = new SettingsWindow(_appController.Settings.Hotkeys, _appController.Settings.Language);
        settingsWindow.Owner = this;
        settingsWindow.ShowDialog();

        if (settingsWindow.Saved)
        {
            _appController.UpdateHotkeys(settingsWindow.Hotkeys);
            _appController.UpdateLanguage(settingsWindow.SelectedLanguage);
            UpdateHotkeyDisplay();
            UpdateUILanguage();
        }
    }

    private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        var helper = new WindowInteropHelper(this);
        helper.EnsureHandle();
        _hotkeyManager.Initialize(this);
        _appController.InitializeAudio(helper.Handle);
        
        await _appController.InitializeAsync();
        _appController.RegisterHotkeys();
        
        _trayIconManager.Initialize();
        
        if (_startMinimized)
        {
            Hide();
        }
    }

    private void UpdateHotkeyDisplay()
    {
        var settings = _appController.Settings;
        
        if (settings.Hotkeys.TryGetValue(HotkeyAction.PlayPause, out var playPause))
            PlayPauseHotkey.Text = playPause.ToDisplayString();
        
        if (settings.Hotkeys.TryGetValue(HotkeyAction.NextTrack, out var next))
            NextTrackHotkey.Text = next.ToDisplayString();
        
        if (settings.Hotkeys.TryGetValue(HotkeyAction.PreviousTrack, out var prev))
            PrevTrackHotkey.Text = prev.ToDisplayString();
        
        if (settings.Hotkeys.TryGetValue(HotkeyAction.VolumeUp, out var volUp))
            VolumeUpHotkey.Text = volUp.ToDisplayString();
        
        if (settings.Hotkeys.TryGetValue(HotkeyAction.VolumeDown, out var volDown))
            VolumeDownHotkey.Text = volDown.ToDisplayString();
        
        if (settings.Hotkeys.TryGetValue(HotkeyAction.Mute, out var mute))
            MuteHotkey.Text = mute.ToDisplayString();
    }

    protected override void OnClosing(CancelEventArgs e)
    {
        if (!_isExiting)
        {
            e.Cancel = true;
            Hide();
        }
        base.OnClosing(e);
    }

    private void ShowWindow()
    {
        Show();
        WindowState = WindowState.Normal;
        Activate();
    }

    private void ExitApplication()
    {
        _isExiting = true;
        _appController.Shutdown();
        _trayIconManager.Dispose();
        System.Windows.Application.Current.Shutdown();
    }
}
