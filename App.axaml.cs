using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CorePuntoVenta;
using PuntoVentaViews.ViewModels;
using PuntoVentaViews.Views;

namespace PuntoVentaViews
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                ApplicationDbContext context = new ApplicationDbContext();
                desktop.MainWindow = new LoginWindowView
                {
                    DataContext = new LoginWindowViewModel(context),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}