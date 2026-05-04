using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
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
    GameStateManager GMS;
    Pathfinding pf;
    UnitStatSheet unitStats;
    public bool enemyTurn=false;
    public List<Cell> pathToTake;
    float lerpTimer=0f;
    public float lerpTime = 0.1f;
    Vector2 currentPos;
    Vector2 newPos;
    bool isMoving=false;
    int effectiveRange;
    int distanceFromEnemy;

    private void Awake()
    {
        GMS = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
        pf = gameObject.GetComponent<Pathfinding>();
        unitStats = GetComponent<UnitStatSheet>();
    }

    private void Start()
    {
        effectiveRange = (int)unitStats.Movement.Value + (int)unitStats.AttackRange.Value;
    }

    public void MoveEnemy()
    {
        GameObject closestEnemy = GetPathingTarget();
        pf.FindPath(currentPos, (Vector2)closestEnemy.transform.position);
        pathToTake = pf.path;
        StartCoroutine(LerpRoutine());
    }

    GameObject GetPathingTarget()
    {
        int closestDistance = int.MaxValue;
        GameObject closestUnit = gameObject;
        foreach (GameObject playerUnit in GMS.playerUnits)
        {
            float tempDist = pf.ManhattanDistance(transform.position, playerUnit.transform.position);
            if (tempDist < closestDistance)
            {
                closestDistance = (int)tempDist;
                closestUnit = playerUnit;
            }
        }
        if (IsClosestEnemyInAttackRange(closestDistance))
        {
            //Prob add more detail to this later so that instead of it being true/false to attack or move it would instead still be false to move but true would
            //go on to the next step of logic checking whether they should attack or use an ability
            actionType = ActionType.ATTACKING;
        }
        else actionType = ActionType.MOVING;
        return (closestUnit);
    }

    bool IsClosestEnemyInAttackRange(int distance)
    {
        if (distance <= effectiveRange) return true;
        else return false;
    }

    private void Update()
    {
        if (!isMoving) currentPos = gameObject.transform.position;
    }

    IEnumerator LerpRoutine()
    {
        int tilesMoved = 0;
        while (pathToTake.Count > 0)
        {
            if (tilesMoved > unitStats.Movement.Value) yield break; //Enemy reached max movement--Using > instead of >= makes the enemy move their full movement despite us having logic that makes them do otherwise elsewhere
            if (pathToTake.Count < unitStats.AttackRange.Value && actionType == ActionType.ATTACKING) yield break; //Enemy reached their max attack range away from target
            Cell c = pathToTake[0];
            newPos = c.worldPosition;
            while (lerpTimer < lerpTime)
            {
                lerpTimer += Time.deltaTime;
                float percent = lerpTimer / lerpTime;
                transform.position = Vector2.Lerp(currentPos, newPos, percent);
                yield return null;
            }
            tilesMoved++;
            lerpTimer = 0;
            transform.position = newPos;
            currentPos = newPos;
            pathToTake.Remove(pathToTake[0]);
        }
    }
}
