using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Printing;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace PuntoVentaViews.ViewModels
{
    public class ConfiguracionViewModel : ViewModelBase
    {
        private List<string> _impresoras = new();
        private string _impresoraSeleccionada;

        public List<string> Impresoras
        {
            get => _impresoras;
            set => this.RaiseAndSetIfChanged(ref _impresoras, value);
        }

        public string ImpresoraSeleccionada
        {
            get => _impresoraSeleccionada;
            set => this.RaiseAndSetIfChanged(ref _impresoraSeleccionada, value);
        }

        [SupportedOSPlatform("windows")]
        public ConfiguracionViewModel()
        {
            foreach (string impresora in PrinterSettings.InstalledPrinters)
            {
                Impresoras.Add(impresora);
                Debug.WriteLine(impresora);
            }
        }
    }
}
