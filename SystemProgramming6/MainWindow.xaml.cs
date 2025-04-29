using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SystemProgramming6;


public partial class MainWindow : Window
{private static Mutex mutex = new Mutex();
    private static TextBlock outputText;

    public MainWindow()
    {
        InitializeComponent();
        outputText = OutputText;
    }

    private void StartThreads_Click(object sender, RoutedEventArgs e)
    {
        new Thread(ShowAscendingNumbers).Start();
        new Thread(ShowDescendingNumbers).Start();
    }

    private void ShowAscendingNumbers()
    {
        mutex.WaitOne();
        for (int i = 0; i <= 20; i++)
        {
            UpdateUI($"Ascending: {i}");
            Thread.Sleep(500);
        }
        mutex.ReleaseMutex();
    }

    private void ShowDescendingNumbers()
    {
        mutex.WaitOne();
        for (int i = 10; i >= 0; i--)
        {
            UpdateUI($"Descending: {i}");
            Thread.Sleep(500);
        }
        mutex.ReleaseMutex();
    }

    private void UpdateUI(string text)
    {
        Dispatcher.Invoke(() =>
        {
            outputText.Text += text + "\n";
        });
    }
}