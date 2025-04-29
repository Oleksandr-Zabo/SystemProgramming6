using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SystemProgramming6;


public partial class MainWindow : Window
{
    private static Mutex _mutex = new Mutex();
    private static int[] _dataArray = Enumerable.Range(1, 10).ToArray();
    private static int _maxValue;
    private static Random _random = new Random();
    private static TextBlock _outputText;

    public MainWindow()
    {
        InitializeComponent();
        _outputText = OutputText;
    }

    private void StartThreads_Click(object sender, RoutedEventArgs e)
    {
        Thread firstThread = new Thread(ModifyArray);
        Thread secondThread = new Thread(FindMaxValue);

        firstThread.Start();
        secondThread.Start();

        // Wait for both threads to finish
        firstThread.Join();
        secondThread.Join();

        Dispatcher.Invoke(() => MainThread_UpdateUI());
    }

    private void ModifyArray()
    {
        _mutex.WaitOne();
        for (int i = 0; i < _dataArray.Length; i++)
        {
            int randomValue = _random.Next(1, 10);
            _dataArray[i] += randomValue;
            Thread.Sleep(200);
        }
        _mutex.ReleaseMutex();
    }

    private void FindMaxValue()
    {
        _mutex.WaitOne();
        _maxValue = _dataArray.Max();
        Thread.Sleep(200);
        _mutex.ReleaseMutex();
    }

    private void MainThread_UpdateUI()
    {
        _outputText.Text += $"Modified Array: {string.Join(", ", _dataArray)}\n";
        _outputText.Text += $"Max Value: {_maxValue}\n";
    }
}