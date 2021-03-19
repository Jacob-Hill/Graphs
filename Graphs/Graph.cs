using System;
using System.Collections.Generic;

namespace Graphs
{
    class Graph<T>
    {
        List<(T, List<(T, float)>)> graph;
        List<(T, List<(T, float)>)> unweightedGraph;
        bool Directed;
        bool Weighted;
        public Graph(bool directed, bool weighted, T name)
        {
            Directed = directed;
            Weighted = weighted;
            graph = new List<(T, List<(T, float)>)> { (name, new List<(T, float)>()) };
            unweightedGraph = new List<(T, List<(T, float)>)> { (name, new List<(T, float)>()) };
        }
        int Find(T name)
        {
            for (int i = 0; i < graph.Count; i++) 
            {
                (T query, _) = graph[i];
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
            unweightedGraph.Add((name, new List<(T, float)>()));
        }
        public void AddPoint(T name, List<T> connected)
        {
            List<(T, float)> connections = new List<(T, float)>();
            foreach (T connectionName in connected)
            {
                connections.Add((connectionName, 1f));
            }
            graph.Add((name, connections));
            unweightedGraph.Add((name, connections));
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
            List<(T, float)> unweightedConnections = new List<(T, float)>();
            foreach ((T connectionName, float weight) in connected)
            {
                connections.Add((connectionName, weight));
                unweightedConnections.Add((connectionName, 1f));
            }
            graph.Add((name, connections));
            unweightedGraph.Add((name, unweightedConnections));
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
                (_, List<(T, float)> connections) = graph[Find(name1)];
                connections.Add((name2, 1f));
                (_, List<(T, float)> unweightedConnections) = unweightedGraph[Find(name1)];
                unweightedConnections.Add((name2, 1f));
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
                (_, List<(T, float)> connections) = graph[Find(name1)];
                connections.Add((name2, weight));
                (_, List<(T, float)> unweightedConnections) = unweightedGraph[Find(name1)];
                unweightedConnections.Add((name2, 1f));
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
            (_, List<(T, float)> connections) = graph[Find(name1)];
            foreach ((T name, _) in connections)
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
                (_, List<(T, float)> connections) = graph[Find(name1)];
                foreach((T name, float weight) in connections)
                {
                    if(name.Equals(name2))
                    {
                        connections.Remove((name2, weight));
                        break;
                    }
                }
                (_, List<(T, float)> unweightedConnections) = unweightedGraph[Find(name1)];
                foreach ((T name, float weight) in connections)
                {
                    if (name.Equals(name2))
                    {
                        unweightedConnections.Remove((name2, weight));
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
            unweightedGraph.Remove(unweightedGraph[Find(name)]);
        }
        public List<T> Neighbours(T name)
        {
            List<T> result = new List<T>();
            (_, List<(T, float)> connections) = graph[Find(name)];
            foreach ((T name3, _) in connections)
            {
                (T a, _) = graph[Find(name3)];
                result.Add(a);
            }
            return result;
        }
        (List<T>, float) Dijkstra(T name1, T name2, List<(T, List<(T, float)>)> Graph)
        {
            List<(T, float)> distances = new List<(T, float)>();
            List<T> visited = new List<T>();
            List<T> unvisited = new List<T>();
            List<(T, T)> pairs = new List<(T, T)>();
            foreach ((T name, _) in Graph)
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
            while (visited.Count < Graph.Count)
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
                (_, List<(T, float)> connections) = Graph[Find(current)];
                foreach((T name, float distance) in connections)
                {
                    if (!visited.Contains(name))
                    {
                        float distanceTo = 0;
                        float newDistance = 0;
                        int index = 0;
                        foreach ((T check, float distance2) in distances)
                        {
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
                                    int index2 = 0;
                                    bool exists = false;
                                    foreach((_, T current2) in pairs)
                                    {
                                        if (name.Equals(current2))
                                        {
                                            pairs[index] = (current, name);
                                            exists = true;
                                            break;
                                        }
                                        index2++;
                                    }
                                    if (!exists)
                                    {
                                        pairs.Add((current, name));
                                    }
                                    break;
                                }
                            }
                            index++;
                        }
                    }
                }
            }
            float result = 0;
            List<T> route = RouteFromPairs(name1, name2, pairs);
            foreach ((T name, float distance) in distances)
            {
                if (name.Equals(name2))
                {
                    result = distance;
                }
            }
            return (route,result);
        }
        List<T> RouteFromPairs(T start, T end, List<(T,T)> pairs)
        {
            List<T> result = new List<T>();
            for (int i = 0; i < pairs.Count; i++)
            {
                (T previous, T current) = pairs[i];
                if (current.Equals(end))
                {
                    if (previous.Equals(start))
                    {
                        return new List<T> { start };
                    }
                    else
                    {
                        result = RouteFromPairs(start, previous, pairs);
                        result.Add(previous);
                    }
                }
            }
            result.Add(end);
            return result;
        }
        public float Distance(T name1, T name2)
        {
            (_, float distance) = Dijkstra(name1, name2, graph);
            return distance;
        }
        public List<T> ShortestRoute(T name1, T name2)
        {
            (List<T> route, _) = Dijkstra(name1, name2, graph);
            return route;
        }
        public List<T> BreadthFirst(T root)
        {
            List<(T, float)> points = new List<(T, float)> { (root, 0) };
            foreach ((T name, _) in graph)
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
                    (_, float distance) = Dijkstra(root, name, unweightedGraph);
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
