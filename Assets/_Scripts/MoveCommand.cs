using UnityEngine;

public class MoveCommand : ICommand
{//unsure if we need it to be monobehavour will revisit later
    public GameObject objectMoved;
    private Vector2 startPos = new Vector2();
    private Vector2 destPos = new Vector2();

    public MoveCommand(GameObject movedGO,Vector2 sPos,Vector2 dPos)
    {
        objectMoved = movedGO;
        startPos = sPos;
        destPos = dPos;
    }
    public void Execute()
    {
        //will talk with Alex about maybe moving movement here but not sure
    }
    public void Undo()
    {
        objectMoved.transform.position = startPos;
        UnitStatSheet unitStatSheet = objectMoved.GetComponent<UnitStatSheet>();
        unitStatSheet.NewTurn();
        unitStatSheet.attackTiles.Clear();
    }
}
