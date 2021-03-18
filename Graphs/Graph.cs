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
                connections.Add((connectionName, 1f));
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
                connections.Add((name2, 1f));
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
        //public (List<T>, float) Distance(T name1, T name2)
        public float Distance(T name1, T name2)
        {
            List<(T, float)> distances = new List<(T, float)>();
            List<T> visited = new List<T>();
            List<T> unvisited = new List<T>();
            foreach ((T name, List<(T, float)> connections) in graph)
            { 
                if(name.Equals(name1))
                {
                    distances.Add((name, 0));
                    unvisited.Add(name);
                }
                else
                {
                    distances.Add((name, float.PositiveInfinity));
                    unvisited.Add(name);
                }
            }
            while (visited.Count < graph.Count)
            {
                float minDistance = float.PositiveInfinity;
                T current = name1;
                for (int i = 0; i < distances.Count; i++) 
                {
                    (T name, float distance) = distances[i];
                    if (distance < minDistance && unvisited.Contains(name))
                    {
                        minDistance = distance;
                        current = name;
                    }
                }
                unvisited.Remove(current);
                visited.Add(current);
                (T dump, List<(T, float)> connections) = graph[Find(current)];
                foreach((T name, float distance) in connections)
                {
                    if (!visited.Contains(name))
                    {
                        float distanceTo = 0;
                        float newDistance = 0;
                        int index = 0;
                        foreach ((T check, float distance2) in distances)
                        {
                            index++;
                            if (check.Equals(current))
                            {
                                distanceTo = distance2;
                            }
                            else if (check.Equals(name))
                            {
                                newDistance = distanceTo + distance;
                                if (newDistance < distance2)
                                {
                                    distances[index] = (name, newDistance);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            float result = 0;
            foreach ((T name, float distance) in distances)
            {
                if (name.Equals(name2))
                {
                    result = distance;
                }
            }
            return result;
        }
        public List<T> BreadthFirst(T root)
        {
            List<(T, float)> points = new List<(T, float)> { (root, 0) };
            foreach((T name, List<(T, float)> connections) in graph)
            {
                bool exists = false;
                for(int i = 0; i<graph.Count; i++)
                {
                    if (points.Contains((name, i)))
                    {
                        exists = true;
                    }
                }
                if (!exists)
                {
                    //(List<T> route, float distance) = Distance(root, name);
                    float distance = Distance(root, name);
                    points.Add((name, distance));
                }
            }
            List<T> result = new List<T>();
            foreach ((T name, float distance) in points)
            {
                for(int i = 0; i<graph.Count; i++)
                {
                    if(distance == i)
                    {
                        result.Add(name);
                    }
                }
            }
            return result;
        }
    }
}
