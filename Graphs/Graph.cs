using System;
using System.Collections.Generic;

namespace Graphs
{
    class Graph<T>
    {
        List<(T, List<(T, float)>)> graph;
        bool Directed;
        bool Weighted;
        public Graph(bool directed, bool weighted, T name)
        {
            Directed = directed;
            Weighted = weighted;
            graph = new List<(T, List<(T, float)>)> { (name, new List<(T, float)>()) };
        }
        int Find(T name)
        {
            for (int i = 0; i < graph.Count; i++) 
            {
                (T query, List<(T, float)> connected) = graph[i];
                if(query.Equals(name))
                {
                    return (i);
                }
            }
            throw new Exception("Point does not exist");
        }
        public void AddPoint(T name)
        {
            graph.Add((name, new List<(T, float)>()));
        }
        public void AddPoint(T name, List<T> connected)
        {
            List<(T, float)> connections = new List<(T, float)>();
            foreach (T connectionName in connected)
            {
                connections.Add((connectionName, 0f));
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
            List<(T, float)> connections = new List<(T, float)>();
            foreach ((T connectionName, float weight) in connected)
            {
                connections.Add((connectionName, weight));
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
                (T dump, List<(T, float)> connections) = graph[Find(name1)];
                connections.Add((name2, 0f));
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
                (T dump, List<(T, float)> connections) = graph[Find(name1)];
                connections.Add((name2, weight));
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
            (T dump, List<(T, float)> connections) = graph[Find(name1)];
            foreach((T name, float weight) in connections)
            {
                if(name.Equals(name2))
                {
                    return true;
                }
            }
            return false;
        }
        public void RemoveConnection(T name1, T name2)
        {
            if(Connected(name1, name2))
            {
                (T dump, List<(T, float)> connections) = graph[Find(name1)];
                foreach((T name, float weight) in connections)
                {
                    if(name.Equals(name2))
                    {
                        connections.Remove((name2, weight));
                        break;
                    }
                }
            }
            if (!Directed)
            {
                if (Connected(name2, name1))
                {
                    RemoveConnection(name2, name1);
                }
            }
            
        }
        public void Remove(T name)
        {
            foreach(T name2 in Neighbours(name))
            {
                RemoveConnection(name, name2);
            }
            graph.Remove(graph[Find(name)]);
        }
        public List<T> Neighbours(T name)
        {
            List<T> result = new List<T>();
            (T name2, List<(T, float)> connections) = graph[Find(name)];
            foreach((T name3, float w) in connections)
            {
                (T a, List<(T, float)> b) = graph[Find(name3)];
                result.Add(a);
            }
            return result;
        }
    }
}
