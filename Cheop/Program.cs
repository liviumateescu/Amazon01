using System;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Cheop.Models;
using Cheop.Util;
using Cheop.Exceptions;

namespace Cheop
{
    class Program
    {
        private static List<drum> _drumuriInitiale { get; set; }
        public static int nrOrase;
        public static int nrDrumuri;
        public static Dictionary<int, GraphNode<string>> Topologie;

        static void Main(string[] args)
        {
            //
            // >>>>>>>>>>>>>>>>>>>>>>>>>> CITESTE DIN FISIER <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 
            //
            try
            {
                _drumuriInitiale = GetInitialRoads();

                int maxDrumuri = nrOrase * (nrOrase - 1) / 2;

                Console.WriteLine("Unic? {0}", Utilities.AllUnique(_drumuriInitiale));
                Console.WriteLine("Numarul de drumuri existente = {0}", nrDrumuri);
                Console.WriteLine("Numarul maxim de drumuri posibile = {0}", maxDrumuri);
                Console.WriteLine("Numarul de drumuri posibile = {0}", maxDrumuri);

            }
            catch (Exception e)
            {
                Console.WriteLine($"Exception occured:\n{e.Message}");
            }

            //
            // >>>>>>>>>>>>>>>>>>>>>>>>>> INAINTE DE EXPLOZIE <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 
            //
            int[,] adjMatrix = new int[nrOrase, nrOrase];
            Topologie = new Dictionary<int, GraphNode<string>>();
            foreach (drum d in _drumuriInitiale)
            {
                adjMatrix[d.oras1 - 1, d.oras2 - 1] = 1;
                adjMatrix[d.oras2 - 1, d.oras1 - 1] = 1;
            }
            PrintMatrix(adjMatrix);

            Graph<string> PlanetaInitiala = new Graph<string>();
            //adauga planetele in obiectul Graph
            for (int i = 0; i < nrOrase; i++)
            {
                PlanetaInitiala.AddNode((i + 1).ToString());
                //Topologie.Add(i+1,);
            }
            //adauga drumurile in obiectul Graph
            foreach (drum d in _drumuriInitiale)
            {
                GraphNode<string> from = (GraphNode<string>)PlanetaInitiala.Nodes.FindByValue(d.oras1.ToString());
                GraphNode<string> to = (GraphNode<string>)PlanetaInitiala.Nodes.FindByValue(d.oras2.ToString());
                PlanetaInitiala.AddUndirectedEdge(from, to);
            }


            //
            // >>>>>>>>>>>>>>>>>>>>>>>>>> DUPA EXPLOZIE <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            //
            // Creaza noaua matrice de drumuri dupa explozie
            int[,] NewAdjMatrix = new int[nrOrase, nrOrase];
            for (int i = 0; i < nrOrase; i++)
            {
                for (int j = 0; j < nrOrase; j++)
                {
                    if ((adjMatrix[i, j] == 0) & (i != j))
                    {
                        NewAdjMatrix[i, j] = 1;
                    }
                }
            }
            Console.WriteLine("++++++++++++++++++++++++++++++++++++");
            PrintMatrix(NewAdjMatrix);

            //Adauga orasele initiale
            Graph<string> PlanetaDupaExplozie = new Graph<string>();
            for (int i = 0; i < PlanetaInitiala.Count; i++)
            {
                PlanetaDupaExplozie.AddNode((i + 1).ToString());
            }
            //Adauga drumurile dupa explozie
            Console.WriteLine("Dupa explozie au fost adaugate {0} drumuri.", GraphFromMatrix(out PlanetaDupaExplozie, NewAdjMatrix));
            Console.WriteLine("Inaine de explozie au fost {0} drumuri.", PlanetaInitiala.EdgeCount);
            Console.WriteLine("Dupa explozie sunt {0} drumuri.", PlanetaDupaExplozie.EdgeCount);
            Console.WriteLine("Numar corect de drumuri? -> {0}", (PlanetaInitiala.EdgeCount + PlanetaDupaExplozie.EdgeCount) == (nrOrase * (nrOrase - 1) / 2));
            bool AddNewRoads = !(PlanetaInitiala.EdgeCount == PlanetaDupaExplozie.EdgeCount);
            Console.WriteLine("Mai este nevoie de noi drumuri? -> {0}", AddNewRoads);
            //
            // >>>>>>>>>>>>>>>>>>>>>>>>>> ADAUGARE DRUMURI NOI <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            //
            bool SOLUTIE = false;
            /*while (!SOLUTIE)
            {
                int NrDrumuriTotal = nrOrase * (nrOrase - 1) / 2;
                int NrDrumuriInitiale = PlanetaInitiala.EdgeCount;
                int NrDrumuriFinale = PlanetaDupaExplozie.EdgeCount;
                bool AdaugDrumuriInitiale = NrDrumuriInitiale < NrDrumuriFinale;
                bool AdaugOrase = NrDrumuriInitiale > NrDrumuriFinale;

                if (AdaugOrase)
                {
                    AdaugaOrase();
                }

                if (NrDrumuriInitiale == NrDrumuriFinale) // conditia 1 indeplinita (acelasi nr de drumuri)
                {
                    bool PoateFiSolutie = PoateFiSol(PlanetaInitiala, PlanetaDupaExplozie);
                    Console.WriteLine("Poate fi solutie? -> {0}", PoateFiSolutie);
                    if (PoateFiSolutie) //conditia 2 indeplinita (orase cu numar identic de drumuri)
                    {
                        SOLUTIE = Algoritm();
                    }
                }
                else
                {
                    //ModificaUnDrumDinCeleAdaugate;
                }
            }*/
            //
            // parcurgere si adaugare laturi
            //
            RoadList<string> DrumuriDeAdaugat = new RoadList<string>(); //va contine toate drumurile suplimentare care pot fi adaugate, prin asta voi merge cu foreach
            Dictionary<Road<string>, bool> DrumVerificat = new Dictionary<Road<string>, bool>(); //dictionar in care tin seama daca am suplimentat sau nu un drum
            Queue<Road<string>> DrumQueue = new Queue<Road<string>>();
            foreach (GraphNode<string> nod in PlanetaInitiala.Nodes) // parcurge fiecare nod
            {
                // creaza laturile posibile a fi adaugate pentru fiecare nod
                //RoadList<string> DrumuriDeAdaugat = new RoadList<string>();
                NodeList<string> diferenta = GetDiff(PlanetaInitiala.Nodes, nod.Neighbors);
                diferenta.Remove(nod);
                foreach (GraphNode<string> nodNou in diferenta)
                {
                    DrumuriDeAdaugat.Add(new Road<string>(nod, nodNou));
                }
            }
            Console.WriteLine(DrumuriDeAdaugat.ToString());

            foreach (Road<string> road in DrumuriDeAdaugat)
            {
                if (!DrumVerificat.ContainsKey(road))
                {
                    DrumQueue.Enqueue(road);
                    while (DrumQueue.Count != 0)
                    {
                        Road<string> drumDeLucru = DrumQueue.Dequeue();
                        DrumVerificat[drumDeLucru] = true;
                        // TODO :   1. adauga drumdelucru la drumurile din planetainitiala
                        //          2. verifica daca noua soultie are sens - PoateFiSol - intrun while: while not poatefisol.....

                        foreach (Road<string> item in collection)
                        {

                        }
                    }
                }
            }



            //
            // >>>>>>>>>>>>>>>>>>>>>>>>>> PARCURGERE GRAPH <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
            //
            Dictionary<GraphNode<string>, bool> visited = new Dictionary<GraphNode<string>, bool>();
            Queue<GraphNode<string>> worklist = new Queue<GraphNode<string>>();

            foreach (GraphNode<string> nod in PlanetaInitiala.Nodes) // doar pentru a sari la grafuri partiale
            {
                if (!visited.ContainsKey(nod)) //daca nu e vizitat incepe sa vizitezi altele din el
                {
                    worklist.Enqueue(nod);
                    while (worklist.Count != 0)
                    {
                        GraphNode<string> node = worklist.Dequeue();
                        visited[node] = true;
                        foreach (GraphNode<string> neighbor in node.Neighbors)
                        {
                            if (!visited.ContainsKey(neighbor))
                            {
                                visited.Add(neighbor, false);
                                worklist.Enqueue(neighbor);
                            }
                        }
                    }
                }
            }


            foreach (KeyValuePair<GraphNode<string>, bool> entry in visited)
            {
                Console.WriteLine("{0} -> {1}", entry.Key.Value, entry.Value);
            }


            Console.ReadKey();
        }


