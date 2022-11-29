using System.IO;
using UnityEngine;
using Microsoft.ClearScript.V8;

public class Robot : MonoBehaviour
{
    [SerializeField] private Actor actor;
    [SerializeField] private Pawn pawn;
    
    private Api api;
    private V8ScriptEngine engine;
    private float lastExecutionTime;
    private string program;

    
    private void Start()
    {
        api = new Api(actor, pawn);
    }


    private void RefreshV8Engine()
    {
        engine = new V8ScriptEngine();
        engine.AddHostType("Debug", typeof(Debug));
        engine.AddHostType("Vector2", typeof(Vector2));
        engine.AddHostObject("Api", api);
        engine.Execute(program);
    }


    private void Update()
    {
        if (Time.time - lastExecutionTime > 10)
        {
            lastExecutionTime = Time.time;
            ExecuteScript();
        }
    }


    private void ExecuteScript()
    {
        var liveCode = File.ReadAllText(Environment.robotScript);
        if (engine == null || program != liveCode)
        {
            program = liveCode;
            RefreshV8Engine();
            return;
        }
        
        engine.Invoke("update");
        engine.CollectGarbage(true);
    }
}
