using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class GameStateManager : MonoBehaviour
{
    public List <GameObject> playerUnits=new List<GameObject>();
    public List<GameObject> enemyUnits = new List<GameObject>();
    UnitSelector uSelector;

    private void Awake()
    {
        uSelector = GameObject.FindGameObjectWithTag("UnitSelector").GetComponent<UnitSelector>();
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

    public void StartEnemyPhase()
    {
        if(enemyUnits.Count<=0||enemyUnits==null) enemyUnits = GameObject.FindGameObjectsWithTag("Enemy").ToList();
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
        foreach (GameObject enemyUnit in enemyUnits)
        {
            enemyUnit.GetComponent<EnemyMovement>().MoveEnemy(); //Move
            //Wait until first unit has finished moving before continuing Loop
            //After movement is finished this enemy Attacks
        }
    }
}
