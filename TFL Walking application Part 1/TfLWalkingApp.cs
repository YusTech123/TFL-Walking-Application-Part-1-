using System;




public class DijkstraShortestPath
{
    private static int GetIndex(string stationName, string[] stationNames)
    {
        for (int i = 0; i < stationNames.Length; i++)
        {
            if (stationNames[i] == stationName)
            {
                return i;
            }
        }
        return -1;
    }

    private static string GetCommonLine(string[] line1, string[] line2)
    {
        foreach (string station in line1)
        {
            if (Array.Exists(line2, element => element == station))
            {
                return station;
            }
        }
        return "";
    }

    private static void PrintPath(int[] parent, int destination, string[] stationNames, string[][] stationLines, int[,] minutesMatrix, int[,] problemsMatrix)
    {
        int[] path = new int[parent.Length];
        int pathIndex = 0;
        int current = destination;

        while (parent[current] != -1)
        {
            path[pathIndex++] = current;
            current = parent[current];
        }
        path[pathIndex] = current;

        int printCount = 1;

        int nextStationIndex = path[pathIndex-1];
        string nextStationName = stationNames[nextStationIndex];
        string commonLine = GetCommonLine(stationLines[current], stationLines[nextStationIndex]);
        string stationName = stationNames[current];

        Console.WriteLine($"({printCount}) Start: {stationName} ({commonLine})");
        printCount++;

        String latestLine = commonLine;
        int minutesCount = 0;
        //Print Start
        for (int i = pathIndex; i >= 0; i--,printCount++)
        {
            int stationIndex = path[i];
            stationName = stationNames[stationIndex];
            
            if (i > 0){
                nextStationIndex = path[i - 1];
                nextStationName = stationNames[nextStationIndex];
                commonLine = GetCommonLine(stationLines[stationIndex], stationLines[nextStationIndex]);
                int minutes = minutesMatrix[stationIndex,nextStationIndex] +  + problemsMatrix[stationIndex,nextStationIndex];
                minutesCount += minutes;
                if (!commonLine.Equals(latestLine)){
                    Console.WriteLine($"({printCount}) Change: {stationName} ({latestLine}) to {stationName} ({commonLine})");
                    latestLine = commonLine;
                    printCount++;
                }

                Console.WriteLine($"({printCount})      {stationName} ({commonLine}) to {nextStationName} ({commonLine})     {minutes} mins");
            }
            else
            {
                Console.WriteLine($"({printCount}) End: {stationName} ({commonLine})");
            }
        }
        Console.WriteLine($"Total Journey Time: {minutesCount} minutes");
    }

    private static int GetClosestVertex(int[] distances, bool[] visited)
    {
        int minDistance = int.MaxValue;
        int minIndex = -1;

        for (int v = 0; v < distances.Length; v++)
        {
            if (!visited[v] && distances[v] <= minDistance)
            {
                minDistance = distances[v];
                minIndex = v;
            }
        }

        return minIndex;
    }

