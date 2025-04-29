using System.Windows;
using System.Windows.Controls;

namespace SystemProgramming6;


public partial class MainWindow : Window
{
    private static Semaphore _semaphore = new Semaphore(3, 3); // Allows 3 threads at a time
    private static Random _random = new Random();
    private static TextBlock? _outputText;

    public MainWindow()
    {
        InitializeComponent();
        _outputText = OutputText;
    }

    private void StartThreads_Click(object sender, RoutedEventArgs e)
    {
        for (int i = 0; i < 10; i++)
        {
            new Thread(GenerateRandomNumbers).Start(i);
        }
    }

    private void GenerateRandomNumbers(object? threadId)
    {
        _semaphore.WaitOne();
        UpdateUi($"Thread {threadId} started.");

        int[] randomNumbers = Enumerable.Range(0, 5).Select(_ => _random.Next(1, 100)).ToArray();
        UpdateUi($"Thread {threadId}: {string.Join(", ", randomNumbers)}");

        Thread.Sleep(2000); // Simulating processing time
        UpdateUi($"Thread {threadId} finished.");

        _semaphore.Release();
    }

    private void UpdateUi(string text)
    {
        Dispatcher.Invoke(() =>
        {
            if (_outputText != null) _outputText.Text += text + "\n";
        });
    }
}