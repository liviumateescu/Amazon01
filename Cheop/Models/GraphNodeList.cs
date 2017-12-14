using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Cheop.Models
{
    //[Serializable]
    public class GraphNodeList<T> : Collection<GraphNode<T>>
    {
        public GraphNodeList() : base() { }

        public GraphNodeList(int intialSize)
        {
            //Add the specified number of items
            for (int i = 0; i < intialSize; i++)
            {
                base.Items.Add(default(GraphNode<T>));
            }
        }

        public Node<T> FindByValue(T value)
        {
            //search the list for the value
            foreach (GraphNode<T> node in Items)
                if (node.Value.Equals(value))
                    return node;
            //if we reached here, we didn't find a matching node
            return null;
        }
    }
}

