﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CorePuntoVenta;
using CorePuntoVenta.Domain.Productos.Actions;
using PuntoVentaViews.Components;
using PuntoVentaViews.ViewModels;

namespace PuntoVentaViews.Models
{
    public class Router
    {
        private readonly ApplicationDbContext _context;

        public Router(ApplicationDbContext context)
        {
            _context = context;
        }

        public ViewModelBase Routed(string route)
        {
            return route switch
            {
                Routes.LOGIN => new LoginViewModel(_context),
                Routes.CLIENTES => new ClientesViewModel(),
                Routes.NUEVO_CLIENTE => new FormClienteViewModel(),
                Routes.VENTAS => new VentasViewModel(_context),
                Routes.PRODUCTOS => new ProductosViewModel(_context),
                Routes.FORM_PRODUCTOS => new FormProductoViewModel(_context),
                Routes.FORM_CAJA => new FormCajaViewModel(_context),
                Routes.INGRESAR_EFECTIVO => new FormIngresoEfectivoViewModel(),
                Routes.RETIRAR_EFECTIVO => new FormEgresoEfectivoViewModel(),
                Routes.ORDENES => new OrdenesViewModel(_context),
                Routes.CATEGORIA_FORM => new CategoriaFormViewModel(_context),
                Routes.HISTORIAL_CAJA => new HistorialCajaViewModel(_context),
                Routes.CORTE_CAJA => new CorteCajaFormViewModel(_context),
                Routes.DASHBOARD => new DashboardViewModel(_context),
                Routes.CONFIGURACION => new ConfiguracionViewModel(),
                _ => new LoginViewModel(_context),
            };
        }

        public ObservableCollection<Node> Menu() => new(){
            new Node("Home", Routes.LOGIN, Material.Icons.MaterialIconKind.SecurityAccount),
            new Node("Dashboard", Routes.DASHBOARD, Material.Icons.MaterialIconKind.ChartBar),
            new Node("Clientes", new ObservableCollection<Node>
            {
                new Node("Todos", Routes.CLIENTES, Material.Icons.MaterialIconKind.ViewAgenda),
                new Node("Nuevo", Routes.NUEVO_CLIENTE, Material.Icons.MaterialIconKind.Plus),
            }),
            new Node("Ventas", new ObservableCollection<Node>
            {
                new Node("Todas", Routes.VENTAS, Material.Icons.MaterialIconKind.ChartBar),
                new Node("Nueva"),
            }),
                new Node("Productos", new ObservableCollection<Node>
            {
                new Node("Todos", Routes.PRODUCTOS, Material.Icons.MaterialIconKind.ListBox),
                new Node("Nuevo", Routes.FORM_PRODUCTOS, Material.Icons.MaterialIconKind.Plus),
                new Node("Nueva categoria", Routes.CATEGORIA_FORM, Material.Icons.MaterialIconKind.Plus),
            }),
                new Node("Caja", new ObservableCollection<Node>
                {
                    new Node("Cobrar", Routes.FORM_CAJA, Material.Icons.MaterialIconKind.CashRegister),
                    new Node("Historial", Routes.HISTORIAL_CAJA, Material.Icons.MaterialIconKind.ClockCheck),
                    new Node("Ingresar Efectivo", Routes.INGRESAR_EFECTIVO, Material.Icons.MaterialIconKind.CashPlus),
                    new Node("Salida Efectivo", Routes.RETIRAR_EFECTIVO, Material.Icons.MaterialIconKind.CashRemove),
                    new Node("Corte", Routes.CORTE_CAJA, Material.Icons.MaterialIconKind.Cash100),
                    new Node("Configuración", Routes.CONFIGURACION, Material.Icons.MaterialIconKind.Cog),
                }),
            new Node("Ordenes", new ObservableCollection<Node>
            {
                new Node("Todas", Routes.ORDENES, Material.Icons.MaterialIconKind.ListBox),
                new Node("Nueva"),
            }),
            new Node("Reportes", new ObservableCollection<Node>
            {
                new Node("Caja"),
                new Node("Ventas"),
                new Node("Ventas"),
            })
        };

        public ObservableCollection<Node> MenuCaja() => new(){
            new Node("Cobrar", Routes.FORM_CAJA, Material.Icons.MaterialIconKind.CashRegister),
            new Node("Historial", Routes.HISTORIAL_CAJA, Material.Icons.MaterialIconKind.ClockCheck),
            new Node("Ingresar Efectivo", Routes.INGRESAR_EFECTIVO, Material.Icons.MaterialIconKind.CashPlus),
            new Node("Salida Efectivo", Routes.RETIRAR_EFECTIVO, Material.Icons.MaterialIconKind.CashRemove),
            new Node("Corte", Routes.CORTE_CAJA, Material.Icons.MaterialIconKind.Cash100),
            new Node("Configuración", Routes.CONFIGURACION, Material.Icons.MaterialIconKind.Cog),
        };

        public ObservableCollection<Node> MenuOrden() => new(){
            new Node("Orden", Routes.ORDENES, Material.Icons.MaterialIconKind.ListBox),
        };
    }
}