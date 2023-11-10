using CorePuntoVenta.Domain.Helpers;
using ReactiveUI;

namespace PuntoVentaViews.ViewModels
{
    public class ViewModelBase : ReactiveObject
    {
        private bool _isLoggedIn = false;
        public bool IsLoggedIn
        {
            get => _isLoggedIn;
            set => this.RaiseAndSetIfChanged(ref _isLoggedIn, value);
        }
    }
}