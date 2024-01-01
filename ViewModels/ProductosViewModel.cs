using CorePuntoVenta;
using CorePuntoVenta.Domain.Productos.Actions;
using CorePuntoVenta.Domain.Productos.Data;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive;

namespace PuntoVentaViews.ViewModels
{
    public class ProductosViewModel : ViewModelBase
    {

        public ReactiveCommand<object, Unit> AddProductoCommand { get; }

        private IList<ProductoData> _productos;
        public IList<ProductoData> Productos {
            get => _productos;
            set => this.RaiseAndSetIfChanged(ref _productos, value);
        }

        public ProductosViewModel(ApplicationDbContext context) {
            var data = new ListProductosAction(context).Execute();
            Productos = data;
            AddProductoCommand = ReactiveCommand.Create<object>(AddProduct);

            data.ForEach(x => Console.WriteLine(x.Nombre));
        }

        private void AddProduct(object data)
        {
            Debug.WriteLine($"Editando: {data}");
        }
    }
}
