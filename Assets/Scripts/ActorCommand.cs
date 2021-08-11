using System;

public class ActorCommand
{
    private Action action; 

    public ActorCommand(Action actionToExecute)
    {
        action = actionToExecute;
    }

    public void Execute()
    {
        action.Invoke();
    }
}
