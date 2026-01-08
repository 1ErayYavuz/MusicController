namespace MusicController.Services;

public static class LocalizationManager
{
    private static string _currentLanguage = "en";
    
    private static readonly Dictionary<string, Dictionary<string, string>> Translations = new()
    {
        ["tr"] = new()
        {
            ["AppTitle"] = "Müzik Kontrolü",
            ["Settings"] = "Ayarlar",
            ["HotkeySettings"] = "⚙️ Kısayol Tuşları Ayarları",
            ["PlayPause"] = "Oynat/Duraklat",
            ["NextTrack"] = "Sonraki Şarkı",
            ["PreviousTrack"] = "Önceki Şarkı",
            ["PressKey"] = "Tuşa basın...",
            ["HotkeyTip"] = "💡 Kutuya tıklayıp yeni kısayol tuşuna basın",
            ["Save"] = "Kaydet",
            ["Cancel"] = "İptal",
            ["Show"] = "Göster",
            ["Exit"] = "Çıkış",
            ["Language"] = "Dil",
            ["ToastPlayPause"] = "⏯️ Oynat/Duraklat",
            ["ToastNextTrack"] = "⏭️ Sonraki Şarkı",
            ["ToastPrevTrack"] = "⏮️ Önceki Şarkı",
            ["ToastNoMedia"] = "❌ Aktif medya bulunamadı",
            ["ToastDefault"] = "🎵 Müzik Kontrolü",
            ["CurrentHotkeys"] = "Mevcut Kısayollar",
            ["UnknownArtist"] = "Bilinmeyen Sanatçı",
            ["UnknownTitle"] = "Bilinmeyen Şarkı",
            ["StartWithWindows"] = "Windows ile başlat",
            ["MinimizeInfo"] = "Pencereyi kapatınca sistem tepsisine küçülür"
        },
        ["en"] = new()
        {
            ["AppTitle"] = "Music Controller",
            ["Settings"] = "Settings",
            ["HotkeySettings"] = "⚙️ Hotkey Settings",
            ["PlayPause"] = "Play/Pause",
            ["NextTrack"] = "Next Track",
            ["PreviousTrack"] = "Previous Track",
            ["PressKey"] = "Press a key...",
            ["HotkeyTip"] = "💡 Click the box and press a new hotkey",
            ["Save"] = "Save",
            ["Cancel"] = "Cancel",
            ["Show"] = "Show",
            ["Exit"] = "Exit",
            ["Language"] = "Language",
            ["ToastPlayPause"] = "⏯️ Play/Pause",
            ["ToastNextTrack"] = "⏭️ Next Track",
            ["ToastPrevTrack"] = "⏮️ Previous Track",
            ["ToastNoMedia"] = "❌ No active media found",
            ["ToastDefault"] = "🎵 Music Controller",
            ["CurrentHotkeys"] = "Current Hotkeys",
            ["UnknownArtist"] = "Unknown Artist",
            ["UnknownTitle"] = "Unknown Title",
            ["StartWithWindows"] = "Start with Windows",
            ["MinimizeInfo"] = "Minimizes to system tray when closed"
        },
        ["ru"] = new()
        {
            ["AppTitle"] = "Музыка",
            ["Settings"] = "Настройки",
            ["HotkeySettings"] = "⚙️ Горячие клавиши",
            ["PlayPause"] = "Воспр./Пауза",
            ["NextTrack"] = "Следующий",
            ["PreviousTrack"] = "Предыдущий",
            ["PressKey"] = "Нажмите...",
            ["HotkeyTip"] = "💡 Нажмите на поле и введите клавишу",
            ["Save"] = "Сохранить",
            ["Cancel"] = "Отмена",
            ["Show"] = "Показать",
            ["Exit"] = "Выход",
            ["Language"] = "Язык",
            ["ToastPlayPause"] = "⏯️ Воспр./Пауза",
            ["ToastNextTrack"] = "⏭️ Следующий",
            ["ToastPrevTrack"] = "⏮️ Предыдущий",
            ["ToastNoMedia"] = "❌ Медиа не найдено",
            ["ToastDefault"] = "🎵 Музыка",
            ["CurrentHotkeys"] = "Горячие клавиши",
            ["UnknownArtist"] = "Неизвестный",
            ["UnknownTitle"] = "Неизвестно",
            ["StartWithWindows"] = "Запуск с Windows",
            ["MinimizeInfo"] = "Сворачивается в трей при закрытии"
        },
        ["de"] = new()
        {
            ["AppTitle"] = "Musik-Controller",
            ["Settings"] = "Einstellungen",
            ["HotkeySettings"] = "⚙️ Tastenkürzel",
            ["PlayPause"] = "Abspielen/Pause",
            ["NextTrack"] = "Nächster Titel",
            ["PreviousTrack"] = "Vorheriger Titel",
            ["PressKey"] = "Taste drücken...",
            ["HotkeyTip"] = "💡 Klicken und neue Taste drücken",
            ["Save"] = "Speichern",
            ["Cancel"] = "Abbrechen",
            ["Show"] = "Anzeigen",
            ["Exit"] = "Beenden",
            ["Language"] = "Sprache",
            ["ToastPlayPause"] = "⏯️ Abspielen/Pause",
            ["ToastNextTrack"] = "⏭️ Nächster Titel",
            ["ToastPrevTrack"] = "⏮️ Vorheriger Titel",
            ["ToastNoMedia"] = "❌ Kein Medium gefunden",
            ["ToastDefault"] = "🎵 Musik-Controller",
            ["CurrentHotkeys"] = "Aktuelle Tastenkürzel",
            ["UnknownArtist"] = "Unbekannt",
            ["UnknownTitle"] = "Unbekannt",
            ["StartWithWindows"] = "Mit Windows starten",
            ["MinimizeInfo"] = "Minimiert in Taskleiste beim Schließen"
        }
    };

    public static event EventHandler? LanguageChanged;

    public static string CurrentLanguage => _currentLanguage;

    public static void SetLanguage(string langCode)
    {
        if (Translations.ContainsKey(langCode))
        {
            _currentLanguage = langCode;
            LanguageChanged?.Invoke(null, EventArgs.Empty);
        }
    }

    public static string Get(string key)
    {
        if (Translations.TryGetValue(_currentLanguage, out var lang) && lang.TryGetValue(key, out var value))
            return value;
        if (Translations["en"].TryGetValue(key, out var fallback))
            return fallback;
        return key;
    }

    public static string GetLanguageDisplayName(string langCode) => langCode switch
    {
        "tr" => "Türkçe",
        "en" => "English",
        "ru" => "Русский",
        "de" => "Deutsch",
        _ => langCode
    };

    public static string[] AvailableLanguages => ["tr", "en", "ru", "de"];
}
