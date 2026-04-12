using UnityEngine;

public class UseItemCommand : MonoBehaviour,ICommand
{
    //track item used
    //maybe implement an inventory system so it might be like playerObj.inventory.remove(item)
    //track stats
    public void Execute()
    {
        //To be implemented
    }
    public void Undo()
    {
        //To be implemented
    }
}
