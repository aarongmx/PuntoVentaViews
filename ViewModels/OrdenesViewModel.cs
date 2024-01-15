﻿using CorePuntoVenta;
using CorePuntoVenta.Domain.Clientes.Actions;
using CorePuntoVenta.Domain.Clientes.Data;
using CorePuntoVenta.Domain.Helpers;
using CorePuntoVenta.Domain.Ordenes.Actions;
using CorePuntoVenta.Domain.Ordenes.Data;
using CorePuntoVenta.Domain.Ordenes.Models;
using CorePuntoVenta.Domain.Productos.Actions;
using CorePuntoVenta.Domain.Productos.Data;
using Material.Dialog;
using PuntoVentaViews.Models;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace PuntoVentaViews.ViewModels
{
    public class OrdenesViewModel : ViewModelBase
    {
        private ObservableCollection<ProductoData> _productos = [];

        private ObservableCollection<ClienteData> _clientes = [];

        private ObservableCollection<ItemOrderUI> _itemsOrden = [];

        private ObservableCollection<Orden> _ordenes = [];

        public ICommand AgregarProductoCommand { get; }

        public ICommand GenerarOrdenCommand { get; }

        public ICommand AddOrderCommand { get; }

        public ICommand RegistrarOrdenCommand { get; }

        private string _precioUnitario;

        private string _kilos;

        private ProductoData _productoSeleccionado;

        private ClienteData _clienteSeleccionado;

        private double _total;

        private double _kilosTotal;

        private readonly ApplicationDbContext _context;

        private OrdenData _ordenSeleccionada;

        public OrdenData OrdenSeleccionada
        {
            get => _ordenSeleccionada;
            set => this.RaiseAndSetIfChanged(ref _ordenSeleccionada, value);
        }

        public ObservableCollection<Orden> Ordenes
        {
            get => _ordenes;
            set => this.RaiseAndSetIfChanged(ref _ordenes, value);
        }

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
            if (OrdenSeleccionada.ItemsOrden.Any(item => item.ProductoId == productoData.Id))
            {
                var itemSelected = OrdenSeleccionada.ItemsOrden.First(item => item.ProductoId == productoData.Id);
                itemSelected.Kilos += 1;
            }

            if (!OrdenSeleccionada.ItemsOrden.Any(item => item.ProductoId == productoData.Id))
            {
                ItemOrdenData item = new()
                {
                    Kilos = 1,
                    ProductoId = (int)productoData.Id,
                    Producto = productoData,
                    Total = 1 * productoData.PrecioUnitario,
                    PrecioUnitario = productoData.PrecioUnitario,
                };
                OrdenSeleccionada?.ItemsOrden?.Add(item);
                OrdenSeleccionada.Total = (new CalcularTotalOrdenAction()).Execute(OrdenSeleccionada.ItemsOrden);
            }
        }

        private void AddOrder()
        {
            //_ordenes.Add(new OrdenData() { Id = 2 });
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

                new StoreOrdenAction(_context).Execute(ordenData);

                DialogHelper.CreateAlertDialog(new AlertDialogBuilderParams()
                {
                    ContentHeader = "Orden creada!",
                    DialogHeaderIcon = Material.Dialog.Icons.DialogIconKind.Success,
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

        private void RegistrarOrden(OrdenData ordenData)
        {
            ordenData.ItemsOrden?.ToList().ForEach(item =>
            {
                Debug.WriteLine(item.ProductoId);
                Debug.WriteLine(item.Kilos);
                Debug.WriteLine(item.Total);
                Debug.WriteLine(item.PrecioUnitario);
            });
            Debug.WriteLine(ordenData.Kilos);
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
            var data = new ListarAllOrdenesAction(context).Execute();
            _ordenes = new ObservableCollection<Orden>(data);

            var productos = new ListProductosAction(context).Execute();
            Productos = new ObservableCollection<ProductoData>(productos);

            var clientes = new ListClientesAction(_context).Execute();
            Clientes = new ObservableCollection<ClienteData>(clientes);

            AgregarProductoCommand = ReactiveCommand.Create<ProductoData>(AgregarProducto);
            GenerarOrdenCommand = ReactiveCommand.Create(GenerarOrden);

            AddOrderCommand = ReactiveCommand.Create(AddOrder);

            RegistrarOrdenCommand = ReactiveCommand.Create<OrdenData>(RegistrarOrden);

            ItemsOrden = [];
        }
    }
}
