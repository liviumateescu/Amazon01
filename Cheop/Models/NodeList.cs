﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;

namespace Cheop.Models
{
    //[Serializable]
    public class NodeList<T>:Collection<Node<T>>
    {
        public NodeList() : base() { }

        public NodeList(int intialSize)
        {
            //Add the specified number of items
            for (int i = 0; i < intialSize; i++)
            {
                base.Items.Add(default(Node<T>));
            }
        }

        public Node<T> FindByValue(T value)
        {
            //search the list for the value
            foreach (Node<T> node in Items)
                if (node.Value.Equals(value))
                    return node;
            //if we reached here, we didn't find a matching node
            return null;
        }

        public override string ToString()
        {
            string s="NodeList: ";
            foreach (Node<T> nod in base.Items)
            {
                s = s + nod.ToString() + " -> ";
            }
            return s+"\n";
        }

    }
}
