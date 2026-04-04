using UnityEngine;

public class FriendlySelectable : MonoBehaviour, ISelectable
{
    public string type = "friendly";
    public GameObject Select()
    {
        print(this.gameObject);
        return this.gameObject;
    }
}
