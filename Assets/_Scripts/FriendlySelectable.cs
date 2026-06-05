using UnityEngine;

public class FriendlySelectable : MonoBehaviour, ISelectable
{
    public string type = "friendly";
    public GameObject Select()
    {
        print($"Selected this: {this.gameObject} w/ string: {type}");
        return this.gameObject;
    }
}
