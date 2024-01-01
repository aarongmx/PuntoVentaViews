using ReactiveUI;
using System.Collections.Generic;
using System.Drawing.Printing;
using System.Runtime.Versioning;

namespace PuntoVentaViews.ViewModels
{
    public class ConfiguracionViewModel : ViewModelBase
    {
        private List<string> _impresoras = [];
        private string? _impresoraSeleccionada;

        public List<string> Impresoras
        {
            get => _impresoras;
            set => this.RaiseAndSetIfChanged(ref _impresoras, value);
        }

        public string ImpresoraSeleccionada
        {
            get => _impresoraSeleccionada ?? string.Empty;
            set => this.RaiseAndSetIfChanged(ref _impresoraSeleccionada, value);
        }

        [SupportedOSPlatform("windows")]
        public ConfiguracionViewModel()
        {
            foreach (string impresora in PrinterSettings.InstalledPrinters)
            {
                Impresoras.Add(impresora);
            }
        }
    }
}
