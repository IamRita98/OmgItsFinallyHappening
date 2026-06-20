using UnityEngine;

public class ItemHandler : MonoBehaviour
{
    public void ItemUsed(GameObject unitSelected, Item item)
    {
        item.Use();
    }
}
