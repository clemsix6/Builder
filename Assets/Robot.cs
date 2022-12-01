using UnityEngine;

public class Robot : MonoBehaviour
{
    [SerializeField] public Actor actor;
    [SerializeField] public Pawn  pawn;

    private float          lastExecutionTime;
    private string         program;


    private void Start()
    {
        actor.environment.robots.Add(this);
    }


    private void Update()
    {
        if (Time.time - lastExecutionTime > 10)
        {
            lastExecutionTime = Time.time;
        }
    }
}