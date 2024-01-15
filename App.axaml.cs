using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CorePuntoVenta;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Npgsql;
using PuntoVentaViews.ViewModels;
using PuntoVentaViews.Views;
using System;

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
                var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
                string connection = "Server=127.0.0.1;Port=5432;User ID=db_user;Password=s3cret;Database=punto";
                NpgsqlDataSourceBuilder dataSourceBuilder = new(connection);

                dataSourceBuilder.UseNodaTime();

                var dataSource = dataSourceBuilder.Build();

                builder
                    .UseNpgsql(dataSource)
                    .LogTo(Console.WriteLine, LogLevel.Information)
                    .EnableSensitiveDataLogging()
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);

                ApplicationDbContext context = new(builder.Options);
                desktop.MainWindow = new LoginWindowView
                {
                    DataContext = new LoginWindowViewModel(context),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}