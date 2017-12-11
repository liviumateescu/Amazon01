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

        static void Main(string[] args)
        {
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

            int[,] adjMatrix = new int[nrOrase, nrOrase];

            foreach (drum d in _drumuriInitiale)
            {
                adjMatrix[d.oras1 - 1, d.oras2 - 1] = 1;
                adjMatrix[d.oras2 - 1, d.oras1 - 1] = 1;
            }
            
            for (int i = 0; i < nrOrase; i++)
            {
                for (int j = 0; j < nrOrase; j++)
                {
                    Console.Write("{0} ", adjMatrix[i, j]);
                }
                Console.WriteLine();
            }

            Graph<string> planeta = new Graph<string>();
            //adauga planetele
            for (int i = 0; i < nrOrase; i++)
            {
                planeta.AddNode((i+1).ToString());
            }
            //adauga drumurile
            foreach (drum d in _drumuriInitiale)
            {
                GraphNode<string> from = (GraphNode<string>)planeta.Nodes.FindByValue(d.oras1.ToString());
                GraphNode<string> to = (GraphNode<string>)planeta.Nodes.FindByValue(d.oras2.ToString());
                planeta.AddUndirectedEdge(from, to);
            }

            Console.ReadKey();
        }

        public static List<drum> GetInitialRoads()
        {
            int[][] listaFisier = File.ReadAllLines("input.txt")
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

    }
}
