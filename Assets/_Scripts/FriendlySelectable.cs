using UnityEngine;

public class FriendlySelectable : MonoBehaviour, ISelectable
{
    public int tempStat=3;
    public string type = "friendly";
    public GameObject Select()
    {
        print(this.gameObject);
        return this.gameObject;
    }
}
