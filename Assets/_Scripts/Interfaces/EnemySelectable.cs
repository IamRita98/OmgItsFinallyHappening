using UnityEngine;

public class EnemySelectable : MonoBehaviour, ISelectable
{
    public string type = "enemy";
    public GameObject Select()
    {
        Debug.Log("Enemy Stats: 10 Attack");
        return this.gameObject;
    }
}
