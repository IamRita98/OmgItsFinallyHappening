using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CombatHandler : MonoBehaviour
{
    float playerUnitRange;
    GameObject unitSelectorGO;
    UnitSelector unitSelector;
    GameObject playerObject;
    GameObject enemyObject;
    public GameObject AttackTiles;
    public List<GameObject> enemiesToAttack = new List<GameObject>();
    List<Vector2> attackRange = new List<Vector2>();
    public List<GameObject> allEnemiesList = new List<GameObject>();
    bool inCombat = false;
    int index = 0;
    DrawTiles drawTiles;
    UnitStatSheet unitStats;
    int critMulti = 2;

    UnitStatSheet attackerStats;
    UnitStatSheet defenderStats;
    int attackerHp;
    int attackerDamage;
    int attackerHitChance;
    int attackerCritChance;

    int defenderHp;
    int defenderDamage;
    int defenderHitChance;
    int defenderCritChance;

    private void Awake()
    {
        allEnemiesList = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        unitSelectorGO = GameObject.FindGameObjectWithTag("UnitSelector");
        drawTiles = gameObject.GetComponent<DrawTiles>();
    }
    public void AttackSelected()
    {
        playerObject = unitSelectorGO.GetComponent<UnitSelector>().PlayerGO;
        //enemyObject = GameObject.FindGameObjectWithTag("UnitSelector").GetComponent<UnitSelector>().GOHovered;

        unitStats = playerObject.GetComponent<UnitStatSheet>();
        unitStats.GetAttackRange();
        drawTiles.DrawTilesGO(AttackTiles, unitStats.attackTiles);
        attackRange = unitStats.attackTiles;
        foreach (var tile in attackRange)
        {
            foreach (var enemy in allEnemiesList)
            {
                Vector2 temp = new Vector2(enemy.transform.position.x, enemy.transform.position.y);
                if (temp == tile) enemiesToAttack.Add(enemy);
            }
        }
        if (enemiesToAttack.Count > 0)
        {
            inCombat = true;
            Vector2 tempPos = enemiesToAttack[index].transform.position;
            unitSelectorGO.transform.position = tempPos;
        }//do something else if no enemies

    }
    private void Update()
    {
        if (inCombat)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                index--;
                if (index < 0)
                {
                    index = enemiesToAttack.Count - 1;
                }
                Vector2 tempPos = enemiesToAttack[index].transform.position;
                unitSelectorGO.transform.position = tempPos;
            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                index++;
                if (index >= enemiesToAttack.Count())
                {
                    index = 0;
                }
                Vector2 tempPos = enemiesToAttack[index].transform.position;
                unitSelectorGO.transform.position = tempPos;
            }
            UnitStatSheet enemyStats = enemiesToAttack[index].GetComponent<UnitStatSheet>();
            CombatCalc(unitStats, enemyStats);
            //CombatCalc()
            //UI.ShowCombatCalcs()

            if (Input.GetKeyDown(KeyCode.Z))
            {
                //Confirm Attack
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                //Cancel Attack
                inCombat = false;
                enemiesToAttack.Clear();
                drawTiles.ClearTiles();
                UIManager.Instance.HideCombatCalcs();
            }
        }
    }

    public void CombatCalc(UnitStatSheet attackerStatsP, UnitStatSheet defenderStatsP)
    {
        attackerStats = attackerStatsP;
        defenderStats = defenderStatsP;

        attackerHp = (int)(attackerStats.Health.Value);
        attackerDamage = (int)Mathf.Clamp((attackerStats.Strength.Value - defenderStats.Defense.Value), 1, Mathf.Infinity);
        attackerHitChance = (int)(attackerStats.HitChance.Value + attackerStats.Skill.Value - defenderStats.Speed.Value);
        attackerCritChance = (int)(attackerStats.Skill.Value);

        defenderHp = (int)(defenderStats.Health.Value);
        defenderDamage = (int)Mathf.Clamp((defenderStats.Strength.Value - attackerStats.Defense.Value), 1, Mathf.Infinity);
        defenderHitChance = (int)(defenderStats.HitChance.Value + defenderStats.Skill.Value - attackerStats.Speed.Value);
        defenderCritChance = (int)(defenderStats.Skill.Value);
        //if(isPlayersTurn)
        UIManager.Instance.ShowCombatCalcs(attackerHp, defenderHp, attackerDamage, defenderDamage, attackerHitChance, defenderHitChance, attackerCritChance, defenderCritChance);
    }

    public void RunCombatCalc()
    {
        //Attacker Hit
        int hitRoll = Random.Range(1, 101);
        if (hitRoll >= 100 - attackerHitChance)
        {
            hitRoll = Random.Range(1, 101);
            if (hitRoll >= 100 - attackerCritChance)
            {
                attackerDamage *= critMulti;
                print("BIG CRIT!!!!");
            }
        }

        //Defender Counter attack
        /*        if(EnemycanCounterAttack)
         *        else defenderDamage = 0;
                hitRoll = Random.Range(1, 101);
                if (hitRoll >= 100 - defenderHitChance)
                {

                }
         */

        // Only run attack command if player turn
        AttackCommand attackerAttack = new AttackCommand(attackerStats, defenderStats, attackerDamage, defenderDamage);
        CommandManager.Instance.Execute(attackerAttack);
        inCombat = false;
    }
}
