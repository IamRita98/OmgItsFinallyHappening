using UnityEngine;

[CreateAssetMenu(fileName = "HealEffect", menuName = "Item/ItemEffects/HealEffect")]
public class HealEffect : ItemEffects
{
    public int healAmount;
    public override void ExecuteEffects(GameObject target)
    {
        target.GetComponent<UnitStatSheet>().health += healAmount;
    }
}
