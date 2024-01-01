using CorePuntoVenta;
using CorePuntoVenta.Domain.Productos.Actions;
using CorePuntoVenta.Domain.Productos.Data;
using Material.Dialog;
using ReactiveUI;
using System;
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
            try
            {
                CategoriaData categoriaData = new()
                {
                    Nombre = Nombre,
                };

                var categoria = new StoreCategoriaAction(_context).Execute(categoriaData);
                DialogHelper.CreateAlertDialog(new AlertDialogBuilderParams
                {
                    Borderless = true,
                    DialogHeaderIcon = Material.Dialog.Icons.DialogIconKind.Success,
                    ContentHeader = "Categoria registrada correctamente!",
                    SupportingText = "La categoria se registro correctamente!"
                }).Show();
                Nombre = "";
            }
            catch (Exception)
            {
                DialogHelper.CreateAlertDialog(new AlertDialogBuilderParams
                {
                    Borderless = true,
                    DialogHeaderIcon = Material.Dialog.Icons.DialogIconKind.Success,
                    ContentHeader = "Error al intentar registrar categoria!",
                    SupportingText = "Se produjo un error al intentar crear la categoria!"
                }).Show();
            }
        }

        private readonly ApplicationDbContext _context;
        public CategoriaFormViewModel(ApplicationDbContext context)
        {
            _context = context;
            SaveCommand = ReactiveCommand.Create(Save);
        }
    }
}
