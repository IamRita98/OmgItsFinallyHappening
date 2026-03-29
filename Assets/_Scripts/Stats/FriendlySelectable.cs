using UnityEngine;

public class FriendlySelectable : MonoBehaviour, ISelectable
{
    public GameObject Select()
    {
        print(this.gameObject);
        return this.gameObject;
    }
}
