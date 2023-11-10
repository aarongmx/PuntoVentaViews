using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using PuntoVentaViews.ViewModels;

namespace PuntoVentaViews.Views;

public partial class LoginWindowView : Window
{
    public LoginWindowView()
    {
        InitializeComponent();
    }

    public void Login(object? sender, RoutedEventArgs args)
    {
        var window = new MainWindow
        {
            DataContext = new MainWindowViewModel(new CorePuntoVenta.ApplicationDbContext())
        };
        window.Show();
        Close();
    }
}