using CommunityToolkit.Mvvm.ComponentModel;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuntoVentaViews.Models
{
    public class ItemOrderUI : ReactiveObject
    {

        private string _kilos;
        public string Kilos
        {
            get => _kilos;
            set {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Total = CalcularTotal(PrecioUnitario, value);
                }
                this.RaiseAndSetIfChanged(ref _kilos, value); 
            }
        }

        private string _precioUnitario;
        public string PrecioUnitario
        {
            get => _precioUnitario;
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    Total = CalcularTotal(value, Kilos);
                }
                this.RaiseAndSetIfChanged(ref _precioUnitario, value);
            }
        }

        private string _producto;
        public string Producto
        {
            get => _producto;
            set => this.RaiseAndSetIfChanged(ref _producto, value);
        }

        private int _productoId;
        public int ProductoId
        {
            get => _productoId;
            set => this.RaiseAndSetIfChanged(ref _productoId, value);
        }

        private double _total;
        public double Total
        {
            get => _total;
            set => this.RaiseAndSetIfChanged(ref _total, value);
        }

        private double CalcularTotal(string precio, string kilos)
        {
            _ = double.TryParse(kilos, out double k);
            _ = double.TryParse(precio, out double p);
            return Math.Round(k * p, 2);
        }
    }
}
