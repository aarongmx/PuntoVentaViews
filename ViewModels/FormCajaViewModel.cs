using Avalonia.Controls;
using CorePuntoVenta;
using CorePuntoVenta.Domain.Cajas.Actions;
using CorePuntoVenta.Domain.Ordenes.Data;
using CorePuntoVenta.Domain.Ordenes.Mappers;
using CorePuntoVenta.Domain.Ordenes.Models;
using CorePuntoVenta.Domain.Ventas.Data;
using CorePuntoVenta.Domain.Ventas.Mappers;
using DialogHostAvalonia;
using Material.Dialog;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PuntoVentaViews.ViewModels
{
    public class FormCajaViewModel : ViewModelBase
    {
        private string _searchOrden;
        public string SearchOrden
        {
            get => _searchOrden;
            set => this.RaiseAndSetIfChanged(ref _searchOrden, value);
        }

        public ICommand BuildNumberCommand { get; }
        private void BuildNumber(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            MontoPagado += value;
            CalcularCambio();
        }

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

        public ICommand PagarCommand { get; }
        private void Pagar()
        {
            CalcularCambio();
        }

        private string _montoPagado = string.Empty;
        public string MontoPagado
        {
            get => _montoPagado;
            set => this.RaiseAndSetIfChanged(ref _montoPagado, value);
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

        private ObservableCollection<OrdenData> _ordenes;
        public ObservableCollection<OrdenData> Ordenes
        {
            get => _ordenes;
            set => this.RaiseAndSetIfChanged(ref _ordenes, value);
        }

        private OrdenData _ordenSeleccionada;
        public OrdenData OrdenSeleccionada
        {
            get => _ordenSeleccionada;
            set
            {
                if (value is not null)
                {
                    Adeudo = value.Total;
                    this.RaiseAndSetIfChanged(ref _ordenSeleccionada, value);
                }
            }
        }

        private readonly ApplicationDbContext _context;
        public FormCajaViewModel(ApplicationDbContext context)
        {
            _context = context;

            BuildNumberCommand = ReactiveCommand.Create<string>(BuildNumber);
            ClearErrorCommand = ReactiveCommand.Create(ClearError);
            PagarCommand = ReactiveCommand.Create(Pagar);
            StoreCommand = ReactiveCommand.Create(Store);

            var data = new BuscarOrdenesPendientesAction(context).Execute();
            Ordenes = new ObservableCollection<OrdenData>(data);
        }


        private void CalcularCambio()
        {
            _ = double.TryParse(MontoPagado, out double monto);
            Cambio = Math.Round(monto - Adeudo, 2);
        }

        public ICommand StoreCommand { get; }
        private void Store()
        {
            try
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
                Ordenes = new ObservableCollection<OrdenData>(data);
                //OrdenSeleccionada = null;
            }
            catch (Exception e)
            {
                DialogHelper.CreateAlertDialog(new AlertDialogBuilderParams()
                {
                    DialogHeaderIcon = Material.Dialog.Icons.DialogIconKind.Error,
                    SupportingText = "Error al intentar pagar la orden!",
                    ContentHeader = "Error al pagar orden!",
                }).Show();
            }
        }

    }
}
