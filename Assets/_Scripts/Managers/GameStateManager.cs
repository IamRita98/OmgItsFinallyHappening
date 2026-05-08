using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
public class GameStateManager : MonoBehaviour
{
    public List <GameObject> playerUnits=new List<GameObject>();
    public List<GameObject> enemyUnits = new List<GameObject>();
    UnitSelector uSelector;
    int enemiesToTakeTheirTurn;
    int idx = 0;


    public enum GameState
    {
        PlayerTurn,
        EnemyTurn,
        OutOfCombat
    }
    public GameState gameState;
    private void OnEnable()
    {
        EnemyMovement.EndedThisEnemyUnitTurn += EnemyActions;
    }
    private void OnDisable()
    {
        EnemyMovement.EndedThisEnemyUnitTurn -= EnemyActions;
    }
    private void OnDestroy()
    {
        EnemyMovement.EndedThisEnemyUnitTurn -= EnemyActions;
    }
    private void Awake()
    {
        uSelector = GameObject.FindGameObjectWithTag("UnitSelector").GetComponent<UnitSelector>();
    }

    private void Start()
    {
        gameState = GameState.PlayerTurn;
    }

    public void EndPlayerPhase()
    {
        if (playerUnits.Count <= 0 || playerUnits == null) playerUnits = GameObject.FindGameObjectsWithTag("Player").ToList();

        uSelector.StopSelectorControl();
       
        foreach (var player in playerUnits)
        {
            player.GetComponent<UnitStatSheet>().NewTurn();
        }
        StartEnemyPhase();
    }
    public void StartPlayerTurn()
    {
        gameState = GameState.PlayerTurn;
        playerUnits.Clear();
        uSelector.ResumeSelectorControl();//this should be the very last line
    }

    public void StartEnemyPhase()
    {
        gameState = GameState.EnemyTurn;
        if(enemyUnits.Count<=0||enemyUnits==null) enemyUnits = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        enemiesToTakeTheirTurn = enemyUnits.Count();
        idx = -1;
        EnemyActions();
        
        //PATHFINDING
        //choice 1:pathfind to everthing to make ai make smarter decision
        //*look into having the enemy ai share its pathfinding data with others*
        //choice 2: pathfind to nearest because we are stupid and gave up
        //check for player units to attack in range (take into account moving this units movement range)
        //if there are player units to attack in range then use a prio system for targeting
        //if there are no player units attacke in range then path to nearest
        //use list of player units and go to nearest 
        /**/

        //uSelector.ResumeSelectorControl();
    }
    void EnemyActions()
    {
        if (gameState == GameState.PlayerTurn) return;
        if (enemiesToTakeTheirTurn <= 0)
        {
            //end enemy phase
            idx = 0;
            enemiesToTakeTheirTurn = 0;
            enemyUnits.Clear();
            StartPlayerTurn();
        }
        else
        {
            idx++;
            enemiesToTakeTheirTurn--;
            if (idx > enemyUnits.Count - 1) EnemyActions();
            else {
                enemyUnits[idx].GetComponent<EnemyMovement>().MoveEnemy();
            }
            
        }
            
        //Move
        //Wait until first unit has finished moving before continuing Loop
        //After movement is finished this enemy Attacks
        //StartPlayerPhase/EndEnemyPhase
    }
}
