using CorePuntoVenta;
using CorePuntoVenta.Domain.Productos.Actions;
using CorePuntoVenta.Domain.Productos.Data;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PuntoVentaViews.ViewModels
{
    public class CategoriaFormViewModel : ViewModelBase
    {
        private string _nombre;
        public string Nombre
        {
            get => _nombre;
            set => this.RaiseAndSetIfChanged(ref _nombre, value);
        }

        public ICommand SaveCommand { get; }
        private void Save()
        {
            CategoriaData categoriaData = new()
            {
                Nombre = Nombre,
            };

            var categoria = new StoreCategoriaAction(_context).Execute(categoriaData);

        }

        private readonly ApplicationDbContext _context;
        public CategoriaFormViewModel(ApplicationDbContext context)
        {
            _context = context;
            SaveCommand = ReactiveCommand.Create(Save);
        }
    }
}
