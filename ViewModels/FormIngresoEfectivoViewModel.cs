using CorePuntoVenta.Domain.Cajas.Actions;
using CorePuntoVenta.Domain.Cajas.Data;
using CorePuntoVenta.Domain.Cajas.Mappers;
using CorePuntoVenta.Domain.Cajas.Models;
using Material.Dialog;
using ReactiveUI;
using System;
using System.Windows.Input;

namespace PuntoVentaViews.ViewModels
{
    public class FormIngresoEfectivoViewModel : ViewModelBase
    {
        private string _monto;
        public string Monto
        {
            get => _monto;
            set => this.RaiseAndSetIfChanged(ref _monto, value);
        }

        private string _motivo;
        public string Motivo
        {
            get => _motivo;
            set => this.RaiseAndSetIfChanged(ref _motivo, value);
        }

        public ICommand GuardarCommand { get; }
        private void Guardar()
        {
            try
            {
                _ = double.TryParse(Monto, out double monto);
                ItemCajaData itemCaja = new()
                {
                    Monto = Math.Round(monto, 2),
                    Motivo = Motivo,
                    Movimiento = CorePuntoVenta.Domain.Cajas.Enums.MovimientoCaja.INGRESO,
                    EmpleadoId = 1,
                };

                ItemCaja? item = new AgregarEfectivoAction(new CorePuntoVenta.ApplicationDbContext()).Execute(1, itemCaja);

                if(item is not null)
                {
                    ItemCajaMapper mapper = new();
                    new GenerarTicketEntradaSalidaEfectivoAction().Execute("POS-80C", mapper.ToDto(item));
                }

                Monto = "";
                Motivo = "";

                DialogHelper.CreateAlertDialog(new AlertDialogBuilderParams()
                {
                    Borderless = true,
                    DialogHeaderIcon = Material.Dialog.Icons.DialogIconKind.Success,
                    ContentHeader = "Efectivo agregado correctamente!",
                    SupportingText = "Se agrego correctamente el efectivo a caja!"
                }).Show();
            }
            catch (Exception)
            {
                DialogHelper.CreateAlertDialog(new AlertDialogBuilderParams()
                {
                    Borderless = true,
                    DialogHeaderIcon = Material.Dialog.Icons.DialogIconKind.Error,
                    ContentHeader = "No fue posible agregar el efectivo a caja!",
                    SupportingText = "Error al intentar agregar efectivo a caja!"
                }).Show();
                throw;
            }
            
        }

        public FormIngresoEfectivoViewModel()
        {
            GuardarCommand = ReactiveCommand.Create(Guardar);
        }

    }
}
