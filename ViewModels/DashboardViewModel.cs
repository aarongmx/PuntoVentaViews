using CorePuntoVenta;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using LiveChartsCore.SkiaSharpView.VisualElements;
using SkiaSharp;

namespace PuntoVentaViews.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public ISeries[] Series { get; set; } = {
            new LineSeries<double>
            {
                Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
                Fill = null
            }
        };

        public Axis[] YAxes { get; set; } = new Axis[]
        {
            new Axis()
            {
                Name = "Ventas"
            }
        };


        private readonly ApplicationDbContext _context;
        public DashboardViewModel(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
