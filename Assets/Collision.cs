using UnityEngine;

public class Collision : MonoBehaviour
{
    public Vector2 position;
    public Actor   parent;
    public Chunk   chunk;
    public float   walkSpeed;


    private void Start()
    {
        parent.environment.collisions.Add(this);
    }


    public Vector2 GetWorldPosition()
    {
        return position + parent.position;
    }


    public void UpdateChunk()
    {
        var pos      = position + (Vector2)parent.transform.position;
        var newChunk = parent.environment.GetChunk(pos);
        if (newChunk == null)
            newChunk = parent.environment.GenerateChunk(pos);
        if (chunk != null)
            chunk.RemoveCollision(this);
        chunk = newChunk;
        chunk.AddCollision(this);
    }
}