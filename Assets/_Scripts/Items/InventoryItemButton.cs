using UnityEngine;
using UnityEngine.UI;

public class InventoryItemButton : MonoBehaviour
{
    Button thisButton;

    private void Start()
    {
        thisButton = GetComponent<Button>();
    }

    public void FillItemInfo(InventoryItem item)
    {

    }

    public void RunButton()
    {
        //Add listener
    }
}
