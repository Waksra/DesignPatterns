using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<ActorCommand> commands;
    private List<Actor> actors;

    private GamePhase currentPhase;

    private static GameManager instance;

    public static GamePhase CurrentPhase => instance.currentPhase;

    private void Awake()
    {
        instance = this;
        commands = new List<ActorCommand>();
        actors = new List<Actor>();
        
        currentPhase = GamePhase.CommandPhase;
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
        instance.currentPhase = GamePhase.ExecutePhase;
        
        foreach (Actor actor in instance.actors)
        {
            ActorCommand command = actor.GetCommand();
            if (command != null)
                instance.commands.Add(command);
        }

        instance.commands.Sort();
        instance.StartCoroutine(instance.ExecuteCommands());
    }

    private IEnumerator ExecuteCommands()
    {
        foreach (ActorCommand command in commands)
        {
            command.Execute();

            while (command.Executor.CurrentState != Actor.ActorState.Idle)
                yield return null;
        }
        
        commands.Clear();
        currentPhase = GamePhase.CommandPhase;
    }
    
    public enum GamePhase
    {
        CommandPhase,
        ExecutePhase,
    }
}