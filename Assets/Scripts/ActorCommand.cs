using System;

public class ActorCommand : IComparable<ActorCommand>
{
    public Actor Executor { get; private set; }
    public Actor Target { get; private set; }
    public ActionType Type { get; private set; }

    public ActorCommand(Actor executor, Actor target, ActionType type)
    {
        Executor = executor;
        Target = target;
        Type = type;
    }

    public void Execute()
    {
        switch (Type)
        {
            case ActionType.Attack:
                Executor.Attack(Target);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    public enum ActionType
    {
        Attack,
    }

    public int CompareTo(ActorCommand other)
    {
        return other.Executor.Initiative.CompareTo(Executor.Initiative);
    }
}