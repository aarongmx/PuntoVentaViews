using CorePuntoVenta;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace PuntoVentaViews.ViewModels
{
    public class DashboardViewModel(ApplicationDbContext context) : ViewModelBase
    {
        public ISeries[] Series { get; set; } = {
            new LineSeries<double>
            {
                Values = new double[] { 2, 1, 3, 5, 3, 4, 6 },
                Fill = null
            }
        };

        public Axis[] YAxes { get; set; } =
        [
            new Axis()
            {
                Name = "Ventas"
            }
        ];


        private readonly ApplicationDbContext _context = context;
    }
}