        public static List<drum> GetInitialRoads()
        {
            int[][] listaFisier = File.ReadAllLines("../Cheop/InputFiles/input.txt")
                    .Select(l => l.Split(' ').Select(i => int.Parse(i)).ToArray())
                    .ToArray();

            nrOrase = listaFisier[0][0];
            if (nrOrase > 2000)
            {
                throw new PreaMulteOraseException("Mai mult de 2.000 de orase!");
            }

            nrDrumuri = listaFisier[0][1];
            if (nrDrumuri > 1000000)
            {
                throw new PreaMulteAutostraziException("Mai mult de 1.000.000 de autostrazi!");
            }

            if (listaFisier.GetLength(0) - 1 != listaFisier[0][1])
            {
                throw new FisierInconsistentException("Numarul de autostrazi nu se potriveste cu numarul de inregistrari din fisier!");
            }

            List<drum> drumuri = new List<drum>();
            for (int i = 1; i <= nrDrumuri; i++)
            {
                drumuri.Add(new drum { oras1 = listaFisier[i][0], oras2 = listaFisier[i][1] });
            };
            return drumuri;
        }

        public static int GraphFromMatrix(out Graph<string> graph, int[,] aM)
        {
            int NrOfRoads = 0;
            graph = new Graph<string>();
            for (int i = 0; i < aM.GetLength(0); i++)
            {
                graph.AddNode((i + 1).ToString());
            }

            foreach (GraphNode<string> nod in graph.Nodes)
            {
                int i = nod.NodeNumber - 1;
                for (int j = nod.NodeNumber - 1; j < aM.GetLength(0); j++)
                {
                    if (aM[i, j] == 1)
                    {
                        GraphNode<string> from = (GraphNode<string>)graph.Nodes.FindByValue((i + 1).ToString());
                        GraphNode<string> to = (GraphNode<string>)graph.Nodes.FindByValue((j + 1).ToString());
                        graph.AddUndirectedEdge(from, to);
                        NrOfRoads++;
                    }
                }

            }
            return NrOfRoads;
        }

