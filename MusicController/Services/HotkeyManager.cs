using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using MusicController.Helpers;
using MusicController.Models;

namespace MusicController.Services;

public class HotkeyManager : IHotkeyManager
{
    private readonly Dictionary<HotkeyAction, HotkeyBinding> _registeredHotkeys = new();
    private readonly Dictionary<int, HotkeyAction> _idToAction = new();
    private IntPtr _windowHandle;
    private HwndSource? _source;
    private int _nextId = 1;
    private bool _disposed;

    public event EventHandler<HotkeyEventArgs>? HotkeyPressed;

    public void Initialize(Window window)
    {
        var helper = new WindowInteropHelper(window);
        _windowHandle = helper.Handle;
        _source = HwndSource.FromHwnd(_windowHandle);
        _source?.AddHook(WndProc);
    }

    public bool RegisterHotkey(HotkeyAction action, HotkeyBinding binding)
    {
        if (_windowHandle == IntPtr.Zero) return false;
        
        // Unregister existing hotkey for this action
        UnregisterHotkey(action);
        
        var id = _nextId++;
        var modifiers = ConvertModifiers(binding.Modifiers);
        var vk = KeyInterop.VirtualKeyFromKey(binding.Key);
        
        if (NativeMethods.RegisterHotKey(_windowHandle, id, modifiers | NativeMethods.MOD_NOREPEAT, (uint)vk))
        {
            _registeredHotkeys[action] = binding;
            _idToAction[id] = action;
            return true;
        }
        
        return false;
    }

    public bool UnregisterHotkey(HotkeyAction action)
    {
        if (!_registeredHotkeys.ContainsKey(action)) return true;
        
        var id = _idToAction.FirstOrDefault(x => x.Value == action).Key;
        if (id != 0 && NativeMethods.UnregisterHotKey(_windowHandle, id))
        {
            _registeredHotkeys.Remove(action);
            _idToAction.Remove(id);
            return true;
        }
        
        return false;
    }

    public IReadOnlyDictionary<HotkeyAction, HotkeyBinding> GetRegisteredHotkeys()
    {
        return _registeredHotkeys.AsReadOnly();
    }

    public void RegisterAllHotkeys(Dictionary<HotkeyAction, HotkeyBinding> hotkeys)
    {
        foreach (var kvp in hotkeys)
        {
            RegisterHotkey(kvp.Key, kvp.Value);
        }
    }

    public void UnregisterAllHotkeys()
    {
        foreach (var action in _registeredHotkeys.Keys.ToList())
        {
            UnregisterHotkey(action);
        }
    }

    private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
    {
        if (msg == NativeMethods.WM_HOTKEY)
        {
            var id = wParam.ToInt32();
            if (_idToAction.TryGetValue(id, out var action))
            {
                HotkeyPressed?.Invoke(this, new HotkeyEventArgs { Action = action });
                handled = true;
            }
        }
        return IntPtr.Zero;
    }

    private static uint ConvertModifiers(ModifierKeys modifiers)
    {
        uint result = NativeMethods.MOD_NONE;
        
        if (modifiers.HasFlag(ModifierKeys.Alt))
            result |= NativeMethods.MOD_ALT;
        if (modifiers.HasFlag(ModifierKeys.Control))
            result |= NativeMethods.MOD_CONTROL;
        if (modifiers.HasFlag(ModifierKeys.Shift))
            result |= NativeMethods.MOD_SHIFT;
        if (modifiers.HasFlag(ModifierKeys.Windows))
            result |= NativeMethods.MOD_WIN;
        
        return result;
    }

    public void Dispose()
    {
        if (_disposed) return;
        
        UnregisterAllHotkeys();
        _source?.RemoveHook(WndProc);
        _disposed = true;
    }
}
