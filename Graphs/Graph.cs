using System;
using System.Collections.Generic;

namespace Graphs
{
    class Graph<T>
    {
        List<(T, List<(int, float)>)> graph;
        bool Directed;
        bool Weighted;
        public Graph(bool directed, bool weighted, T name)
        {
            Directed = directed;
            Weighted = weighted;
            graph = new List<(T, List<(int, float)>)> { (name, new List<(int, float)>()) };
        }
        int Find(T name)
        {
            for (int i = 0; i < graph.Count; i++) 
            {
                (T query, List<(int, float)> connected) = graph[i];
                if(query.Equals(name))
                {
                    return (i);
                }
            }
            throw new Exception("Point does not exist");
        }
        public void AddPoint(T name)
        {
            graph.Add((name, new List<(int, float)>()));
        }
        public void AddPoint(T name, List<T> connected)
        {
            List<(int, float)> connections = new List<(int, float)>();
            foreach (T connectionName in connected
            {
                connections.Add((Find(connectionName), 0f));
            }
            graph.Add((name, connections));
            if (!Directed)
            {
                foreach (T connectionName in connected)
                {
                    AddConnection(connectionName, name);
                }
            }
        }
        public void AddPoint(T name, List<(T, float)> connected)
        {
            List<(int, float)> connections = new List<(int, float)>();
            foreach ((T connectionName, float weight) in connected)
            {
                connections.Add((Find(connectionName), weight));
            }
            graph.Add((name, connections));
            if (!Directed)
            {
                foreach ((T connectionName, float weight) in connected)
                {
                    AddConnection(connectionName, name, weight);
                }
            }
        }
        public void AddConnection(T name1, T name2)
        {
            if (!Connected(name1, name2))
            {
                (T dump, List<(int, float)> connections) = graph[Find(name1)];
                connections.Add((Find(name2), 0f));
            }
            if (!Directed)
            {
                if (!Connected(name2, name1))
                {
                    AddConnection(name2, name1);
                }
            }
        }
        public void AddConnection(T name1, T name2, float weight)
        {
            if (!Connected(name1, name2))
            {
                (T dump, List<(int, float)> connections) = graph[Find(name1)];
                connections.Add((Find(name2), weight));
            }
            if (!Directed)
            {
                if (!Connected(name2, name1))
                {
                    AddConnection(name2, name1, weight);
                }
            }
        }
        public bool Connected(T name1, T name2)
        {
            (T dump, List<(int, float)> connections) = graph[Find(name1)];
            foreach((int index, float weight) in connections)
            {
                if(index == Find(name2))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
