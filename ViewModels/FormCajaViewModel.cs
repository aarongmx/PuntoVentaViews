using CorePuntoVenta;
using CorePuntoVenta.Domain.Cajas.Actions;
using CorePuntoVenta.Domain.Ordenes.Data;
using CorePuntoVenta.Domain.Ordenes.Mappers;
using CorePuntoVenta.Domain.Ordenes.Models;
using CorePuntoVenta.Domain.Ventas.Data;
using CorePuntoVenta.Domain.Ventas.Mappers;
using Material.Dialog;
using Microsoft.EntityFrameworkCore;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace PuntoVentaViews.ViewModels
{
    public class FormCajaViewModel : ViewModelBase
    {
        public ICommand ClearErrorCommand { get; }
        private void ClearError()
        {
            if (string.IsNullOrWhiteSpace(MontoPagado))
            {
                return;
            }

            MontoPagado = MontoPagado.Remove(MontoPagado.Length - 1, 1);
            CalcularCambio();
        }

        private string _formaPago;
        public string FormaPago
        {
            get => _formaPago;
            set => this.RaiseAndSetIfChanged(ref _formaPago, value);
        }

        public ICommand PagarCommand { get; }
        private void Pagar()
        {
            CalcularCambio();
        }

        public ICommand BuscarOrdenCommand { get; }
        private void BuscarOrden()
        {
            if (int.TryParse(OrdenId.Trim(), out int id))
            {
                Orden? orden = _context.Ordenes
                    .Where(o => o.Id == id)
                    .Include(o => o.Cliente)
                    .Include(o => o.ItemsOrden)
                    .ThenInclude(o => o.Producto)
                    .AsSplitQuery()
                    .FirstOrDefault();

                if (orden == null)
                {
                    return;
                }

                OrdenMapper mapper = new();
                OrdenSeleccionada = mapper.ToDto(orden);
                OrdenSeleccionada.ItemsOrden?.ToList().ForEach(o =>
                {
                    Debug.WriteLine(o.PrecioUnitario);
                });
            }
            else
            {
                return;
            }
        }

        private string _montoPagado = string.Empty;
        public string MontoPagado
        {
            get => _montoPagado;
            set
            {
                if (double.TryParse(value, out double monto))
                {
                    Cambio = Math.Round(monto - OrdenSeleccionada.Total, 2);
                    this.RaiseAndSetIfChanged(ref _montoPagado, value);
                }
                else
                {
                    _montoPagado = "";
                }
            }
        }

        private double _adeudo = 0;
        public double Adeudo
        {
            get => _adeudo;
            set => this.RaiseAndSetIfChanged(ref _adeudo, value);
        }

        private double _cambio = 0;
        public double Cambio
        {
            get => _cambio;
            set => this.RaiseAndSetIfChanged(ref _cambio, value);
        }

        private OrdenData _ordenSeleccionada;
        public OrdenData OrdenSeleccionada
        {
            get => _ordenSeleccionada;
            set
            {
                Adeudo = value.Total;
                this.RaiseAndSetIfChanged(ref _ordenSeleccionada, value);
            }
        }

        public string _ordenId;
        public string OrdenId
        {
            get => _ordenId;
            set => this.RaiseAndSetIfChanged(ref _ordenId, value);
        }

        private readonly ApplicationDbContext _context;
        public FormCajaViewModel(ApplicationDbContext context)
        {
            _context = context;

            ClearErrorCommand = ReactiveCommand.Create(ClearError);
            PagarCommand = ReactiveCommand.Create(Pagar);
            StoreCommand = ReactiveCommand.Create(Store);
            BuscarOrdenCommand = ReactiveCommand.Create(BuscarOrden);
        }


        private void CalcularCambio()
        {
            _ = double.TryParse(MontoPagado, out double monto);
            Cambio = Math.Round(monto - Adeudo, 2);
        }

        public ICommand StoreCommand { get; }
        private void Store()
        {
            Debug.WriteLine(FormaPago);
            /*try
            {
                _ = double.TryParse(MontoPagado.ToString(), out double montoPagado);
                var ventaData = new VentaData()
                {
                    OrdenId = (int)OrdenSeleccionada.Id!,
                    Total = Math.Round(montoPagado, 2),
                    Iva = 0,
                    Subtotal = Math.Round(montoPagado, 2),
                    SucursalId = 1,
                };

                var ventaMapper = new VentaMapper();
                var ordenMapper = new OrdenMapper();
                var venta = ventaMapper.ToEntity(ventaData);
                var caja = _context.Cajas.First();

                OrdenSeleccionada.EstatusOrdenId = 2;
                var orden = ordenMapper.ToEntity(OrdenSeleccionada);

                caja.EfectivoDisponible += venta.Total;

                _context.Update(caja);
                _context.Update(orden);
                _context.Add(venta);
                _context.SaveChanges();

                DialogHelper.CreateAlertDialog(new AlertDialogBuilderParams()
                {
                    DialogHeaderIcon = Material.Dialog.Icons.DialogIconKind.Success,
                    SupportingText = "Orden pagada correctamente!",
                    ContentHeader = "Pago registrado correctamente!",
                }).Show();

                MontoPagado = "";
                Cambio = 0;

                var data = (new BuscarOrdenesPendientesAction(_context)).Execute();
                //OrdenSeleccionada = null;
            }
            catch (Exception)
            {
                DialogHelper.CreateAlertDialog(new AlertDialogBuilderParams()
                {
                    DialogHeaderIcon = Material.Dialog.Icons.DialogIconKind.Error,
                    SupportingText = "Error al intentar pagar la orden!",
                    ContentHeader = "Error al pagar orden!",
                }).Show();
            }*/
        }

    }
}
