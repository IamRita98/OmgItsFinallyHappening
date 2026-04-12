using UnityEngine;
using System.Collections.Generic;
public class CommandManager: MonoBehaviour
{
    public static CommandManager Instance;
    public Stack<ICommand> commandHistory = new Stack<ICommand>();//public for testing purposes, will be private
    private void Awake()//We only ever want one of these
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Execute(ICommand command)
    {
        command.Execute();
        commandHistory.Push(command);
    }
    public void Undo()
    {//don't get confused by this, when this is called it will "undo"
     //the last command added to the stack
        if (commandHistory.Count > 0)
        {
            ICommand lastCommand = commandHistory.Pop();
            lastCommand.Undo();
        }
    }
}
