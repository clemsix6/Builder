using System.Linq;
using UnityEngine;

public class Api
{
    private Actor actor;
    private Pawn pawn;


    public Api(Actor actor, Pawn pawn)
    {
        this.actor = actor;
        this.pawn = pawn;
    }


    public Vector2 GetPosition()
    {
        return actor.position;
    }


    public bool Go(Vector2 position)
    {
        return pawn.Go(position);
    }


    public string[] GetObjectsAt(Vector2 position)
    {
        return actor.environment.GetActorsAt(position).Select(x => x.type).ToArray();
    }
}
