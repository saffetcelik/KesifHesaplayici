using System.Windows;
using KesifUDFGenerator.ViewModels;

namespace KesifUDFGenerator;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;

        // Enable dragging
        MouseLeftButtonDown += (sender, e) => DragMove();
    }

    private void MinimizeButton_Click(object sender, RoutedEventArgs e)
    {
        WindowState = WindowState.Minimized;
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void SettingsButton_Click(object sender, RoutedEventArgs e)
    {
        MessageBox.Show("Ayarlar butonu çalışıyor!", "Debug", MessageBoxButton.OK, MessageBoxImage.Information);
    }
}