using System.Windows;

namespace MusicController;

public partial class App : System.Windows.Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        AppDomain.CurrentDomain.UnhandledException += (s, args) =>
        {
            var ex = args.ExceptionObject as Exception;
            System.Windows.MessageBox.Show($"Hata: {ex?.Message}\n\n{ex?.StackTrace}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
        };

        DispatcherUnhandledException += (s, args) =>
        {
            System.Windows.MessageBox.Show($"Hata: {args.Exception.Message}\n\n{args.Exception.StackTrace}", "Hata", MessageBoxButton.OK, MessageBoxImage.Error);
            args.Handled = true;
        };

        base.OnStartup(e);
    }
}
