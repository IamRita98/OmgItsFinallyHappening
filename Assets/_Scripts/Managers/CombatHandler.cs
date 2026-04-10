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

    private void Awake()
    {
        allEnemiesList = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        unitSelectorGO = GameObject.FindGameObjectWithTag("UnitSelector");
    }
    public void AttackSelected()
    {
        playerObject = unitSelectorGO.GetComponent<UnitSelector>().PlayerGO;
        //enemyObject = GameObject.FindGameObjectWithTag("UnitSelector").GetComponent<UnitSelector>().GOHovered;
        DrawTiles drawTiles = gameObject.GetComponent<DrawTiles>();
        UnitStatSheet unitStats = playerObject.GetComponent<UnitStatSheet>();
        unitStats.GetAttackRange();
        drawTiles.DrawTilesGO(AttackTiles, unitStats.attackTiles);
        attackRange = unitStats.attackTiles;
        foreach (var tile in attackRange)
        {
            foreach (var enemy in allEnemiesList)
            {
                Vector2 temp=new Vector2(enemy.transform.position.x, enemy.transform.position.y);
                if (temp==tile) enemiesToAttack.Add(enemy);
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
        if(inCombat)
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
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //Confirm Attack
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                //Cancel Attack
                inCombat = false;
                enemiesToAttack.Clear();
            }
        }
    }
    public void PassTurnSelected()
    {
        //End Units Turn
    }
}
