using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject target1;
    public GameObject target2;
    GameStateManager GMS;
    Pathfinding pf;
    public bool enemyTurn=false;
    public List<Cell> pathToTake;
    float lerpTimer=0f;
    public float lerpTime = 0.1f;
    Vector2 currentPos;
    Vector2 newPos;
    bool isMoving=false;
    private void Awake()
    {
        GMS = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
        pf = gameObject.GetComponent<Pathfinding>();
    }

    public void MoveEnemy()
    {
        //Target AI--Closest enemy for now, later make more sophisticated to target squishiest in range?
        Vector2 t = GetPathingTarget();
        pf.FindPath(currentPos, t);
        pathToTake = pf.path;
        GetFoundPath();
    }

    Vector2 GetPathingTarget()
    {
        float closestDistance = Mathf.Infinity;
        GameObject closestUnit = gameObject;
        foreach (GameObject playerUnit in GMS.playerUnits)
        {
            float tempDist = pf.ManhattanDistance(transform.position, playerUnit.transform.position);
            if (tempDist < closestDistance)
            {
                closestDistance = tempDist;
                closestUnit = playerUnit;
            }
        }
        return (Vector2)closestUnit.transform.position;
    }

    private void GetFoundPath()
    {
        LerpMovement();
    }

    private void Update()
    {
        if (!isMoving) currentPos = gameObject.transform.position;
    }

    void LerpMovement()
    {//convert to coroutine or implement back into update
        StartCoroutine(LerpRoutine());
        
    }

    IEnumerator LerpRoutine()
    {
        while (pathToTake.Count > 0)
        {
            Cell c = pathToTake[0];
            newPos = c.worldPosition;
            while (lerpTimer < lerpTime)
            {
                lerpTimer += Time.deltaTime;
                float percent = lerpTimer / lerpTime;
                transform.position = Vector2.Lerp(currentPos, newPos, percent);
                yield return null;
            }
            lerpTimer = 0;
            transform.position = newPos;
            currentPos = newPos;
            pathToTake.Remove(pathToTake[0]);
        }
    }
}
