using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CommandExecutor : MonoBehaviour
{
    [SerializeField] private Environment environment;
    
    private string CleanSpaces(string line)
    {
        var result = new List<char>();
        foreach (var c in line.Where(c => !result.Any() || result.Last() != c))
            result.Add(c);
        return new string(result.ToArray());
    }
    
    
    public string ExecuteCommand(string command)
    {
        var details = CleanSpaces(command.ToLower()).Split(' ').Select(x => x.Split('.')).ToArray();
        var function = details[0];

        if (function[0] == "game")
            return ExecuteGame(details);
        return $"ERR:/Typo error \"{function[0]}\"";
    }


    private string ExecuteGame(string[][] details)
    {
        var function = details[0];
        
        if (function.Length == 1)
            return "ERR:/Command processing error";
        if (function[1].Split('[')[0] == "robots")
            return ExecuteGameRobots(details);
        return $"ERR:/Typo error \"{function[1]}\"";
    }


    private string ExecuteGameRobots(string[][] details)
    {
        var function = details[0];
        
        if (function.Length == 2)
            return "ERR:/Command processing error";
        var robotIdSplit = details[0][1].Replace("]", "").Split('[');
        if (robotIdSplit.Length != 2)
            return "ERR:/Please specify robot id";
        if (!long.TryParse(robotIdSplit[1], out var robotId))
            return "ERR:/Invalid robot id format";
        var robot = environment.robots.Find(x => x.actor.id == robotId);
        if (robot == null)
            return "ERR:/Robot not found";
        
        if (function[2] == "move_to")
            return ExecuteGameRobotsMoveTo(details, robot);        
        if (function[2] == "get_position")
            return ExecuteGameRobotsGetPosition(details, robot);
        return $"ERR:/Typo error \"{function[2]}\"";
    }


    private string ExecuteGameRobotsMoveTo(string[][] details, Robot robot)
    {
        if (details.Length != 3)
            return "ERR:/Command usage error";
        if (!int.TryParse(details[1][0], out var x))
            return "ERR:/Invalid x coordinate format";        
        if (!int.TryParse(details[2][0], out var y))
            return "ERR:/Invalid y coordinate format";
        if (Vector2Int.RoundToInt(robot.actor.position) == new Vector2Int(x, y))
            return "ERR:/Robot already here";
        if (!robot.pawn.MoveTo(new Vector2(x, y)))
            return "ERR:/Path not found";
        return "SUCCESS:/Success";
    }
    
    
    private string ExecuteGameRobotsGetPosition(string[][] details, Robot robot)
    {
        if (details.Length != 1)
            return "ERR:/Command usage error";
        var result = $"{{\"x\": {Mathf.RoundToInt(robot.actor.position.x)}," +
                     $" \"y\": {Mathf.RoundToInt(robot.actor.position.y)}}}";
        return "SUCCESS:/" + result;
    }
}
