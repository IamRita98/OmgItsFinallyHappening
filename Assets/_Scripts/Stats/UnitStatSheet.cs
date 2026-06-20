using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UnitStatSheet : MonoBehaviour
{
    public CharacterStat Health;
    public int health;
    private int maxHP;
    public CharacterStat Mana;
    public CharacterStat Strength;
    public CharacterStat Intelligence;
    public CharacterStat Defense;
    public CharacterStat MagicDefense;
    public CharacterStat Speed;
    public CharacterStat Skill;
    public CharacterStat Movement;
    public CharacterStat AttackRange;
    public CharacterStat HitChance;
    public GameObject attackRangeTiles;
    public List<Vector2> attackTiles=new List<Vector2>();

    public bool hasActionThisTurn;

    SpriteRenderer sRend;

    private void Start()
    {
        hasActionThisTurn = true;
        sRend = GetComponent<SpriteRenderer>();
        SetStatsForCombat();
    }
    private void OnEnable()
    {
        CombatHandler.UnitDied += HandleDeath;
        //Signal for taking damage += CheckHealthForClamp;
    }
    private void OnDisable()
    {
        CombatHandler.UnitDied -= HandleDeath;
        //Signal for taking damage -= CheckHealthForClamp;
    }
    private void HandleDeath(GameObject unitDied)
    {
        if (unitDied == gameObject)
        {
            gameObject.SetActive(false);
            //add animations, reset states, other things later
        }
        return;
    }

    void CheckHealthForClamp()
    {
        health = Mathf.Clamp(health, 0, maxHP);
    }

    void SetStatsForCombat()
    {
        health = maxHP = (int)Health.Value;
    }

    public void UnitTookTurn()
    {
        hasActionThisTurn = false;
        sRend.color = Color.gray;
        this.attackTiles.Clear();
    }

    public void NewTurn()
    {
        hasActionThisTurn = true;
        sRend.color = Color.white;
        this.attackTiles.Clear();
    }

    public void GetAttackRange()
    {
        for(int i = 0; i <= 1; i++)
        {
            for(int j = 0; j <= 1-i; j++)
            {
                if (!attackTiles.Contains(new Vector2(this.gameObject.transform.position.x+i, this.gameObject.transform.position.y + j)))
                {
                    attackTiles.Add(new Vector2(this.gameObject.transform.position.x + i, this.gameObject.transform.position.y + j));
                }
                if (!attackTiles.Contains(new Vector2(this.gameObject.transform.position.x + i, this.gameObject.transform.position.y - j)))
                {
                    attackTiles.Add(new Vector2(this.gameObject.transform.position.x + i, this.gameObject.transform.position.y - j));
                }
                if (!attackTiles.Contains(new Vector2(this.gameObject.transform.position.x - i, this.gameObject.transform.position.y - j)))
                {
                    attackTiles.Add(new Vector2(this.gameObject.transform.position.x - i, this.gameObject.transform.position.y - j));
                }
                if (!attackTiles.Contains(new Vector2(this.gameObject.transform.position.x - i, this.gameObject.transform.position.y + j)))
                {
                    attackTiles.Add(new Vector2(this.gameObject.transform.position.x - i, this.gameObject.transform.position.y + j));
                }
            }
        }
    }
    public void TryAttack()
    {
        if (attackTiles == null) GetAttackRange();
    }
}
