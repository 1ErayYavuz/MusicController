# 🎵 Müzik Kontrolü

[![TR](https://img.shields.io/badge/🇹🇷-Türkçe-red)](#-müzik-kontrolü)
[![EN](https://img.shields.io/badge/🇬🇧-English-blue)](../README.md)
[![RU](https://img.shields.io/badge/🇷🇺-Русский-blue)](README_RU.md)
[![DE](https://img.shields.io/badge/🇩🇪-Deutsch-yellow)](README_DE.md)

Windows'ta global kısayol tuşlarıyla müziğinizi kontrol edin. Spotify, YouTube ve tüm medya oynatıcılarla çalışır.

![Windows](https://img.shields.io/badge/Windows-10%2F11-0078D6?logo=windows)
![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?logo=dotnet)
![License](https://img.shields.io/badge/Lisans-MIT-green)

## ✨ Özellikler

- 🎹 **Global Kısayollar** - Uygulama arka planda bile olsa müziği kontrol edin
- 🔊 **Ses Kontrolü** - Kısayollarla ses aç, kıs ve sessiz
- 🔔 **Toast Bildirimleri** - Şık karanlık temalı popup'larla şarkı bilgisini görün
- 🎵 **Platform Algılama** - Hangi uygulamadan çaldığını gösterir (Spotify, Chrome, YouTube, vs.)
- 🌍 **Çoklu Dil** - Türkçe, İngilizce, Rusça, Almanca
- ⚙️ **Özelleştirilebilir** - Kendi kısayol kombinasyonlarınızı ayarlayın
- 📌 **Sistem Tepsisi** - Arka planda sessizce çalışır
- 🚀 **Taşınabilir** - Kurulum gerektirmez, tek EXE dosyası

## 📥 İndir

**[⬇️ Son Sürümü İndir](https://github.com/1ErayYavuz/MusicController/releases/latest)**

## 🎮 Varsayılan Kısayollar

| İşlem | Kısayol |
|-------|---------|
| Oynat/Duraklat | `Ctrl + Alt + Space` |
| Sonraki Şarkı | `Ctrl + Alt + →` |
| Önceki Şarkı | `Ctrl + Alt + ←` |
| Ses Aç | `Ctrl + Alt + ↑` |
| Ses Kıs | `Ctrl + Alt + ↓` |
| Sessiz | `Ctrl + M` |

> Bunları Ayarlar'dan (⚙️ butonu) özelleştirebilirsiniz

## 🚀 Kullanım

1. [Releases](https://github.com/1ErayYavuz/MusicController/releases) sayfasından `MusicController.exe` indirin
2. Çalıştırın
3. **Uygulama sistem tepsisinde başlar** - görev çubuğunda müzik simgesini arayın
4. Kısayol tuşlarıyla müziğinizi kontrol edin
5. Ayarları açmak için tepsi simgesine tıklayın

## 🔧 Kaynak Koddan Derleme

```bash
git clone https://github.com/1ErayYavuz/MusicController.git
cd MusicController
dotnet build
dotnet run --project MusicController
```

### Tek EXE olarak yayınlama:
```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true -o publish
```

## 📋 Gereksinimler

- Windows 10/11
- Ek bağımlılık yok (self-contained)

## 📄 Lisans

MIT Lisansı - özgürce kullanın ve değiştirin.

---

WPF ve .NET 8 ile ❤️ yapıldı
