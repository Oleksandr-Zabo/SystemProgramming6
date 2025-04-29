using System.Windows;

namespace SystemProgramming6;

public partial class App : Application
{
    private static Mutex mutex;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        bool isNewInstance;
        mutex = new Mutex(true, "SystemProgramming6Mutex", out isNewInstance);

        if (!isNewInstance)
        {
            MessageBox.Show("The application is already running!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            Shutdown();
        }
    }
}