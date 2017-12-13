using System;
using System.Collections.Generic;
using System.Text;

namespace Cheop.Models
{
    public class Road<T>
    {
        //public T oras1 { get; }
        //public T oras2 { get; }
        public KeyValuePair<GraphNode<T>, GraphNode<T>> road;

        public Road()
        {

        }

        public Road(GraphNode<T> o1, GraphNode<T> o2)
        {
            road = new KeyValuePair<GraphNode<T>, GraphNode<T>>(o1, o2);
        }

        public override string ToString() => new StringBuilder(this.road.Key.NodeNumber.ToString() + " -> " + this.road.Value.NodeNumber.ToString()).ToString();


    }
}
