using UnityEngine;

public class InventoryItem
{
    public Item item;
    private int stacks;

    public InventoryItem(Item itemP)
    {
        item = itemP;
        stacks = 1;
    }
    public InventoryItem(Item itemP, int amount)
    {
        item = itemP;
        stacks = amount;
    }
    public void AddToStack()
    {
        stacks++;
    }
    public void AddToStack(int amount)
    {
        stacks += amount;
    }
    public void RemoveFromStack()
    {
        stacks--;
    }
    public int GetStacks()
    {
        return stacks;
    }
}
