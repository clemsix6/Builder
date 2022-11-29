using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class Chunk : MonoBehaviour
{
    public const int chunkSize = 64;
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TileBase[] grassTiles;
    [SerializeField] private TileBase[] flowerTiles;
    [SerializeField] private GameObject[] rocks;
    [SerializeField] private GameObject[] trees;

    public Environment environment;
    public List<Collision> collisions = new();
    public Vector2 position;

    void Start()
    {
        position = transform.position;
        GenerateTileMap();
        GenerateProps();
    }


    public void AddCollision(Collision collision)
    {
        lock (collisions)
            collisions.Add(collision);
    }


    public void RemoveCollision(Collision collision)
    {
        lock (collisions)
            collisions.Remove(collision);
    }


    private void GenerateTileMap()
    {
        for (var x = 0; x < chunkSize; x++)
        {
            for (var y = 0; y < chunkSize; y++)
            {
                var rndTile = Random.value < 0.5
                    ? grassTiles[0]
                    : Random.value < 0.8
                        ? grassTiles[Random.Range(0, grassTiles.Length)]
                        : flowerTiles[Random.Range(0, flowerTiles.Length)];
                tilemap.SetTile(new Vector3Int(x, y, 0), rndTile);
            }
        }
    }


    private void GenerateProps()
    {
        for (var x = 0; x < chunkSize; x++)
        {
            for (var y = 0; y < chunkSize; y++)
            {
                if (PlaceRock(x, y))
                    continue;
                PlaceTree(x, y);
            }
        }
    }


    private bool PlaceRock(int x, int y)
    {
        var position = (Vector2)transform.position;

        if (Random.value < 0.02f)
        {
            var rock = rocks[Random.Range(0, rocks.Length)];
            rock = Instantiate(
                rock,
                new Vector3(position.x + x, position.y + y, position.y + y),
                Quaternion.identity
            );
            rock.transform.parent = environment.transform;
            return true;
        }

        return false;
    }


    private bool PlaceTree(int x, int y)
    {
        var position = (Vector2)transform.position;

        if (Random.value < 0.005f)
        {
            var tree = trees[Random.Range(0, trees.Length)];
            tree = Instantiate(
                tree,
                new Vector3(position.x + x, position.y + y, position.y + y),
                Quaternion.identity
            );
            tree.transform.parent = environment.transform;
            return true;
        }

        return false;
    }
}
