using CorePuntoVenta;
using CorePuntoVenta.Domain.Cajas.Data;
using CorePuntoVenta.Domain.Cajas.Mappers;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace PuntoVentaViews.ViewModels
{
    public class HistorialCajaViewModel : ViewModelBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ItemCajaMapper _mapper;
        private ObservableCollection<ItemCajaData> _historial = [];

        public ObservableCollection<ItemCajaData> Historial { 
            get => _historial;
            set => this.RaiseAndSetIfChanged(ref _historial, value);
        }

        public HistorialCajaViewModel(ApplicationDbContext context)
        {
            _context = context;
            _mapper = new();

            var startDate = DateTime.UtcNow;

            var data = _context.ItemsCaja.Where(i => i.CajaId == 1).Select(i => _mapper.ToDto(i)).ToList() ?? [];
            Historial = new ObservableCollection<ItemCajaData>(data);
        }
    }
}
