using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    public string type;
    public Environment environment;
    public List<Collision> collisions;

    public Vector2 position;
    public Vector3 rotation;
    public Vector2 scale;

    
    private void Start()
    {
        if (environment == null)
            environment = GameObject.Find("Environment").GetComponent<Environment>();
        environment.actors.Add(this);
        UpdateCollisions();
        UpdateTransformData();
    }


    private void UpdateCollisions()
    {
        foreach (var collision in collisions)
            collision.UpdateChunk();
    }


    private void UpdateTransformData()
    {
        position = transform.position;
        rotation = transform.eulerAngles;
        scale = transform.localScale;
    }


    public void Move(Vector2 pos)
    {
        transform.position = new Vector3(
            pos.x,
            pos.y,
            transform.position.z
        );
        position = pos;
    }
}