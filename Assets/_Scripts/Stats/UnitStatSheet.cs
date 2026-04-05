using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatSheet : MonoBehaviour
{
    public CharacterStat Health;
    public CharacterStat Mana;
    public CharacterStat Strength;
    public CharacterStat Intelligence;
    public CharacterStat Defense;
    public CharacterStat MagicDefense;
    public CharacterStat Speed;
    public CharacterStat Skill;
    public CharacterStat Movement;
    public CharacterStat AttackRange;
    public GameObject attackRangeTiles;
    public List<Vector2> attackTiles=new List<Vector2>();
    public void GetAttackRange()
    {
        for(int i = 0; i <= 1; i++)
        {
            for(int j = 0; j <= 1; j++)
            {
                attackTiles.Add(new Vector2(this.gameObject.transform.position.x + i, this.gameObject.transform.position.y + j));
                attackTiles.Add(new Vector2(this.gameObject.transform.position.x + i, this.gameObject.transform.position.y - j));
                attackTiles.Add(new Vector2(this.gameObject.transform.position.x - i, this.gameObject.transform.position.y + j));
                attackTiles.Add(new Vector2(this.gameObject.transform.position.x - i, this.gameObject.transform.position.y - j));
            }
        }
    }
    public void TryAttack()
    {
        if (attackTiles == null) GetAttackRange();

    }
}
