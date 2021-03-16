using System;
using System.Collections.Generic;

namespace Graphs
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph<string> testGraph = new Graph<string>(false, false, "A"); //Initialise undirected, weightless graph, with a point A
            testGraph.AddPoint("B", new List<string> { "A" }); //Adds a point B, with a connection to point A
            testGraph.AddPoint("C", new List<string> { "A" });
            testGraph.AddPoint("D", new List<string> { "A", "C" });
            testGraph.AddPoint("E", new List<string> { "B" });
            testGraph.AddPoint("F", new List<string> { "D" });
            testGraph.AddPoint("G", new List<string> { "E" });
            Console.WriteLine(testGraph.Connected("A", "B")); //Checks if connection was made backwards
            Console.WriteLine();
            testGraph.AddPoint("ExtraPoint");
            testGraph.AddConnection("ExtraPoint", "C");
            PrintList(testGraph.Neighbours("C"));
            testGraph.RemoveConnection("C", "D");
            PrintList(testGraph.Neighbours("C"));
            testGraph.Remove("ExtraPoint");
            testGraph.AddConnection("C", "D");
            PrintList(testGraph.Neighbours("C"));
            PrintList(testGraph.BreadthFirst("A"));
            Console.Read();
        }

        static void PrintList(List<string> list)
        {
            foreach(string s in list)
            {
                Console.WriteLine(s);
            }
            Console.WriteLine();
        }
    }
}
