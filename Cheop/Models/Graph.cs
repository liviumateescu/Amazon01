using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Cheop.Models
{
    //[Serializable]
    public class Graph<T> : IEnumerable<T>
    {
        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        private NodeList<T> nodeSet;  // ??????????????????????????????????????
        //private NodeList<T> nodesPosition;
        private int edgeCount = 0;

        public Graph() : this(null) { }
        public Graph(NodeList<T> nodeSet)
        {
            if (nodeSet == null)
                this.nodeSet = new NodeList<T>();
            else
                this.nodeSet = nodeSet;
        }

        public void AddNode(GraphNode<T> node)
        {
            nodeSet.Add(node);
        }

        public void AddNode(T value)
        {
            nodeSet.Add(new GraphNode<T>(value));
        }

        //
        // Summary:
        //     Adauga o latura neorientata intr-un obiect de tipul NodeList<T>, de la "from" la "to".
        public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to, int cost)
        {
            from.Neighbors.Add(to);
            from.Costs.Add(cost);
            to.Neighbors.Add(from);
            to.Costs.Add(cost);
        }

        //
        // Summary:
        //     Adauga o latura neorientata intr-un un obiect de tipul NodeList<T>, de la "from" la "to".
        public void AddUndirectedEdge(GraphNode<T> from, GraphNode<T> to)
        {
            AddUndirectedEdge(from, to, 0);
            edgeCount++;
        }

        public void AddUndirectedEdge(Road<T> drum)
        {
            AddUndirectedEdge(drum.road.Key, drum.road.Value, 0);
        }

        public void AddUndirectedEdge(RoadList<T> listaDeDrumuri)
        {
            foreach (Road<T> drum in listaDeDrumuri)
            {
                AddUndirectedEdge(drum);
            }
        }

        public bool Contains(T value)
        {
            return nodeSet.FindByValue(value) != null;
        }

        public bool Remove(T value)
        {
            //first remove the node from the nodeset
            GraphNode<T> nodeToRemove = (GraphNode<T>) nodeSet.FindByValue(value);
            if (nodeToRemove == null) //node not found
                return false;
            nodeSet.Remove(nodeToRemove);

            foreach (GraphNode<T> gnode in nodeSet) //enumerate the nodeSet, removing edges to this node
            {
                int index = gnode.Neighbors.IndexOf(nodeToRemove);
                if (index != -1)
                {
                    gnode.Neighbors.RemoveAt(index);
                    gnode.Costs.RemoveAt(index);
                }
            }
            return true;
        }

        public NodeList<T> Nodes
        {
            get
            {
                return nodeSet;
            }
        }

        /*public NodeList<T> NodesPosition
        {
            get
            {
                return nodesPosition;
            }
        }*/

        public int Count
        {
            get { return nodeSet.Count; }
        }

        public int EdgeCount
        {
            get
            {
                int sum = 0;
                foreach (GraphNode<T> nod in this.nodeSet)
                {
                    sum = sum + nod.NumberOfNeighbors;
                }
                return sum/2;

            }
        }
        
        public bool Teleport(T oras1, T oras2)
        {

            return true;
        }

        public void PrintGraph()
        {
            Console.WriteLine("-------- PLANETA --------");
            Console.WriteLine("Numarul de orase = {0}.", this.Count);
            Console.WriteLine("Planeta are {0} drumuri dintr-un total de {1} posibile.", this.EdgeCount, this.Count*(this.Count-1) /2);
            foreach (GraphNode<T> nod in this.nodeSet)
            {
                Console.WriteLine("Orasul {0} are {1} vecini.", nod.Value, nod.NumberOfNeighbors);
            }
            Console.WriteLine("-------------------------");
        }
    }
}
