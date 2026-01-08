using System.Drawing;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace MusicController.Services;

public class TrayIconManager : ITrayIconManager
{
    private NotifyIcon? _notifyIcon;
    private bool _disposed;

    public event EventHandler? ExitRequested;
    public event EventHandler? ShowWindowRequested;

    public void Initialize()
    {
        _notifyIcon = new NotifyIcon
        {
            Icon = LoadAppIcon(),
            Text = "Music Controller",
            Visible = true,
            ContextMenuStrip = CreateContextMenu()
        };

        _notifyIcon.Click += (s, e) =>
        {
            if (e is MouseEventArgs me && me.Button == MouseButtons.Left)
            {
                ShowWindowRequested?.Invoke(this, EventArgs.Empty);
            }
        };
    }

    private static Icon LoadAppIcon()
    {
        try
        {
            var exePath = Environment.ProcessPath;
            if (!string.IsNullOrEmpty(exePath))
            {
                return Icon.ExtractAssociatedIcon(exePath) ?? SystemIcons.Application;
            }
        }
        catch { }
        return SystemIcons.Application;
    }

    private ContextMenuStrip CreateContextMenu()
    {
        var menu = new ContextMenuStrip();
        
        var showItem = new ToolStripMenuItem(LocalizationManager.Get("Show"));
        showItem.Click += (s, e) => ShowWindowRequested?.Invoke(this, EventArgs.Empty);
        
        var exitItem = new ToolStripMenuItem(LocalizationManager.Get("Exit"));
        exitItem.Click += (s, e) => ExitRequested?.Invoke(this, EventArgs.Empty);
        
        menu.Items.Add(showItem);
        menu.Items.Add(new ToolStripSeparator());
        menu.Items.Add(exitItem);
        
        return menu;
    }

    public void ShowBalloonTip(string title, string message)
    {
        _notifyIcon?.ShowBalloonTip(2000, title, message, ToolTipIcon.Info);
    }

    public void Dispose()
    {
        if (_disposed) return;
        
        if (_notifyIcon != null)
        {
            _notifyIcon.Visible = false;
            _notifyIcon.Dispose();
        }
        
        _disposed = true;
    }
}