    public static void Dijkstra(int[,] minutesMatrix, int[,] problemsMatrix, string[] stationNames, string[][] stationLines, int source, int destination)
    {
        int numVertices = minutesMatrix.GetLength(0);

        int[] distances = new int[numVertices];
        bool[] visited = new bool[numVertices];
        int[] parent = new int[numVertices];

        for (int i = 0; i < numVertices; i++)
        {
            distances[i] = int.MaxValue;
            visited[i] = false;
            parent[i] = -1;
        }

        distances[source] = 0;

        for (int count = 0; count < numVertices - 1; count++)
        {
            int u = GetClosestVertex(distances, visited);
            visited[u] = true;

            for (int v = 0; v < numVertices; v++)
            {
                if (!visited[v] && minutesMatrix[u, v] != -1 && problemsMatrix[u, v] != -1 && (distances[u] + minutesMatrix[u, v] + problemsMatrix[u, v]) < distances[v])
                {
                    distances[v] = distances[u] + minutesMatrix[u, v] + problemsMatrix[u, v];
                    parent[v] = u;
                }
            }
        }

        Console.WriteLine($"Route: {stationNames[source]} to {stationNames[destination]}");

        if (distances[destination] == int.MaxValue)
        {
            Console.WriteLine("No path exists.");
        }
        else
        {
            PrintPath(parent, destination, stationNames, stationLines, minutesMatrix, problemsMatrix);
        }
    }
    public static void Main()
    {
        // Create the adjacency matrix
        int[,] adjacencyMatrix = new int[,]
        {
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,9,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,10,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,16,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,20,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,16,-1,-1,-1,-1,-1,-1,10,-1,-1,-1,-1,-1,-1,-1,-1,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,10,6,-1,-1,-1,0,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,9,-1,-1,-1,-1,-1,-1,33,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,10,-1,-1,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,10,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,16,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,7,-1,-1,-1,-1,-1,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,4,-1,-1,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,14,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,8,-1,-1,-1,-1,-1,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,12,-1,-1,-1,18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,7,-1,-1,-1,-1,-1,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,9,-1,-1,-1,-1,-1,6,10 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,9,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,10,-1,-1,-1,-1,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,26,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,7,-1,-1,-1,7,-1,-1 },
            { -1,-1,-1,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,15,-1,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,19,-1,-1,21 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,18,-1,-1,-1,-1,-1,-1,-1,18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,8,-1,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,9,-1,-1,-1,-1,-1,-1,10,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,12,-1,-1,-1,-1,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,16,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,12,15,26,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,9,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,7,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,8,-1,-1,-1,-1,-1,-1 },
            { 9,11,-1,-1,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,6,-1,-1,-1,-1,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,19,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,10,-1,-1,4,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,0,-1,-1,-1,-1,-1,5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,10,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,9,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,6,-1,-1,-1,-1,-1,-1,-1,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,13,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,20,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,12,-1,-1,15,-1,-1,-1,-1,-1,-1,-1,9,-1,-1,-1,18,-1,-1 },
            { -1,-1,-1,-1,-1,-1,17,-1,-1,-1,-1,-1,-1,-1,-1,10,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,8,-1,-1,-1,-1,-1,-1,-1,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,18,12,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,10,-1,-1,-1,-1,-1,-1,-1,-1,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,9,-1,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,17,-1,-1,-1,-1,-1,-1,-1,13,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,8,-1,-1,-1,-1,-1,-1,-1,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,17,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,19,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,8,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,11,-1,-1,11 },
            { -1,-1,-1,-1,9,-1,-1,-1,-1,-1,-1,14,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,7,-1,-1,-1,10,-1,-1,-1,-1,-1,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { 9,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,10,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,19,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,12,-1,-1,-1,13,-1,-1,11,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,9,-1,-1,-1,7,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,18,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1 },
            { -1,-1,-1,-1,33,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,9,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,8,-1,-1,-1,-1,-1,-1,-1,-1,-1,17 },
            { -1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,10,-1,-1,-1,-1,-1,-1,21,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,11,-1,-1,-1,-1,-1,-1,-1,17,-1 }
        };

        int[,] problemsMatrix = new int[63,63];

        for (int x = 0; x < 63; x++)
        {
            for (int y = 0; y < 63; y++){
                problemsMatrix[x,y] = 0;
            }
        }


        string[] stationNames = new string[]
        {
            "Aldgate",
            "Aldgate East",
            "Angel",
            "Baker Street",
            "Bank",
            "Barbican",
            "Bayswater",
            "Blackfriars",
            "Bond Street",
            "Borough",
            "Cannon Street",
            "Chancery Lane",
            "Charing Cross",
            "Covent Garden",
            "Earl's Court",
            "Edgware Road CDHC",
            "Edgware Road Bakerloo",
            "Elephant & Castle",
            "Embankment",
            "Euston",
            "Euston Square",
            "Farringdon",
            "Gloucester Road",
            "Goodge Street",
            "Great Portland Street",
            "Green Park",
            "High Street Kensington",
            "Holborn",
            "Hyde Park Corner",
            "King's Cross St Pancras",
            "Knightsbridge",
            "Lambeth North",
            "Lancaster Gate",
            "Leicester Square",
            "Liverpool Street",
            "London Bridge",
            "Mansion House",
            "Marble Arch",
            "Marylebone",
            "Monument",
            "Moorgate",
            "Notting Hill Gate",
            "Old Street",
            "Oxford Circus",
            "Paddington",
            "Piccadilly Circus",
            "Pimlico",
            "Queensway",
            "Regent's Park",
            "Russell Square",
            "Sloane Square",
            "South Kensington",
            "Southwark",
            "St James's Park",
            "St Paul's",
            "Temple",
            "Tottenham Court Road",
            "Tower Hill",
            "Vauxhall",
            "Victoria",
            "Warren Street",
            "Waterloo",
            "Westminster"
        };

        string[][] stationLines = new string[][]
        {
            new string[] { "Circle", "Metropolitan" }, // Aldgate
            new string[] { "District", "Hammersmith & City" }, // Aldgate East
            new string[] { "Northern" }, // Angel
            new string[] { "Bakerloo", "Circle", "Hammersmith & City", "Jubilee", "Metropolitan" }, // Baker Street
            new string[] { "Central", "Northern", "Waterloo & City" }, // Bank
            new string[] { "Circle", "Hammersmith & City", "Metropolitan" }, // Barbican
            new string[] { "Circle", "District" }, // Bayswater
            new string[] { "Circle", "District" }, // Blackfriars
            new string[] { "Central", "Jubilee" }, // Bond Street
            new string[] { "Northern" }, // Borough
            new string[] { "Circle", "District" }, // Cannon Street
            new string[] { "Central" }, // Chancery Lane
            new string[] { "Bakerloo", "Northern" }, // Charing Cross
            new string[] { "Piccadilly" }, // Covent Garden
            new string[] { "District", "Piccadilly" }, // Earl's Court
            new string[] { "Circle", "District", "Hammersmith & City" }, // Edgware Road
            new string[] { "Bakerloo" }, // Edgware Road
            new string[] { "Bakerloo", "Northern" }, // Elephant & Castle
            new string[] { "Bakerloo", "Circle", "District", "Northern" }, // Embankment
            new string[] { "Northern", "Victoria" }, // Euston
            new string[] { "Circle", "Metropolitan", "Hammersmith & City" }, // Euston Square
            new string[] { "Circle", "Hammersmith & City", "Metropolitan" }, // Farringdon
            new string[] { "Circle", "District", "Piccadilly" }, // Gloucester Road
            new string[] { "Northern" }, // Goodge Street
            new string[] { "Circle", "Hammersmith & City", "Metropolitan" }, // Great Portland Street
            new string[] { "Jubilee", "Piccadilly", "Victoria" }, // Green Park
            new string[] { "Circle", "District" }, // High Street Kensington
            new string[] { "Central", "Piccadilly" }, // Holborn
            new string[] { "Piccadilly" }, // Hyde Park Corner
            new string[] { "Circle", "Hammersmith & City", "Metropolitan", "Northern", "Piccadilly", "Victoria" }, // King's Cross St Pancras
            new string[] { "Piccadilly" }, // Knightsbridge
            new string[] { "Bakerloo" }, // Lambeth North
            new string[] { "Central" }, // Lancaster Gate
            new string[] { "Northern", "Piccadilly" }, // Leicester Square
            new string[] { "Central", "Circle", "Hammersmith & City", "Metropolitan" }, // Liverpool Street
            new string[] { "Jubilee", "Northern" }, // London Bridge
            new string[] { "Circle", "District" }, // Mansion House
            new string[] { "Central" }, // Marble Arch
            new string[] { "Bakerloo" }, // Marylebone
            new string[] { "Circle", "District" }, // Monument
            new string[] { "Circle", "Hammersmith & City", "Metropolitan", "Northern" }, // Moorgate
            new string[] { "Central", "Circle", "District" }, // Notting Hill Gate
            new string[] { "Northern" }, // Old Street
            new string[] { "Bakerloo", "Central", "Victoria" }, // Oxford Circus
            new string[] { "Bakerloo", "Circle", "District", "Hammersmith & City" }, // Paddington
            new string[] { "Bakerloo", "Piccadilly" }, // Piccadilly Circus
            new string[] { "Victoria" }, // Pimlico
            new string[] { "Central" }, // Queensway
            new string[] { "Bakerloo" }, // Regent's Park
            new string[] { "Piccadilly" }, // Russell Square
            new string[] { "Circle", "District" }, // Sloane Square
            new string[] { "Circle", "District", "Piccadilly" }, // South Kensington
            new string[] { "Jubilee" }, // Southwark
            new string[] { "Circle", "District" }, // St James's Park
            new string[] { "Central" }, // St Paul's
            new string[] { "Circle", "District" }, // Temple
            new string[] { "Central", "Northern" }, // Tottenham Court Road
            new string[] { "Circle", "District" }, // Tower Hill
            new string[] { "Victoria" }, // Vauxhall
            new string[] { "Circle", "District", "Victoria" }, // Victoria
            new string[] { "Northern", "Victoria" }, // Warren Street
            new string[] { "Bakerloo", "Jubilee", "Northern", "Waterloo & City" }, // Waterloo
            new string[] { "Circle", "District", "Jubilee" } // Westminster
        };

        int choice = 0;
        Console.WriteLine("Welcome to the TfL tube walking route application!");

        while (choice != 3) {
            Console.WriteLine("Are you a customer or TfL Manager? Please enter 1 or 2:");
            Console.WriteLine("1. TfL Manager");
            Console.WriteLine("2. Customer");
            Console.WriteLine("3. Exit");
            choice = Convert.ToInt32(Console.ReadLine());
            if (choice == 1){
                while (choice != 7){
                    Console.WriteLine("Please select one of the options below:");
                    Console.WriteLine("1. List of impossible walking routes");
                    Console.WriteLine("2. List of delayed walking routes");
                    Console.WriteLine("3. Add walking time delays");
                    Console.WriteLine("4. Remove walking time delays");
                    Console.WriteLine("5. Set route is now impossible");
                    Console.WriteLine("6. Set route is now possible");
                    Console.WriteLine("7. Go Back");
                    choice = Convert.ToInt32(Console.ReadLine());
                    if (choice == 1){
                        // Closed routes:
                        // Northern Line: London Bridge - Monument : bridge closed
                        Console.WriteLine("Closed routes:");
                        for (int x = 0; x < 63; x++)
                        {
                            for (int y = 0; y < 63; y++){
                                if (problemsMatrix[x,y] == -1){
                                    String station1 = stationNames[x];
                                    String station2 = stationNames[y];
                                    String commonLine = GetCommonLine(stationLines[x], stationLines[y]);
                                    Console.WriteLine($"{commonLine} Line: {station1} - {station2} : route closed");
                                }
                            }
                        }
                        

                    } else if (choice == 2){
                        //Delayed routes:
                        //Victoria Line:  Oxford Circus - Warren Street : 18 min now 23 min
                        Console.WriteLine("Delayed routes:");
                        for (int x = 0; x < 63; x++)
                        {
                            for (int y = 0; y < 63; y++){
                                if (problemsMatrix[x,y] > 0){
                                    String station1 = stationNames[x];
                                    String station2 = stationNames[y];
                                    String commonLine = GetCommonLine(stationLines[x], stationLines[y]);
                                    int oldTime = adjacencyMatrix[x,y];
                                    int newTime = oldTime + problemsMatrix[x,y];
                                    Console.WriteLine($"{commonLine} Line: {station1} - {station2} : {oldTime} min now {newTime} min");
                                }
                            }
                        }

                    } else if (choice == 3){
                        Console.WriteLine("Please enter starting station name:");
                        int source = GetIndex(Console.ReadLine(), stationNames);
                        if (source == -1){
                            Console.WriteLine("INVALID INPUT: Station not found!");
                            continue;
                        }
                        Console.WriteLine("Please enter destination station name:");
                        int destination = GetIndex(Console.ReadLine(), stationNames);
                        if (destination == -1){
                            Console.WriteLine("INVALID INPUT: Station not found!");
                            continue;
                        }
                        Console.WriteLine("Please enter delay amount in minutes:");
                        int delay = Convert.ToInt32(Console.ReadLine());
                        problemsMatrix[source, destination] = delay;
                        problemsMatrix[destination, source] = delay;
                        Console.WriteLine("Delay added!");
                        Console.WriteLine("");
                    } else if (choice == 4){
                        Console.WriteLine("Please enter starting station name:");
                        int source = GetIndex(Console.ReadLine(), stationNames);
                        if (source == -1){
                            Console.WriteLine("INVALID INPUT: Station not found!");
                            continue;
                        }
                        Console.WriteLine("Please enter destination station name:");
                        int destination = GetIndex(Console.ReadLine(), stationNames);
                        if (destination == -1){
                            Console.WriteLine("INVALID INPUT: Station not found!");
                            continue;
                        }
                        problemsMatrix[source, destination] = 0;
                        problemsMatrix[destination, source] = 0;
                        Console.WriteLine("Delay removed!");
                        Console.WriteLine("");

                    } else if (choice == 5){
                        Console.WriteLine("Please enter starting station name:");
                        int source = GetIndex(Console.ReadLine(), stationNames);
                        if (source == -1){
                            Console.WriteLine("INVALID INPUT: Station not found!");
                            continue;
                        }
                        Console.WriteLine("Please enter destination station name:");
                        int destination = GetIndex(Console.ReadLine(), stationNames);
                        if (destination == -1){
                            Console.WriteLine("INVALID INPUT: Station not found!");
                            continue;
                        }
                        problemsMatrix[source, destination] = -1;
                        problemsMatrix[destination, source] = -1;
                        Console.WriteLine("Route Closed!");
                        Console.WriteLine("");



                    } else if (choice == 6){
                        Console.WriteLine("Please enter starting station name:");
                        int source = GetIndex(Console.ReadLine(), stationNames);
                        if (source == -1){
                            Console.WriteLine("INVALID INPUT: Station not found!");
                            continue;
                        }
                        Console.WriteLine("Please enter destination station name:");
                        int destination = GetIndex(Console.ReadLine(), stationNames);
                        if (destination == -1){
                            Console.WriteLine("INVALID INPUT: Station not found!");
                            continue;
                        }
                        problemsMatrix[source, destination] = 0;
                        problemsMatrix[destination, source] = 0;
                        Console.WriteLine("Route Opened!");
                        Console.WriteLine("");

                    } else if (choice != 7){
                        Console.WriteLine("INVALID INPUT: Please enter 1, 2 or 3.");
                    }
                }
            } else if (choice == 2){
                while (choice != 3){
                    Console.WriteLine("Please select one of the options below:");
                    Console.WriteLine("1. Find the fastest walking route");
                    Console.WriteLine("2. Display Tube Information about a station");
                    Console.WriteLine("3. Go Back");
                    choice = Convert.ToInt32(Console.ReadLine());
                    if (choice == 1){
                        Console.WriteLine("Please enter starting station name:");
                        int source = GetIndex(Console.ReadLine(), stationNames);
                        if (source == -1){
                            Console.WriteLine("INVALID INPUT: Station not found!");
                            continue;
                        }
                        Console.WriteLine("Please enter destination station name:");
                        int destination = GetIndex(Console.ReadLine(), stationNames);
                        if (destination == -1){
                            Console.WriteLine("INVALID INPUT: Station not found!");
                            continue;
                        }
                        Console.WriteLine("");
                        Dijkstra(adjacencyMatrix, problemsMatrix, stationNames, stationLines, source, destination);
                        Console.WriteLine("");
                    } else if (choice == 2){
                        Console.WriteLine("Please enter station name:");
                        int stationIndex = GetIndex(Console.ReadLine(), stationNames);
                        if (stationIndex == -1){
                            Console.WriteLine("INVALID INPUT: Station not found!");
                            continue;
                        }
                        string stationName = stationNames[stationIndex];
                        string[] currentStationLines = stationLines[stationIndex];
                        Console.WriteLine("");
                        Console.WriteLine($"Station Name: {stationName}");
                        string joinedLines = string.Join(", ", currentStationLines);
                        Console.WriteLine($"Station Lines: {joinedLines}");
                        Console.WriteLine("");
                        
                    } else if (choice != 3){
                        Console.WriteLine("INVALID INPUT: Please enter 1, 2 or 3.");
                    }
                }
                choice = 0;
            } else if (choice != 3) {
                Console.WriteLine("INVALID INPUT: Please enter 1, 2 or 3.");
            }
        }
    }

}
