using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

public class Pawn : MonoBehaviour
{
    [SerializeField] private Actor actor;

    private          Vector2       target = Vector2.zero;
    private readonly List<Vector2> path   = new();

    private Vector2 start;
    private float   startTime;
    private Vector2 stop;
    private float   progression = 1;


    private void Start()
    {
        stop = transform.position;
    }


    public bool MoveTo(Vector2 position)
    {
        if (target == position)
            return false;
        target = position;
        var currentTime     = Time.time;
        var currentPosition = stop;

        var sw = new Stopwatch();
        sw.Start();
        lock (path)
            path.Clear();

        lock (actor)
        {
            var pathfinding = new Pathfinding(actor.environment, currentPosition, position);
            var newPath     = pathfinding.GetPath();
            if (!newPath.Any() || newPath.Last() != position)
                return false;

            lock (path)
            {
                sw.Stop();
                start     = currentPosition;
                startTime = currentTime + sw.ElapsedMilliseconds / 1000f;
                path.AddRange(newPath);
                stop = path.First();
                path.RemoveAt(0);
            }
        }

        return true;
    }


    private void Update()
    {
        if (startTime != 0)
            Move();
    }


    private void Move()
    {
        progression = (Time.time - startTime) / 0.1f;

        if (progression >= 1)
        {
            StepUp();
            return;
        }

        var pos = Vector2.Lerp(start, stop, progression);
        transform.position = new Vector3(pos.x, pos.y, transform.position.z);
    }


    private void StepUp()
    {
        transform.position = stop;

        lock (path)
        {
            if (path.Count > 0)
            {
                startTime += 0.1f;
                start     =  stop;
                stop      =  path.First();
                path.RemoveAt(0);
            }
            else
            {
                startTime = 0;
            }
        }
    }
}