using System;
using System.Text;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Cheop.Models;
using Cheop.Util;
using Cheop.Exceptions;
using Cheop.Combinatorics;

namespace Cheop
{
    class Program
    {
        private static List<drum> _drumuriInitiale { get; set; }
        public static int nrOrase;
        public static int nrDrumuri;
        public static int nrOraseDinFisier;
        public static int nrDrumuriDinFisier;
        public static Dictionary<int, GraphNode<string>> Topologie;

        static void Main(string[] args)
        {
            //
            // >>>>>>>>>>>>>>>>>>>>>>>>>> CITESTE DIN FISIER <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<< 
            //
            try
            {
                _drumuriInitiale = GetInitialRoads();

                int maximumDrumuri = nrOrase * (nrOrase - 1) / 2;

                Console.WriteLine("Unic? {0}", Utilities.AllUnique(_drumuriInitiale));
                Console.WriteLine("Numarul de drumuri existente = {0}", nrDrumuri);
                Console.WriteLine("Numarul maxim de drumuri posibile = {0}", maximumDrumuri);
                Console.WriteLine("Numarul de drumuri posibile = {0}", maximumDrumuri);
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
            //PrintMatrix(adjMatrix);

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
            PlanetaInitiala.PrintGraph();
            //_________________________________ Avem PlanetaInitiala __________________________________________

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
            //Console.WriteLine("++++++++++++++++++++++++++++++++++++");
            //PrintMatrix(NewAdjMatrix);

            //Adauga orasele initiale
            Graph<string> PlanetaDupaExplozie_DinFisier = new Graph<string>();
            for (int i = 0; i < PlanetaInitiala.Count; i++)
            {
                PlanetaDupaExplozie_DinFisier.AddNode((i + 1).ToString());
            }
            //Adauga drumurile dupa explozie
            //Console.WriteLine("Dupa explozie au fost adaugate {0} drumuri.", GraphFromMatrix(out PlanetaDupaExplozie, NewAdjMatrix)); //au fost adaugate drumurile in Graf
            GraphFromMatrix(out PlanetaDupaExplozie_DinFisier, NewAdjMatrix);
            PlanetaDupaExplozie_DinFisier.PrintGraph();
            //____________________________________ Avem PlanetaDupaExplozie ______________________________________

            nrOraseDinFisier = nrOrase;
            nrDrumuriDinFisier = nrDrumuri;

            bool EXISTA_SOLUTIE = false;
            bool ADAUGA_ORAS_PUNCT_MORT = false;

            int numardewhileuri = 0;
            while (!EXISTA_SOLUTIE)
            {
                //
                // >>>>>>>>>>>>>>>>>>>>>>>>>> VERIFICA DACA MAI TREBUIE ADAUGATE ORASE <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //
                int NumarInitialDeOrase = nrOrase;
                int NumarNouDeOrase = nrOrase;

                int maxDrumuri = nrOrase * (nrOrase - 1) / 2; // cand adaug un oras se schimba numarul maxim de drumuri posibile

                int AddNewCities_01 = maxDrumuri % 2;
                double NumarDrumuriDeAdaugat = maxDrumuri / 2 - nrDrumuri; //nrDrumuri ramane acelasi citit din fisier, nu adaug niciun drum, doar orase

                // conditiile in care se adauga un oras nou: daca in declaratie sunt TRUE => se adauga un oras nou
                // se adauga un oras nou cand CEL PUTIN UNA din conditii este FALSE
                bool conditia1 = (AddNewCities_01 == 0); // daca restul e zero = se pot genera atatea drumuri cate au fost, daca restul e 1, numarul total de drumuri e impar si trebuie sa adaug oras
                bool conditia2 = (NumarDrumuriDeAdaugat >= 0); // daca NumarDrumuriDeAdaugat >= 0 inseamna ca dupa explozie raman destule drumuri ca sa pot reconstitui graful initial
                bool conditia3 = (NumarDrumuriDeAdaugat == Math.Floor(NumarDrumuriDeAdaugat)); // numarul de drumuri care trebuie adaugate trebuie sa fie intreg, duh!
                bool conditia4 = ADAUGA_ORAS_PUNCT_MORT;

                while ((!conditia1) | (!conditia2) | (!conditia3) | (ADAUGA_ORAS_PUNCT_MORT))
                {
                    NumarNouDeOrase++;
                    PlanetaInitiala.AddNode(NumarNouDeOrase.ToString());

                    maxDrumuri = NumarNouDeOrase * (NumarNouDeOrase - 1) / 2;
                    AddNewCities_01 = maxDrumuri % 2;
                    if (AddNewCities_01 == 0)
                    {
                        conditia1 = true;
                    }
                    else
                    {
                        conditia1 = false;
                    }

                    NumarDrumuriDeAdaugat = maxDrumuri / 2 - nrDrumuri;
                    if (NumarDrumuriDeAdaugat >= 0)
                    {
                        conditia2 = true;
                    }
                    else
                    {
                        conditia2 = false;
                    }


                    if (NumarDrumuriDeAdaugat == Math.Floor(NumarDrumuriDeAdaugat))
                    {
                        conditia3 = true;
                    }
                    else
                    {
                        conditia3 = false;
                    }

                    if (ADAUGA_ORAS_PUNCT_MORT)
                    {
                        conditia4 = false;
                        ADAUGA_ORAS_PUNCT_MORT = false;
                    }
                }
                int NumarDeOraseAdaugate = NumarNouDeOrase - NumarInitialDeOrase;
                nrOrase = NumarNouDeOrase;

                Console.WriteLine("Au mai fost adaugate {0} orase.", NumarDeOraseAdaugate);
                Console.WriteLine("Numar total de orase dupa adaugare de orase = {0}", nrOrase);

                Graph<string> PlanetaDupaExplozie = new Graph<string>();
                if (NumarDeOraseAdaugate > 0) // Recalculeaza si PlanetaDupaExplozie daca s-au modificat numarul de orase
                {
                    PlanetaDupaExplozie = CreazaPlanetaDupaExplozie(PlanetaInitiala);
                }
                else
                {
                    PlanetaDupaExplozie = PlanetaDupaExplozie_DinFisier; // DUBIOUS ??????????? - copiere de referinta, eh plm
                }
                Console.WriteLine("++++++++++++++++++++++++++++++++++");
                PlanetaInitiala.PrintGraph();
                PlanetaDupaExplozie.PrintGraph();
                Console.WriteLine("++++++++++++++++++++++++++++++++++");
                //____________________________________ Avem numarul de orase corect ______________________________________


                //
                // >>>>>>>>>>>>>>>>>>>>>>>>>> VERIFICA DACA MAI TREBUIE ADAUGATE DRUMURI NOI <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //
                Console.WriteLine("Inaine de explozie au fost {0} drumuri.", PlanetaInitiala.EdgeCount);
                Console.WriteLine("Dupa explozie sunt {0} drumuri.", PlanetaDupaExplozie.EdgeCount);
                Console.WriteLine("Numar corect de drumuri? -> {0}", (PlanetaInitiala.EdgeCount + PlanetaDupaExplozie.EdgeCount) == (nrOrase * (nrOrase - 1) / 2));
                bool AddNewRoads = PlanetaInitiala.EdgeCount < PlanetaDupaExplozie.EdgeCount;
                Console.WriteLine("Mai este nevoie de noi drumuri? -> {0}", AddNewRoads);
                int nrOfRoadsToBeAdded = 0;
                if (AddNewRoads)
                {
                    nrOfRoadsToBeAdded = ((nrOrase * (nrOrase - 1) / 2 / 2) - PlanetaInitiala.EdgeCount);
                }
                Console.WriteLine("Mai este nevoie de {0} noi drumuri.", nrOfRoadsToBeAdded);

                //
                // parcurgere si adaugare laturi
                //

                if (AddNewRoads)
                {
                    //
                    // >>>>>>>>>>>>>>>>>>>>>>>>>> ADAUGARE DRUMURI NOI <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                    //
                    RoadList<string> DrumuriDeAdaugat = new RoadList<string>(); //va contine toate drumurile suplimentare care pot fi adaugate, prin asta voi merge cu foreach
                    List<RoadList<string>> CombinatiiDrumuriDeAdaugat = new List<RoadList<string>>();
                    Dictionary<Road<string>, bool> DrumVerificat = new Dictionary<Road<string>, bool>(); //dictionar in care tin seama daca am suplimentat sau nu un drum
                    Queue<Road<string>> DrumQueue = new Queue<Road<string>>();
                    foreach (GraphNode<string> nod in PlanetaInitiala.Nodes) // parcurge fiecare nod
                    {
                        // creaza laturile posibile a fi adaugate pentru fiecare nod
                        NodeList<string> diferenta = GetDiff(PlanetaInitiala.Nodes, nod.Neighbors);
                        diferenta.Remove(nod);
                        foreach (GraphNode<string> nodNou in diferenta)
                        {
                            DrumuriDeAdaugat.Add(new Road<string>(nod, nodNou));
                        }
                    }
                    Console.WriteLine(DrumuriDeAdaugat.ToString());
                    // creaza toate combinatiile posibile de drumuri care pot fi adaugate
                    CombinatiiDrumuriDeAdaugat = GetCombinations(DrumuriDeAdaugat, nrOfRoadsToBeAdded);

                    //PlanetaInitialaDeLucru.PrintGraph();

                    // in acest punct am :
                    //          - o planeta initiala pentru care stiu cate drumuri trebuie sa adaug ca sa o fac solutie posibila 
                    //          - toate combinatiile posibile de drumuri care pot fi adaugate
                    // mai departe:
                    //          - incep sa adaug perechi de drumuri in planeta initiala
                    //          - recalculez planeta dupa explozie
                    //          - verific daca cele doua topologii pot fi rezolvate
                    bool GasitSolutie = false;
                    int pasCurent = 0;
                    while ((!GasitSolutie) & (pasCurent < CombinatiiDrumuriDeAdaugat.Count))
                    {
                        Graph<string> PlanetaInitiala_DeLucru = new Graph<string>(PlanetaInitiala.Nodes); // copie de lucru
                                                                                                          //PlanetaInitiala_DeLucru.PrintGraph();
                        Graph<string> PlanetaDupaExplozie_DeLucru = CreazaPlanetaDupaExplozie(PlanetaInitiala_DeLucru); //creeaza noua planeta dupa explozie
                                                                                                                //PlanetaDupaExplozie_DeLucru.PrintGraph();
                                                                                                                //Console.WriteLine(CombinatiiDrumuriDeAdaugat[pasCurent].ToString());
                        PlanetaInitiala_DeLucru.AddUndirectedEdge(CombinatiiDrumuriDeAdaugat[pasCurent]); // adauga drumuri noi in planeta initiala
                        Console.WriteLine("Se adauga urmatorul drum: {0}", CombinatiiDrumuriDeAdaugat[pasCurent].ToString());
                        Console.WriteLine("Planeta initala de lucru dupa adaugare noi drumuri:");
                        PlanetaInitiala_DeLucru.PrintGraph();
                        PlanetaDupaExplozie_DeLucru = CreazaPlanetaDupaExplozie(PlanetaInitiala_DeLucru); // creeaza planeta dupa explozie din graful modificat de dinainte de exlozie
                        Console.WriteLine("Planeta dupa explozie de lucru dupa adaugare noi drumuri:");
                        PlanetaDupaExplozie_DeLucru.PrintGraph();

                        if (PoateFiSol(PlanetaInitiala_DeLucru, PlanetaDupaExplozie_DeLucru))
                        {
                            GasitSolutie = true;
                            Console.WriteLine("-----gasita la pas {0}", pasCurent);
                            // >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> incepe sa schimbi nodurile intre ele <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                        }
                        else
                        {
                            foreach (Road<string> road in CombinatiiDrumuriDeAdaugat[pasCurent])
                            {
                                PlanetaInitiala.Nodes
                            }
                            pasCurent++;
                            Console.WriteLine("Graful propus nu poate fi solutie. Se trece la urmatorul pas si se adauga alt grup de drumuri.");
                        }
                        //PlanetaInitialaDeLucru.PrintGraph();
                    }

                    if ((GasitSolutie) & (pasCurent < CombinatiiDrumuriDeAdaugat.Count)) //a fost gasita o solutie -> scrie in fisier 
                    {
                        Console.WriteLine("Solutia este:");
                        Console.WriteLine("");
                        Console.WriteLine(CombinatiiDrumuriDeAdaugat[pasCurent]);
                        EXISTA_SOLUTIE = true;
                    }

                    // daca nu a fost gasita o solutie dupa ce s-au adaugat toate combinatiile de drumuri posibilie
                    // inseamna ca graful asta nu poate avea nicio solutie si atunci trebuie sa ma intorc sa adaug oras
                    if ((!GasitSolutie) & (pasCurent >= CombinatiiDrumuriDeAdaugat.Count))
                    {
                        ADAUGA_ORAS_PUNCT_MORT = true; //adauga oras
                    }

                }
                else
                {
                    Console.WriteLine("Nu mai trebuie sa adaug niciun drum nou dar trebuie sa verific daca chestia asta poate fi solutie.");
                    bool PoateFiSolutie = PoateFiSol(PlanetaInitiala, PlanetaDupaExplozie);
                    Console.WriteLine("Poate fi solutie? => {0}", PoateFiSolutie);
                    if (PoateFiSolutie)
                    {
                        //a fost gasita o solutie -> scrie in fisier
                        EXISTA_SOLUTIE = true;
                    }
                    else
                    {
                        // daca asta nu este solutie trebuie sa ma intorc sa adaug un oras
                        ADAUGA_ORAS_PUNCT_MORT = true; // adauga oras
                    }
                }
                numardewhileuri++;
            }

            Console.ReadKey();
        }

        public static List<drum> GetInitialRoads()
        {
            int[][] listaFisier = File.ReadAllLines("../Cheop/InputFiles/01_AMS.txt")
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
            int[] legaturiInitial = new int[initial.Count];
            int[] legaturiFinal = new int[dupaExplozie.Count];
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

        // diferenta dintre doua NodeList.
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

        public static Road<string> GetInverseRoad(Road<string> undrum)
        {
            return new Road<string>(undrum.road.Value, undrum.road.Key);
        }

        public static List<RoadList<string>> GetCombinations(RoadList<string> toateDrumurile, int combinationLength)
        {
            List<RoadList<string>> combinatii = new List<RoadList<string>>();
            Combinations<Road<string>> combinations = new Combinations<Road<string>>(toateDrumurile, combinationLength);

            //string cformat = "Combinations of {{A B C D}} choose 2: size = {0}";
            //Console.WriteLine(String.Format(cformat, combinations.Count));

            foreach (IList<Road<string>> c in combinations)
            {
                RoadList<string> listadrumuri = new RoadList<string>();
                foreach (Road<string> road in c)
                {
                    listadrumuri.Add(road);
                }
                combinatii.Add(listadrumuri);
                //Console.WriteLine(String.Format("{{{0} {1}}}", c[0], c[1]));
            }
            /*foreach (RoadList<string> c in combinatii)
            {
                foreach (var d in c)
                {
                    Console.WriteLine(d);
                }
                Console.WriteLine("_______________________________");
            }*/

            return combinatii;
        }

        public static Graph<string> CreazaPlanetaDupaExplozie(Graph<string> plInainteDeExplozie)
        {
            Graph<string> plDupaExplozie = new Graph<string>();
            int numar_orase = plInainteDeExplozie.Count;
            int[,] mInitial = new int[numar_orase, numar_orase];
            int[,] mFinal = new int[numar_orase, numar_orase];

            foreach (GraphNode<string> nod in plInainteDeExplozie.Nodes)
            {
                int from = nod.NodeNumber - 1;
                foreach (GraphNode<string> vecin in nod.Neighbors)
                {
                    int to = vecin.NodeNumber - 1;
                    mInitial[from, to] = 1;
                }
            }
            for (int i = 0; i < numar_orase; i++)
            {
                for (int j = 0; j < numar_orase; j++)
                {
                    if ((mInitial[i, j] == 0) & (i != j))
                    {
                        mFinal[i, j] = 1;
                    }
                }
            }

            //PrintMatrix(mInitial);
            //PrintMatrix(mFinal);

            for (int i = 0; i < plInainteDeExplozie.Count; i++)
            {
                plDupaExplozie.AddNode((i + 1).ToString());
            }
            //Console.WriteLine("Dupa explozie au fost adaugate {0} drumuri.", GraphFromMatrix(out plDupaExplozie, mFinal)); //au fost adaugate drumurile in Graf
            GraphFromMatrix(out plDupaExplozie, mFinal);

            //plDupaExplozie.PrintGraph();



            //Graph<string> plDupaExplozie = new Graph<string>(plInainteDeExplozie.Nodes);
            //Console.WriteLine("********************");
            //planetaDupaExplozie.PrintGraph();
            //Console.WriteLine("********************");
            /*Graph<string> planetaDupaExplozie = new Graph<string>();
            for (int i = 0; i < nrOrase; i++)
            {
                planetaDupaExplozie.AddNode((i + 1).ToString());
            }*/

            /*Graph<string> plDupaExplozie = new Graph<string>();
            for (int i = 0; i < nrOrase; i++)
            {
                plDupaExplozie.AddNode((i + 1).ToString());
            }

            foreach (GraphNode<string> nod in plInainteDeExplozie.Nodes)
            {
                NodeList<string> diferenteVecini = GetDiff(plInainteDeExplozie.Nodes, nod.Neighbors); // lista cu vecinii ficarui nod catre care trebuie sa existe drumuri
                bool remok = diferenteVecini.Remove(nod);
                foreach (GraphNode<string> vecin in diferenteVecini)
                {
                    plDupaExplozie.AddUndirectedEdge(nod, vecin);
                }

                //diferenteVecini.Remove(nod);
                //foreach (GraphNode<string> vecin in nod.Neighbors)
                //{
                //nod.ResetNeighbors(diferenteVecini);
                //}
            }
            //Console.WriteLine("???????????????????????????");
            //plDupaExplozie.PrintGraph();
            //Console.WriteLine("??????????????????????????");
            */
            return plDupaExplozie;
        }
    }
}
