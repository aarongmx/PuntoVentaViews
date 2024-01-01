using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using CorePuntoVenta;
using CorePuntoVenta.Domain.Administracion.Data;
using CorePuntoVenta.Domain.Helpers;
using PuntoVentaViews.Views;
using ReactiveUI;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Windows.Input;

namespace PuntoVentaViews.ViewModels
{
    public class LoginWindowViewModel : ViewModelBase
    {
        private string? _email;
        private string? _password;
        private readonly ApplicationDbContext _context;

        [Required(ErrorMessage = "El campo correo es requerido.")]
        public string? Email
        {
            get => _email;
            set => this.RaiseAndSetIfChanged(ref _email, value);
        }

        [Required(ErrorMessage = "El campo contraseña es requerido.")]
        public string? Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public ICommand LoginCommand { get; }

        private void Login()
        {
            try
            {
                UsuarioData usuarioData = new() { Correo = Email!, Password = Password! };
                Session? session = (new CorePuntoVenta.Domain.Administracion.Actions.LoginAction(_context)).Execute(usuarioData);

                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
                {
                    var currentWindow = desktop.MainWindow;
                    desktop.MainWindow = new MainWindow()
                    {
                        DataContext = new MainWindowViewModel(_context)
                    };
                    desktop.MainWindow.Show();
                    currentWindow?.Close();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error {ex.Message}");
            }
            
        }

        public LoginWindowViewModel(ApplicationDbContext context)
        {
            _context = context;
            LoginCommand = ReactiveCommand.Create(Login);
        }
    }
}
