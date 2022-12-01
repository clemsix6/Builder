using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Pathfinding
{
    private const int                       maxDistance = 10000;
    private       Environment               environment;
    private       Vector2                   start;
    private       Vector2                   target;
    private       Dictionary<Vector2, Node> nodes;


    public Pathfinding(Environment environment, Vector2 start, Vector2 target)
    {
        this.environment = environment;
        this.start       = start;
        this.target      = target;
        this.nodes       = new Dictionary<Vector2, Node>();
    }


    public List<Vector2> GetPath()
    {
        var path = new List<Vector2> { target };
        CreateNode(start);
        TakeDirection(start, 0);
        SmoothPath(start, 0);

        for (var i = 0; path.First() != start; i++)
        {
            if (i >= maxDistance)
                return new List<Vector2>();
            if (!Walk(path))
                break;
        }

        return path;
    }


    private void SmoothPath(Vector2 point, int distance)
    {
        if (!nodes.TryGetValue(point, out var node) || node.distance != -1)
            return;
        nodes[point].distance = distance;

        foreach (var direction in new[] { Vector2.up, Vector2.down, Vector2.right, Vector2.left })
        {
            var pos = point          + direction;
            SmoothPath(pos, distance + 1);
        }
    }


    private void TakeDirection(Vector2 point, int distance)
    {
        var directions = new List<(Vector2, float)>();
        foreach (var direction in new[] { Vector2.up, Vector2.down, Vector2.right, Vector2.left })
        {
            var pos = point + direction;
            if (environment.GetWalkSpeed(pos) <= 0)
                continue;
            var score = GetPointScore(pos, distance);
            if (score == null)
                continue;
            directions.Add((pos, score.Value));
        }

        foreach (var pos in directions.OrderByDescending(x => x.Item2))
        {
            if (nodes.ContainsKey(target))
                return;
            CreateNode(pos.Item1);
            if (distance < maxDistance)
                TakeDirection(pos.Item1, distance + 1);
        }
    }


    private void CreateNode(Vector2 point)
    {
        nodes.Add(point, new Node());
    }


    private float? GetPointScore(Vector2 point, int distance)
    {
        if (nodes.ContainsKey(point))
            return null;
        var score = distance - Vector2.Distance(point, target);
        return score;
    }


    private bool Walk(List<Vector2> path)
    {
        var      minDistance = float.MaxValue;
        Vector2? step        = null;

        foreach (var direction in new[] { Vector2.up, Vector2.down, Vector2.right, Vector2.left })
        {
            var pos = path.First() + direction;
            if (!nodes.TryGetValue(pos, out var value) ||
                value.distance >= minDistance)
                continue;
            minDistance = value.distance;
            step        = pos;
        }

        if (step == null)
            return false;
        path.Insert(0, step.Value);
        return true;
    }


    class Node
    {
        public int distance;


        public Node()
        {
            this.distance = -1;
        }
    }
}