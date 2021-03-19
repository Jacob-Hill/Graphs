using System;
using System.Collections.Generic;

namespace Graphs
{
    class Program
    {
        static void Main(string[] args)
        {
            Graph<string> testGraph1 = new Graph<string>(false, false, "A"); //Initialise undirected, weightless graph, with a point A
            testGraph1.AddPoint("B", new List<string> { "A" }); //Adds a point B, with a connection to point A
            testGraph1.AddPoint("C", new List<string> { "A" });
            testGraph1.AddPoint("D", new List<string> { "A", "C" });
            testGraph1.AddPoint("E", new List<string> { "B" });
            testGraph1.AddPoint("F", new List<string> { "D" });
            testGraph1.AddPoint("G", new List<string> { "E" });
            Console.WriteLine(testGraph1.Connected("A", "B")); //Checks if connection was made backwards
            Console.WriteLine();
            testGraph1.AddPoint("ExtraPoint");
            testGraph1.AddConnection("ExtraPoint", "C");
            PrintList(testGraph1.Neighbours("C"));
            testGraph1.RemoveConnection("C", "D");
            PrintList(testGraph1.Neighbours("C"));
            testGraph1.Remove("ExtraPoint");
            testGraph1.AddConnection("C", "D");
            PrintList(testGraph1.Neighbours("C"));
            PrintList(testGraph1.BreadthFirst("A"));
            Graph<string> testGraph2 = new Graph<string>(false, true, "A");
            testGraph2.AddPoint("B", new List<(string, float)> { ("A", 2f) });
            testGraph2.AddPoint("C", new List<(string, float)> { ("A", 10f) });
            testGraph2.AddPoint("D", new List<(string, float)> { ("B", 3f), ("C", 6f) });
            testGraph2.AddPoint("E", new List<(string, float)> { ("B", 9f) });
            testGraph2.AddPoint("F", new List<(string, float)> { ("D", 11f), ("E", 5f) });
            testGraph2.AddPoint("G", new List<(string, float)> { ("C", 8f), ("D", 7f) });
            testGraph2.AddPoint("H", new List<(string, float)> { ("F", 6f), ("G", 1f) });
            PrintList(testGraph2.ShortestRoute("A", "H"));
            Console.WriteLine(testGraph2.Distance("A", "H"));
            Console.WriteLine();
            PrintList(testGraph2.ShortestRoute("F", "C"));
            Console.Write(testGraph2.Distance("F", "C"));
            Console.WriteLine();
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
