using Material.Icons;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuntoVentaViews.Components
{
    public class Node
    {
        public ObservableCollection<Node>? SubNodes { get; }
        public string Title { get; }
        public string? To { get; }
        public MaterialIconKind? Icon { get; }

        public Node(string title)
        {
            Title = title;
        }

        public Node(string title, string to)
        {
            Title = title;
            To = to;
        }

        public Node(string title, string to, MaterialIconKind icon)
        {
            Title = title;
            To = to;
            Icon = icon;
        }

        public Node(string title, ObservableCollection<Node> subNodes)
        {
            Title = title;
            SubNodes = subNodes;
        }
    }
}
