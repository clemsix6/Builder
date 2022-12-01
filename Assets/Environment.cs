using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Environment : MonoBehaviour
{
    [SerializeField] private Chunk               chunkPrefab;

    public List<Chunk>     chunks;
    public List<Actor>     actors;
    public List<Robot>     robots;
    public List<Collision> collisions;


    private IEnumerator Start()
    {
        for (var x = 0; x < 10; x++)
        {
            for (var y = 0; y < 10; y++)
            {
                GenerateChunk(new Vector2(x * Chunk.chunkSize, y * Chunk.chunkSize));
                yield return new WaitForEndOfFrame();
            }
        }
    }


    public Chunk GetChunk(Vector2 position)
    {
        var x = (int)(position.x / Chunk.chunkSize) * Chunk.chunkSize;
        var y = (int)(position.y / Chunk.chunkSize) * Chunk.chunkSize;

        lock (chunks)
            return chunks.Find(c => c.position.x == x && c.position.y == y);
    }


    public Chunk GenerateChunk(Vector2 position)
    {
        var x = (int)(position.x / Chunk.chunkSize);
        var y = (int)(position.y / Chunk.chunkSize);
        position.x = x * Chunk.chunkSize;
        position.y = y * Chunk.chunkSize;

        var chunkObject = Instantiate(chunkPrefab, position, Quaternion.identity);
        chunkObject.name = $"chunk_{x}_{y}";
        var chunk = chunkObject.GetComponent<Chunk>();
        chunk.environment = this;
        lock (chunks) chunks.Add(chunk);
        return chunk;
    }


    public float GetWalkSpeed(Vector2 point)
    {
        var chunk     = GetChunk(point);
        var collision = chunk.collisions.Find(x => x.GetWorldPosition() == point);
        if (collision != null)
            return collision.walkSpeed;
        return 1;
    }


    public IEnumerable<Actor> GetActorsAt(Vector2 point)
    {
        var chunk = GetChunk(point);
        if (chunk == null)
            return Array.Empty<Actor>();
        var result = chunk.collisions.FindAll(x => x.GetWorldPosition() == point);
        return result.Select(x => x.parent);
    }
}