# 🎵 Music Controller

[![TR](https://img.shields.io/badge/🇹🇷-Türkçe-red)](docs/README_TR.md)
[![EN](https://img.shields.io/badge/🇬🇧-English-blue)](#-music-controller)
[![RU](https://img.shields.io/badge/🇷🇺-Русский-blue)](docs/README_RU.md)
[![DE](https://img.shields.io/badge/🇩🇪-Deutsch-yellow)](docs/README_DE.md)

Control your music with global hotkeys on Windows. Works with Spotify, YouTube, and any media player.

![Windows](https://img.shields.io/badge/Windows-10%2F11-0078D6?logo=windows)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![License](https://img.shields.io/badge/License-MIT-green)

## ✨ Features

- 🎹 **Global Hotkeys** - Control music from anywhere, even when app is in background
- 🔔 **Toast Notifications** - See current song info with beautiful dark-themed popups
- 🌍 **Multi-language** - Turkish, English, Russian, German
- ⚙️ **Customizable** - Set your own hotkey combinations
- 📌 **System Tray** - Runs quietly in the background
- 🚀 **Portable** - No installation required, single EXE file

## 📥 Download

**[⬇️ Download Latest Release](https://github.com/1ErayYavuz/MusicController/releases/latest)**

## 🎮 Default Hotkeys

| Action | Hotkey |
|--------|--------|
| Play/Pause | `Ctrl + Alt + Space` |
| Next Track | `Ctrl + Alt + →` |
| Previous Track | `Ctrl + Alt + ←` |

> You can customize these in Settings (⚙️ button)

## 🖼️ Screenshots

*Coming soon*

## 🚀 Usage

1. Download `MusicController.exe` from [Releases](https://github.com/1ErayYavuz/MusicController/releases)
2. Run the executable
3. Use hotkeys to control your music
4. Click the tray icon to open settings

## 🔧 Building from Source

```bash
git clone https://github.com/1ErayYavuz/MusicController.git
cd MusicController
dotnet build
dotnet run --project MusicController
```

### Publish as single EXE:
```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

## 📋 Requirements

- Windows 10/11
- No additional dependencies (self-contained)

## 📄 License

MIT License - feel free to use and modify.

---

Made with ❤️ using WPF and .NET 8
