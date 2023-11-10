using CorePuntoVenta;
using CorePuntoVenta.Domain.Productos.Actions;
using CorePuntoVenta.Domain.Productos.Data;
using CorePuntoVenta.Domain.Productos.Models;
using iText.Layout.Renderer;
using Material.Dialog;
using Material.Dialog.Icons;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PuntoVentaViews.ViewModels
{
    public class FormProductoViewModel : ViewModelBase
    {
        private string _nombre = string.Empty;
        private string _precioUnitario;
        public ICommand SaveCommand { get; }
        private ObservableCollection<CategoriaData> _categorias = new();
        private CategoriaData _categoriaSeleccionada;
        private readonly ApplicationDbContext _context;

        [Required]
        [Display(Name = "nombre")]
        public string Nombre
        {
            get => _nombre;
            set => this.RaiseAndSetIfChanged(ref _nombre, value);
        }

        [Required]
        [Display(Name = "precio unitario")]
        public string PrecioUnitario
        {
            get => _precioUnitario;
            set => this.RaiseAndSetIfChanged(ref _precioUnitario, value);
        }

        private void Save()
        {
            try
            {
                _ = double.TryParse(PrecioUnitario, out double precioUnitario);
                ProductoData productoData = new() { Nombre = Nombre.Trim().ToUpper(), CategoriaId = CategoriaSeleccionada?.Id ?? 0, PrecioUnitario = Math.Round(precioUnitario, 2) };
                Producto producto = (new StoreProductoAction(new CorePuntoVenta.ApplicationDbContext())).Execute(productoData);

                Nombre = "";
                PrecioUnitario = "";
                CategoriaSeleccionada = null;

                DialogHelper.CreateAlertDialog(new AlertDialogBuilderParams()
                {
                    DialogHeaderIcon = DialogIconKind.Success,
                    ContentHeader = "Producto guardado!",
                    SupportingText = "Se guardo correctamente el producto!"
                }).Show();
            }
            catch (Exception ex)
            {
                DialogHelper.CreateAlertDialog(new AlertDialogBuilderParams()
                {
                    DialogHeaderIcon = DialogIconKind.Success,
                    ContentHeader = "Erorr al guardar producto!",
                    SupportingText = "Se guardo correctamente el producto!"
                }).Show();
                Debug.WriteLine(ex.Message);
            }
        }

        public ObservableCollection<CategoriaData> Categorias
        {
            get => _categorias;
            set => this.RaiseAndSetIfChanged(ref _categorias, value);
        }

        [Required]
        public CategoriaData? CategoriaSeleccionada
        {
            get => _categoriaSeleccionada!;
            set => this.RaiseAndSetIfChanged(ref _categoriaSeleccionada, value);
        }

        public FormProductoViewModel(ApplicationDbContext context)
        {
            _context = context;
            SaveCommand = ReactiveCommand.Create(Save);

            var categorias = new ListCategoriasAction(_context).Execute();
            Categorias = new ObservableCollection<CategoriaData>(categorias);
        }
    }
}
