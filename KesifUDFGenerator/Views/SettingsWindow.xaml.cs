using System.Windows;
using KesifUDFGenerator.ViewModels;

namespace KesifUDFGenerator.Views;

/// <summary>
/// Interaction logic for SettingsWindow.xaml
/// </summary>
public partial class SettingsWindow : Window
{
    public SettingsWindow(SettingsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;

        // Pencere kapatma action'ını ayarla
        viewModel.CloseWindow = () => Close();

        // Enable dragging
        MouseLeftButtonDown += (sender, e) => DragMove();
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
