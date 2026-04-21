using UnityEngine;
using System.Collections.Generic;
public class CommandManager: MonoBehaviour
{
    public static CommandManager Instance;
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
    //we track sessions so that each unit turn is independent of the others and we can rollback however many we want
    private class UnitSession
    {
        public string unitId;
        public Stack<ICommand> Actions = new Stack<ICommand>();
    }
    private UnitSession activeSession = null;
    private List<UnitSession> confirmedUnits = new List<UnitSession>();
    //unit sessions track their own command actions 
    //so we do not have to keep a counter of actions taken for each unit to roll back changes
    public void BeginUnitTurn(string UnitID)
    {//we can pass instanceId of the units or their GO names(each should be unique)
        if (activeSession != null) return;//end current units turn
        activeSession = new UnitSession { unitId = UnitID };
    }
    public void Execute(ICommand command)
    {
        if (activeSession == null) return;
        command.Execute();
        activeSession.Actions.Push(command);
    }
    public void Undo()
    {//don't get confused by this, when this is called it will "undo"
     //the last command added to the stack
        if (activeSession == null) return;
        if (activeSession.Actions.Count > 0)
        {
            ICommand lastCommand = activeSession.Actions.Pop();
            lastCommand.Undo();
        }
    }
    public void ConfirmUnitTurn()
    {
        if (activeSession == null) return;
        confirmedUnits.Add(activeSession);
        activeSession =null;//reset for next unit turn
    }
    public void ResetEntireTurn()
    {
        //check for charges or something first
        //if out of charges or whatever limit then warn player and return
        if (activeSession != null)
        {
            UndoSession(activeSession);
        }
        for(int i = confirmedUnits.Count - 1; i >= 0; i--)
        {
            UndoSession(confirmedUnits[i]);
        }
        confirmedUnits.Clear();
    }
    public void ResetPartialTurn(int unitSessionsToReset)
    {
        //check for charges or something first
        //if out of charges or whatever limit then warn player and return
        //also add a check to make sure the unitSessionsToReset is less thatn the sessions count
        if (activeSession != null)
        {
            UndoSession(activeSession);
        }
        for (int i = confirmedUnits.Count; unitSessionsToReset >= 0; unitSessionsToReset--)
        {
            UndoSession(confirmedUnits[i]);
        }
    }
    private void UndoSession(UnitSession session)
    {
        while (session.Actions.Count > 0)
        {
            ICommand com = session.Actions.Pop();
            com.Undo();
        }
    }
    public void EndTurn()
    {
        if (activeSession != null) ConfirmUnitTurn();
        confirmedUnits.Clear();
    }
}
