using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace SystemProgramming6;


public partial class MainWindow : Window
{
    private static Mutex _mutex = new Mutex();
    private static int[] _dataArray = Enumerable.Range(1, 10).ToArray();
    private static Random _random = new Random();
    private static TextBlock _outputText;

    public MainWindow()
    {
        InitializeComponent();
        _outputText = OutputText;
    }

    private void StartThreads_Click(object sender, RoutedEventArgs e)
    {
        new Thread(WriteArray).Start();
        new Thread(ModifyArray).Start();
        new Thread(FindMaxValue).Start();
    }

    private void WriteArray()
    {
        _mutex.WaitOne();
        string array = "Array";
        foreach (var el in _dataArray)
        {
            array += " " + el;
        }
        UpdateUi(array);
        Thread.Sleep(500);
        _mutex.ReleaseMutex();
    }

    private void ModifyArray()
    {
        _mutex.WaitOne();
        for (int i = 0; i < _dataArray.Length; i++)
        {
            int randomValue = _random.Next(1, 10);
            _dataArray[i] += randomValue;
            UpdateUi($"Modified: [{i}] - {_dataArray[i]}");
            Thread.Sleep(500);
        }
        _mutex.ReleaseMutex();
    }

    private void FindMaxValue()
    {
        _mutex.WaitOne();
        int maxValue = _dataArray.Max();
        UpdateUi($"Max Value: {maxValue}");
        _mutex.ReleaseMutex();
    }

    private void UpdateUi(string text)
    {
        Dispatcher.Invoke(() =>
        {
            _outputText.Text += text + "\n";
        });
    }
}