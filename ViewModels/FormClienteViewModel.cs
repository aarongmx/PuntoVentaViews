using CorePuntoVenta.Domain.Clientes.Actions;
using CorePuntoVenta.Domain.Clientes.Data;
using CorePuntoVenta.Domain.Direcciones.Actions;
using CorePuntoVenta.Domain.Direcciones.Data;
using Material.Dialog;
using Material.Dialog.Icons;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PuntoVentaViews.ViewModels
{
    public class FormClienteViewModel : ViewModelBase
    {
        private string _rfc = string.Empty;

        [Required]
        [RegularExpression(@"^([A-ZÑa-zñ]|&){3,4}[0-9]{2}(0[1-9]|1[0-2])([12][0-9]|0[1-9]|3[01])[A-Z0-9]{3}$", ErrorMessage = "Characters are not allowed.")]
        public string Rfc
        {
            get => _rfc;
            set => this.RaiseAndSetIfChanged(ref _rfc, value);
        }

        private string _razonSocial = string.Empty;

        [Required]
        public string RazonSocial
        {
            get => _razonSocial;
            set => this.RaiseAndSetIfChanged(ref _razonSocial, value);
        }

        private string _nombreComercial = string.Empty;
        public string NombreComercial
        {
            get => _nombreComercial;
            set => this.RaiseAndSetIfChanged(ref _nombreComercial, value);
        }

        private string _codigoPostal = string.Empty;

        [Required]
        [MinLength(5)]
        [MaxLength(5)]
        public string CodigoPostal
        {
            get => _codigoPostal;
            set => this.RaiseAndSetIfChanged(ref _codigoPostal, value);
        }

        private string _numeroExterior = string.Empty;

        [Required]
        public string NumeroExterior
        {
            get => _numeroExterior;
            set => this.RaiseAndSetIfChanged(ref _numeroExterior, value);
        }

        private string _calle = string.Empty;
        public string Calle
        {
            get => _calle;
            set => this.RaiseAndSetIfChanged(ref _calle, value);
        }

        private string _numeroInterior = string.Empty;
        public string NumeroInterior
        {
            get => _numeroInterior;
            set => this.RaiseAndSetIfChanged(ref _numeroInterior, value);
        }

        public ICommand SaveCommand { get; }
        private void Save()
        {
            try
            {
                DireccionData direccionData = new()
                {
                    CodigoPostal = CodigoPostal,
                    Calle = Calle,
                    NumeroExterior = NumeroExterior,
                    NumeroInterior = NumeroInterior,
                };

                ClienteData clienteData = new () { NombreComercial = NombreComercial, Rfc = Rfc, RazonSocial = RazonSocial };

                var cliente = (new StoreClienteAction(new CorePuntoVenta.ApplicationDbContext())).Execute(clienteData, direccionData);
                Clear();

                var dialog = DialogHelper.CreateAlertDialog(new AlertDialogBuilderParams
                {
                    WindowTitle = "CLIENTE GUARDADO",
                    DialogHeaderIcon = DialogIconKind.Success,
                    SupportingText = "¡Cliente guardado correctamente!",
                    Borderless = true,
                    Width = 264,
                    DialogIcon = DialogIconKind.Success,
                });
                dialog.Show();
            }
            catch (Exception ex)
            {
                var dialog = DialogHelper.CreateAlertDialog(new AlertDialogBuilderParams
                {
                    WindowTitle = "Error",
                    DialogHeaderIcon = DialogIconKind.Success,
                    SupportingText = ex.Message,
                });
                dialog.Show();
                Debug.WriteLine($"Could not save {ex.Message}");
            }
        }

        private void Clear()
        {
            CodigoPostal = string.Empty;
            Calle = string.Empty;
            NumeroExterior = string.Empty;
            NumeroInterior = string.Empty;
            NombreComercial = string.Empty;
            Rfc = string.Empty;
            RazonSocial = string.Empty;
        }

        public FormClienteViewModel()
        {
            SaveCommand = ReactiveCommand.Create(Save);
        }


    }
}
