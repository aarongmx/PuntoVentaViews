using CorePuntoVenta;
using CorePuntoVenta.Domain.Helpers;
using PuntoVentaViews.Components;
using PuntoVentaViews.Models;
using ReactiveUI;
using System.Collections.ObjectModel;

namespace PuntoVentaViews.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<Node> Nodes { get; }

        private object? _menuSeleccionado = null;
        public object MenuSeleccionado
        {
            get => _menuSeleccionado!;
            set
            {
                string? to = ((Node)value).To;
                if (!string.IsNullOrWhiteSpace(to))
                {
                    CurrentPage = _router.Routed(to);
                    this.RaiseAndSetIfChanged(ref _menuSeleccionado, value);
                }
            }
        }
        private readonly Session? _session;
        private readonly Router _router;
        private readonly ApplicationDbContext _context;

        public Session? Session
        {
            get => _session;
        }

        public MainWindowViewModel(ApplicationDbContext context)
        {
            _context = context;
            _router = new Router(_context);
            _session = Session.GetInstance(new CorePuntoVenta.Domain.Administracion.Data.UsuarioData());

            if (_session.Value.RolId == 1)
            {
                Nodes = _router.Menu();
                CurrentPage = _router.Routed(Routes.DASHBOARD);
            }

            if (_session.Value.RolId == 2)
            {
                Nodes = _router.MenuCaja();
                CurrentPage = _router.Routed(Routes.FORM_CAJA);
            }

            if (_session.Value.RolId == 3)
            {
                Nodes = _router.MenuOrden();
                CurrentPage = _router.Routed(Routes.ORDENES);
            }
        }

        private ViewModelBase? _CurrentPage;
        public ViewModelBase CurrentPage
        {
            get => _CurrentPage!;
            private set
            {
                this.RaiseAndSetIfChanged(ref _CurrentPage, value);
            }
        }
    }
}