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
    List<GameObject> enemiesToAttack = new List<GameObject>();
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
        inCombat = true;
        unitSelector.transform.position = enemiesToAttack[index].transform.position;
    }
    private void Update()
    {
        if(inCombat)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A))
            {
                if (index > 0)
                {
                    unitSelector.transform.position = enemiesToAttack[index--].transform.position;
                }
                else
                {
                    index = enemiesToAttack.Count();
                    unitSelector.transform.position = enemiesToAttack[index--].transform.position;
                }

            }
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D))
            {
                if (index < enemiesToAttack.Count() - 1)
                {
                    unitSelector.transform.position = enemiesToAttack[index++].transform.position;
                }
                else
                {
                    index = 0;
                    unitSelector.transform.position = enemiesToAttack[index++].transform.position;
                }
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                //Confirm Attack
            }
            else if (Input.GetKeyDown(KeyCode.X))
            {
                //Cancel Attack
            }
        }
    }
    public void PassTurnSelected()
    {
        //End Units Turn
    }
}
