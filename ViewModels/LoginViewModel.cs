using Avalonia.Controls;
using CorePuntoVenta;
using CorePuntoVenta.Domain.Administracion.Data;
using CorePuntoVenta.Domain.Helpers;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PuntoVentaViews.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {

        private string? _email;
        private string? _password;

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

        public ReactiveCommand<Unit, Unit> LoginCommand { get; }

        private readonly ApplicationDbContext _context;
        public LoginViewModel(ApplicationDbContext context)
        {
            _context = context;
            LoginCommand = ReactiveCommand.Create(Login);
        }

        private bool _isLoggedIn = false;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set => this.RaiseAndSetIfChanged(ref _isLoggedIn, value);
        }

        public void Login()
        {
            try
            {
                UsuarioData usuarioData = new() { Correo = Email!, Password = Password! };
                Session? session = (new CorePuntoVenta.Domain.Administracion.Actions.LoginAction(_context)).Execute(usuarioData);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error {ex.Message}");
            }
        }
    }
}
