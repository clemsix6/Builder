using System;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    private static long globalId = 0L; 
    
    [SerializeField] public string type;
    [SerializeField] public long   id;
    [SerializeField] public string description;
    
    [NonSerialized] public Environment     environment;
    [NonSerialized] public List<Collision> collisions;
    [NonSerialized] public Vector2         position;
    [NonSerialized] public Vector3         rotation;
    [NonSerialized] public Vector2         scale;


    private void Start()
    {
        id = globalId++;
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
        scale    = transform.localScale;
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