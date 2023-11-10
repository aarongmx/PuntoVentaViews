using Avalonia.Rendering.Composition;
using CorePuntoVenta;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PuntoVentaViews.ViewModels
{
    public class CorteCajaFormViewModel : ViewModelBase
    {
        private double _totalIngresosCaja;
        private double _totalEgresosCaja;
        private double _montoInicialCaja;
        private double _totalVentaEfectivo;

        public double TotalIngresosCaja
        {
            get => _totalIngresosCaja;
            set => this.RaiseAndSetIfChanged(ref _totalIngresosCaja, value);
        }

        public double TotalEgresosCaja
        {
            get => _totalEgresosCaja;
            set => this.RaiseAndSetIfChanged(ref _totalEgresosCaja, value);
        }

        public double MontoInicialCaja
        {
            get => _montoInicialCaja;
            set => this.RaiseAndSetIfChanged(ref _montoInicialCaja, value);
        }

        public double TotalVentaEfectivo
        {
            get => _totalVentaEfectivo;
            set => this.RaiseAndSetIfChanged(ref _totalVentaEfectivo, value);
        }

        private readonly ApplicationDbContext _context;
        public CorteCajaFormViewModel(ApplicationDbContext context)
        {
            _context = context;
            CrearCorteCommand = ReactiveCommand.Create(CrearCorte);

            TotalVentaEfectivo = _context.Ordenes.Where(o => o.CajaId == 1).Sum(o => o.Total);
            MontoInicialCaja = _context.Cajas.Where(c => c.Id == 1).Select(c => c.EfectivoDisponible).FirstOrDefault();
            TotalIngresosCaja = _context.ItemsCaja.Where(i => i.CajaId == 1).Where(i => i.Monto > 0).Sum(i => i.Monto);
            TotalEgresosCaja = _context.ItemsCaja.Where(i => i.CajaId == 1).Where(i => i.Monto < 0).Sum(i => i.Monto);
        }

        public ICommand CrearCorteCommand { get; }
        private void CrearCorte()
        {
            var totalOrden = _context.Ordenes.Where(o => o.CajaId == 1).Sum(o => o.Total);
            var totalIngresosEgresos = _context.ItemsCaja.Where(i => i.CajaId == 1).Sum(i => i.Monto);
            var totalEgresos = _context.ItemsCaja.Where(i => i.CajaId == 1).Where(i => i.Monto < 0).Sum(i => i.Monto);
            var totalIngresos = _context.ItemsCaja.Where(i => i.CajaId == 1).Where(i => i.Monto > 0).Sum(i => i.Monto);

            Debug.WriteLine(totalOrden);
            Debug.WriteLine(totalIngresos);
            Debug.WriteLine(totalEgresos);
            Debug.WriteLine(totalIngresosEgresos);
        }
    }
}
