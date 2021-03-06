﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Cheop.Models
{
    //[Serializable]
    public class Node<T>
    {
        private T data;
        private NodeList<T> neighbors = null;

        public int NodeNumber => Int32.Parse(data.ToString());

        public Node() { }
        public Node(T data) : this(data, null) { }
        public Node(T data, NodeList<T> neighbors)
        {
            this.data = data;
            this.neighbors = neighbors;
        }

        public T Value
        {
            get
            {
                return data;
            }
            set
            {
                data = value;
            }
        }

        public NodeList<T> Neighbors
        {
            get
            {
                return neighbors;
            }
            set
            {
                neighbors = value;
            }
        }

        public void ResetNeighbors(NodeList<T> newNeighbors)
        {
            this.neighbors = null;
            this.neighbors = newNeighbors;
        }

    }
}