        public static int MatrixFromGraph(out int[,] aM, Graph<string> graph)
        {
            int NrOfRoads = 0;
            aM = new int[graph.Count, graph.Count];
            foreach (GraphNode<string> nod in graph.Nodes)
            {
                foreach (Node<string> neighbour in nod.Neighbors)
                {
                    aM[nod.NodeNumber, neighbour.NodeNumber] = 1;
                    NrOfRoads++;
                }
            }
            return NrOfRoads / 2;
        }

        public static void PrintMatrix(int[,] m)
        {
            for (int i = 0; i < m.GetLength(0); i++)
            {
                for (int j = 0; j < m.GetLength(1); j++)
                {
                    Console.Write("{0} ", m[i, j]);
                }
                Console.WriteLine();
            }
        }

        public static void Teleportare(GraphNode<string> oras1, GraphNode<string> oras2)
        {
            Topologie[oras1.NodeNumber] = oras2;
            Topologie[oras2.NodeNumber] = oras1;
        }

        public static bool PoateFiSol(Graph<string> initial, Graph<string> dupaExplozie)
        {
            // Ex. legaturi[x] = 5 inseamna ca exista 5 orase din care pleaca x drumuri 
            int[] legaturiInitial = new int[nrOrase - 1];
            int[] legaturiFinal = new int[nrOrase - 1];
            foreach (GraphNode<string> nod in initial.Nodes)
            {
                legaturiInitial[nod.NumberOfNeighbors] = legaturiInitial[nod.NumberOfNeighbors] + 1;
            }
            foreach (GraphNode<string> nod in dupaExplozie.Nodes)
            {
                legaturiFinal[nod.NumberOfNeighbors] = legaturiFinal[nod.NumberOfNeighbors] + 1;
            }
            return legaturiInitial.SequenceEqual(legaturiFinal);
        }

        public static bool Algoritm()
        {
            throw new NotImplementedException();
        }

        private static void AdaugaOrase()
        {
            throw new NotImplementedException();
        }

        public static NodeList<string> GetDiff(NodeList<string> first, NodeList<string> second)
        {
            NodeList<string> diferenta = new NodeList<string>();
            foreach (GraphNode<string> nod in first)
            {
                if (!second.Contains(nod))
                {
                    diferenta.Add(nod);
                }
            }
            return diferenta;
        }

        public Road<string> GetInverseRoad(Road<string> undrum)
        {
            return new Road<string>(undrum.road.Value, undrum.road.Key);
        }
    }
}
