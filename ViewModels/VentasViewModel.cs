using CorePuntoVenta;
using CorePuntoVenta.Domain.Ventas.Actions;
using Material.Dialog;
using ReactiveUI;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;

namespace PuntoVentaViews.ViewModels
{
    public class VentasViewModel : ViewModelBase
    {
        private readonly ApplicationDbContext _context;
        public ICommand DescargarReporteCommand { get; }
        private DateTimeOffset? _fechaInicio = null;
        private DateTimeOffset? _fechaFin = null;


        private void DescargarReporte()
        {
            try
            {
                (new GenerarReporteVentasAction(_context)).Execute();
                DialogHelper.CreateAlertDialog(new AlertDialogBuilderParams()
                {
                    Borderless = true,
                    DialogHeaderIcon = Material.Dialog.Icons.DialogIconKind.Success,
                    ContentHeader = "Se creo el reporte correctamente!",
                    SupportingText = "Reporte creado"
                }).Show();
            }catch (Exception)
            {
                DialogHelper.CreateAlertDialog(new AlertDialogBuilderParams()
                {
                    Borderless = true,
                    DialogHeaderIcon = Material.Dialog.Icons.DialogIconKind.Error,
                    ContentHeader = "Error al crear reporte!",
                    SupportingText = "Error"
                }).Show();
            }
        }

        public DateTimeOffset? FechaInicio
        {
            get => _fechaInicio;
            set
            {
                if (value is not null)
                {
                    Debug.WriteLine(value);
                    this.RaiseAndSetIfChanged(ref _fechaInicio, value);
                }
            }
        }

        public DateTimeOffset? FechaFin
        {
            get => _fechaFin;
            set
            {
                if (value is not null)
                {
                    Debug.WriteLine(value);
                    this.RaiseAndSetIfChanged(ref _fechaFin, value);
                }
            }
        }

        public VentasViewModel(ApplicationDbContext context)
        {
            _context = context;
            DescargarReporteCommand = ReactiveCommand.Create(DescargarReporte);
        }
    }
}
