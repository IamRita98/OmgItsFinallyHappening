using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyActions : MonoBehaviour
{
    public enum ActionType
    {
        MOVING,
        ATTACKING,
        ABILITY,
        //add more as needed
    }
    public ActionType actionType;
    public GameObject target1;
    public GameObject target2;
    Pathfinding pf;
    CombatHandler cHandler;
    UnitStatSheet unitStats;
    public bool enemyTurn=false;
    public List<Cell> pathToTake;
    Vector2 currentPos;
    Vector2 newPos;
    bool isMoving=false;
    int effectiveRange;
    public bool needsToMove = false;
    GameObject bestTarget;
    Lerping lerp;

    public static event Action EndedThisEnemyUnitTurn;
    private void Awake()
    {
        pf = gameObject.GetComponent<Pathfinding>();
        unitStats = GetComponent<UnitStatSheet>();
    }

    private void Start()
    {
        cHandler = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CombatHandler>();
        effectiveRange = (int)unitStats.Movement.Value + (int)unitStats.AttackRange.Value;
        lerp = gameObject.GetComponent<Lerping>();
    }

    public void MoveEnemy()
    {
        print("Going into Move Enemy");
        bestTarget = GetPathingTarget();
        if (needsToMove||actionType==ActionType.MOVING)
        {
            pf.FindPath(currentPos, (Vector2)bestTarget.transform.position);
            pathToTake = pf.path;
            StartCoroutine(TryLerp());
        }
        else
        {
            if (actionType == ActionType.ATTACKING) { 
                Attack(); 
            }
            else if (actionType == ActionType.ABILITY)
            {
                //do the things
            }
        }
    }

    GameObject GetPathingTarget()
    {
        print("Going into Pathing target");
        int closestDistance = int.MaxValue;
        GameObject closestUnit = gameObject;
        foreach (GameObject playerUnit in GameStateManager.Instance.playerUnits)
        {
            float tempDist = pf.ManhattanDistance(transform.position, playerUnit.transform.position);
            if (tempDist < closestDistance)
            {
                closestDistance = (int)tempDist;
                closestUnit = playerUnit;
            }
        }
        if (IsBestTargetInAttackRange(closestDistance))
        {
            //Prob add more detail to this later so that instead of it being true/false to attack or move it would instead still be false to move but true would
            //go on to the next step of logic checking whether they should attack or use an ability
            actionType = ActionType.ATTACKING;
        }
        else actionType = ActionType.MOVING;
        return (closestUnit);
    }

    bool IsBestTargetInAttackRange(int distance)
    {
        print("Going into BestTarget");
        if (distance > unitStats.AttackRange.Value) needsToMove = true;
        else needsToMove = false;

        if (distance <= effectiveRange) {
            return true; 
        }
        else return false;
    }

    private void Update()
    {
        if (!isMoving) currentPos = gameObject.transform.position;
    }
    IEnumerator TryLerp()
    {
        
        int tilesMoved = 0;
        while (pathToTake.Count > 0)
        {
            if (tilesMoved > unitStats.Movement.Value) break; //Enemy reached max movement--Using > instead of >= makes the enemy move their full movement despite us having logic that makes them stop 1 early elsewhere
            if (pathToTake.Count < unitStats.AttackRange.Value && actionType == ActionType.ATTACKING) break; //Enemy reached their max attack range away from target
            Cell c = pathToTake[0];
            newPos = c.worldPosition;
            lerp.SetValues(newPos);
            yield return StartCoroutine(lerp.LerpRoutine());
            //bool testB = true;
            //while (testB)
            //{
            //    testB = lerp.isLerping;
            //}
            //while (lerpTimer < lerpTime)
            //{
            //    lerpTimer += Time.deltaTime;
            //    float percent = lerpTimer / lerpTime;
            //    transform.position = Vector2.Lerp(currentPos, newPos, percent);
            //    yield return null;
            //}
            tilesMoved++;
            //transform.position = newPos;
            currentPos = newPos;
            pathToTake.Remove(pathToTake[0]);
        }
        if (actionType == ActionType.ATTACKING) Attack(); //I'm putting this here for now, later we might want to break it up and leave this class to just handle movement?
        else
        {
            EndedThisEnemyUnitTurn?.Invoke();
        }
    }
  

    void Attack()
    {
        cHandler.CombatCalc(unitStats, bestTarget.GetComponent<UnitStatSheet>());
        cHandler.RunCombatCalc();
        EndedThisEnemyUnitTurn?.Invoke();
    }
}
