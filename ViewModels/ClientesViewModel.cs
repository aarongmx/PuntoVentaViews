﻿using Avalonia.Controls;
using CorePuntoVenta.Domain.Clientes.Actions;
using CorePuntoVenta.Domain.Clientes.Data;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace PuntoVentaViews.ViewModels
{
    public class ClientesViewModel : ViewModelBase
    {
        public ObservableCollection<ClienteData> _clientes = new();

        public ICommand RefreshClientesCommand { get; }

        public ClientesViewModel()
        {
            var data = (new ListClientesAction(new CorePuntoVenta.ApplicationDbContext())).Execute().OrderBy(cliente => cliente.Id);
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
            List<ClienteData> data = (new ListClientesAction(new CorePuntoVenta.ApplicationDbContext())).Execute().OrderBy(cliente => cliente.Id).ToList();
            data.ForEach(Clientes.Add);
        }

        public void Updated()
        {

        }

    }
}