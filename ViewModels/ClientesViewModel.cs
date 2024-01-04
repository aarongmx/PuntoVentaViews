using CorePuntoVenta;
using CorePuntoVenta.Domain.Clientes.Actions;
using CorePuntoVenta.Domain.Clientes.Data;
using ReactiveUI;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;


namespace PuntoVentaViews.ViewModels
{
    public class ClientesViewModel : ViewModelBase
    {
        public ApplicationDbContext _context;
        public ObservableCollection<ClienteData> _clientes = [];

        public ICommand RefreshClientesCommand { get; }

        public ClientesViewModel(ApplicationDbContext context)
        {
            _context = context;
            var data = (new ListClientesAction(_context)).Execute().OrderBy(cliente => cliente.Id);
            Clientes = new ObservableCollection<ClienteData>(data);
            RefreshClientesCommand = ReactiveCommand.Create(RefreshClientes);
        }

        public ObservableCollection<ClienteData> Clientes
        {
            get => _clientes;
            set => this.RaiseAndSetIfChanged(ref _clientes, value);
        }

        public void RefreshClientes()
        {
            Clientes.Clear();
            List<ClienteData> data = [.. (new ListClientesAction(new CorePuntoVenta.ApplicationDbContext())).Execute().OrderBy(cliente => cliente.Id)];
            data.ForEach(Clientes.Add);
        }

        public void Updated()
        {

        }

    }
}
