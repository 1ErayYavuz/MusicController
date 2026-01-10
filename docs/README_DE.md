# 🎵 Musik-Controller

[![TR](https://img.shields.io/badge/🇹🇷-Türkçe-red)](README_TR.md)
[![EN](https://img.shields.io/badge/🇬🇧-English-blue)](../README.md)
[![RU](https://img.shields.io/badge/🇷🇺-Русский-blue)](README_RU.md)
[![DE](https://img.shields.io/badge/🇩🇪-Deutsch-yellow)](#-musik-controller)

Steuern Sie Ihre Musik mit globalen Tastenkombinationen unter Windows. Funktioniert mit Spotify, YouTube und jedem Mediaplayer.

![Windows](https://img.shields.io/badge/Windows-10%2F11-0078D6?logo=windows)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![License](https://img.shields.io/badge/Lizenz-MIT-green)

## ✨ Funktionen

- 🎹 **Globale Tastenkürzel** - Musik von überall steuern, auch im Hintergrund
- 🔊 **Lautstärkeregelung** - Lauter, leiser und stumm mit Tastenkürzel
- 🔔 **Toast-Benachrichtigungen** - Aktuelle Songinfos in schönen dunklen Popups
- 🎵 **Plattformerkennung** - Zeigt welche App abspielt (Spotify, Chrome, YouTube, etc.)
- 🌍 **Mehrsprachig** - Türkisch, Englisch, Russisch, Deutsch
- ⚙️ **Anpassbar** - Eigene Tastenkombinationen festlegen
- 📌 **Systemtray** - Läuft leise im Hintergrund
- 🚀 **Portabel** - Keine Installation erforderlich, einzelne EXE-Datei

## 📥 Download

**[⬇️ Neueste Version herunterladen](https://github.com/1ErayYavuz/MusicController/releases/latest)**

## 🎮 Standard-Tastenkürzel

| Aktion | Tastenkürzel |
|--------|--------------|
| Abspielen/Pause | `Ctrl + Alt + Space` |
| Nächster Titel | `Ctrl + Alt + →` |
| Vorheriger Titel | `Ctrl + Alt + ←` |
| Lauter | `Ctrl + Alt + ↑` |
| Leiser | `Ctrl + Alt + ↓` |
| Stumm | `Ctrl + M` |

> Sie können diese in den Einstellungen (⚙️ Button) anpassen

## 🚀 Verwendung

1. Laden Sie `MusicController.exe` von [Releases](https://github.com/1ErayYavuz/MusicController/releases) herunter
2. Führen Sie die Datei aus
3. **Die App startet im Systemtray** - suchen Sie das Musiksymbol in der Taskleiste
4. Verwenden Sie Tastenkürzel zur Musiksteuerung
5. Klicken Sie auf das Tray-Symbol für Einstellungen

## 🔧 Aus Quellcode erstellen

```bash
git clone https://github.com/1ErayYavuz/MusicController.git
cd MusicController
dotnet build
dotnet run --project MusicController
```

### Als einzelne EXE veröffentlichen:
```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

## 📋 Anforderungen

- Windows 10/11
- Keine zusätzlichen Abhängigkeiten (self-contained)

## 📄 Lizenz

MIT-Lizenz - frei verwenden und modifizieren.

---

Mit ❤️ erstellt mit WPF und .NET 8
