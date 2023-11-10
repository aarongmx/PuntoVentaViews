using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;
using CorePuntoVenta;
using CorePuntoVenta.Domain.Clientes.Actions;
using CorePuntoVenta.Domain.Clientes.Data;
using CorePuntoVenta.Domain.Helpers;
using CorePuntoVenta.Domain.Ordenes.Actions;
using CorePuntoVenta.Domain.Ordenes.Data;
using CorePuntoVenta.Domain.Productos.Actions;
using CorePuntoVenta.Domain.Productos.Data;
using DynamicData.Binding;
using Material.Dialog;
using NodaTime;
using PuntoVentaViews.Models;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PuntoVentaViews.ViewModels
{
    public class OrdenesViewModel : ViewModelBase
    {
        private ObservableCollection<ProductoData> _productos = new();

        private ObservableCollection<ClienteData> _clientes = new();

        private ObservableCollection<ItemOrderUI> _itemsOrden = new();

        public ICommand AgregarProductoCommand { get; }

        public ICommand GenerarOrdenCommand { get; }

        private string _precioUnitario;

        private string _kilos;

        private ProductoData _productoSeleccionado;

        private ClienteData _clienteSeleccionado;

        private double _total;

        private double _kilosTotal;

        private readonly ApplicationDbContext _context;


        public ObservableCollection<ProductoData> Productos
        {
            get => _productos;
            set => this.RaiseAndSetIfChanged(ref _productos, value);
        }

        public ObservableCollection<ClienteData> Clientes
        {
            get => _clientes;
            set => this.RaiseAndSetIfChanged(ref _clientes, value);
        }

        public ObservableCollection<ItemOrderUI> ItemsOrden
        {
            get => _itemsOrden;
            set => this.RaiseAndSetIfChanged(ref _itemsOrden, value);
        }

        private void AgregarProducto(ProductoData productoData)
        {
            ItemOrderUI itemOrderUI = new()
            {
                Kilos = "1",
                PrecioUnitario = "" + productoData.PrecioUnitario,
                Producto = productoData.Nombre,
                ProductoId = (int)productoData.Id!,
            };

            ItemsOrden.Add(itemOrderUI);
            CalcularTotales();

            /*ProductoSeleccionado = null!;
            PrecioUnitario = "";
            Kilos = "";*/
        }

        private void GenerarOrden()
        {
            try
            {
                var items = ItemsOrden.Select(i =>
                {
                    _ = double.TryParse(i.PrecioUnitario, out double precio);
                    _ = double.TryParse(i.Kilos, out double kilos);
                    return new ItemOrdenData()
                    {
                        ProductoId = i.ProductoId,
                        Kilos = Math.Round(kilos, 2),
                        PrecioUnitario = Math.Round(precio, 2),
                        Total = Math.Round(precio * kilos, 2),
                    };
                }).ToList();

                OrdenData ordenData = new()
                {
                    EmpleadoId = 1,
                    Fecha = DateTime.UtcNow,
                    Kilos = KilosTotal,
                    Total = Total,
                    ItemsOrden = items,
                    ClienteId = (int)ClienteSeleccionado.Id!,
                    EstatusOrdenId = 1,
                };

                new StoreOrdenAction().Execute(ordenData);

                DialogHelper.CreateAlertDialog(new AlertDialogBuilderParams()
                {
                    ContentHeader = "Orden creada!",
                    DialogHeaderIcon = Material.Dialog.Icons.DialogIconKind.Success,
                    Width = 364,
                    SupportingText = "Orden creada correctamente!"
                }).Show();
            }
            catch (Exception e)
            {
                DialogHelper.CreateAlertDialog(new AlertDialogBuilderParams()
                {
                    ContentHeader = "Error al crear orden!",
                    DialogHeaderIcon = Material.Dialog.Icons.DialogIconKind.Error,
                    SupportingText = "Orden creada correctamente!"
                }).Show();
                Console.WriteLine(e.StackTrace);
            }
        }


        private void CalcularTotales()
        {
            Total = ItemsOrden.Sum(x =>
            {
                _ = double.TryParse(x.PrecioUnitario, out double precio);
                _ = double.TryParse(x.Kilos, out double kilos);

                return precio * kilos;
            });

            KilosTotal = ItemsOrden.Sum(i =>
            {
                _ = double.TryParse(i.Kilos, out double kilos);
                return kilos;
            });
        }

        public string PrecioUnitario
        {
            get => _precioUnitario;
            set => this.RaiseAndSetIfChanged(ref _precioUnitario, value);
        }


        public string Kilos
        {
            get => _kilos;
            set => this.RaiseAndSetIfChanged(ref _kilos, value);
        }


        public ProductoData ProductoSeleccionado
        {
            get => _productoSeleccionado;
            set
            {
                if (value is not null)
                {
                    PrecioUnitario = $"{value.PrecioUnitario}";
                    Kilos = "1";
                }

                this.RaiseAndSetIfChanged(ref _productoSeleccionado!, value);
            }
        }


        public ClienteData ClienteSeleccionado
        {
            get => _clienteSeleccionado;
            set => this.RaiseAndSetIfChanged(ref _clienteSeleccionado, value);
        }


        public double Total
        {
            get => _total;
            set => this.RaiseAndSetIfChanged(ref _total, value);
        }


        public double KilosTotal
        {
            get => _kilosTotal;
            set => this.RaiseAndSetIfChanged(ref _kilosTotal, value);
        }


        public OrdenesViewModel(ApplicationDbContext context)
        {
            _context = context;
            Session session = Session.GetInstance(new CorePuntoVenta.Domain.Administracion.Data.UsuarioData());

            Debug.WriteLine(session.Value.Id);

            //Image = LoadFromResource(new Uri(@"C:\punto_venta\images\huevo.jpg"));

            var productos = new ListProductosAction(context).Execute();
            Productos = new ObservableCollection<ProductoData>(productos);

            var clientes = new ListClientesAction(_context).Execute();
            Clientes = new ObservableCollection<ClienteData>(clientes);

            AgregarProductoCommand = ReactiveCommand.Create<ProductoData>(AgregarProducto);
            GenerarOrdenCommand = ReactiveCommand.Create(GenerarOrden);

            ItemsOrden = new();
        }
    }
}
