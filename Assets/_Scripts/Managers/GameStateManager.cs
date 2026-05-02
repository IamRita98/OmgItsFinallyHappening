using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
public class GameStateManager : MonoBehaviour
{
    public List <GameObject> playerUnits=new List<GameObject>();
    public List<GameObject> enemyUnits = new List<GameObject>();

    public void EndPlayerPhase()
    {
        if(playerUnits.Count<=0||playerUnits==null) playerUnits = GameObject.FindGameObjectsWithTag("Player").ToList();

        foreach (var player in playerUnits)
        {
            player.GetComponent<UnitStatSheet>().NewTurn();
        }
        //stop player control
        //play enemy turn
        StartEnemyPhase();
    }
    public void StartEnemyPhase()
    {
        if(enemyUnits.Count<=0||enemyUnits==null) enemyUnits = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        foreach (GameObject enemyUnit in enemyUnits)
        {
            enemyUnit.GetComponent<EnemyMovement>().MoveEnemy();
        }
        //PATHFINDING
        //choice 1:pathfind to everthing to make ai make smarter decision
        //*look into having the enemy ai share its pathfinding data with others*
        //choice 2: pathfind to nearest because we are stupid and gave up
        //check for player units to attack in range (take into account moving this units movement range)
        //if there are player units to attack in range then use a prio system for targeting
        //if there are no player units attacke in range then path to nearest
        //use list of player units and go to nearest 
        /**/
    }
}
