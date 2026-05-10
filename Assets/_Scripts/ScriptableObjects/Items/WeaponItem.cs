using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponItem", menuName = "Scriptable Objects/WeaponItem")]
public class WeaponItem : Item
{
    //are gonna affect stats
    public Rarity rarity;
   
   
    public void DoSomething()
    {
        Debug.Log("hello");
    }
}
