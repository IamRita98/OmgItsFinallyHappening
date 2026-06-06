using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine.SceneManagement;


public enum State
{
    Menu,//Inventory/InCombatMenus/GeneralMenus
    Combat,//unitSelection/attackSelection/etc.
    Exploration,//NonCombat
    Dialogue,
}
public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance;
    public string currentSceneName;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        uSelector = GameObject.FindGameObjectWithTag("UnitSelector").GetComponent<UnitSelector>();
        currentSceneName = SceneManager.GetActiveScene().name;
    }
    public State state;

    public List <GameObject> playerUnits=new List<GameObject>();
    public List<GameObject> enemyUnits = new List<GameObject>();
    UnitSelector uSelector;
    int enemiesToTakeTheirTurn;
    int idx = 0;


    public enum TurnState
    {
        PlayerTurn,
        EnemyTurn,
        OutOfCombat
    }
    public TurnState gameState;
    private void OnEnable()
    {
        EnemyActions.EndedThisEnemyUnitTurn += EnemyPhaseValues;
    }
    private void OnDisable()
    {
        EnemyActions.EndedThisEnemyUnitTurn -= EnemyPhaseValues;
    }
    private void OnDestroy()
    {
        EnemyActions.EndedThisEnemyUnitTurn -= EnemyPhaseValues;
    }


    private void Start()
    {
        gameState = TurnState.PlayerTurn;
        state = State.Combat;
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
        gameState = TurnState.PlayerTurn;
        playerUnits.Clear();
        uSelector.ResumeSelectorControl();//this should be the very last line
    }

    public void StartEnemyPhase()
    {
        gameState = TurnState.EnemyTurn;
        if(enemyUnits.Count<=0||enemyUnits==null) enemyUnits = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        enemiesToTakeTheirTurn = enemyUnits.Count();
        idx = 0;
        EnemyAction();
        
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
    public void EnemyPhaseValues()
    {
        idx++;
        enemiesToTakeTheirTurn--;
        EnemyAction();
    }
    void EnemyAction()
    {
        if (gameState == TurnState.PlayerTurn) return;
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
                enemyUnits[idx].GetComponent<EnemyActions>().MoveEnemy();   
        }
            
        //Move
        //Wait until first unit has finished moving before continuing Loop
        //After movement is finished this enemy Attacks
        //StartPlayerPhase/EndEnemyPhase
    }
}
