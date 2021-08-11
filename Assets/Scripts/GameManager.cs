using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<ActorCommand> commands;
    private List<Actor> actors;

    private static GameManager instance;

    private void Awake()
    {
        instance = this;
        commands = new List<ActorCommand>();
        actors = new List<Actor>();
    }

    public static void AddActor(Actor actor)
    {
        instance.actors.Add(actor);
    }

    public static void RemoveActor(Actor actor)
    {
        instance.actors.Remove(actor);
    }

    public static void AddCommand(ActorCommand command)
    {
        instance.commands.Add(command);
    }

    public static void EndTurn()
    {
        foreach (Actor actor in instance.actors)
        {
            ActorCommand command = actor.GetCommand();
            if (command != null)
                instance.commands.Add(command);
        }
        instance.ExecuteCommands();
    }

    private void ExecuteCommands()
    {
        foreach (ActorCommand command in instance.commands)
        {
            command.Execute();
        }
        
        instance.commands.Clear();
    }
}