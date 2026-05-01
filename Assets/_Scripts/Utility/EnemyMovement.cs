using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject target1;
    public GameObject target2;
    Pathfinding pf;
    public bool enemyTurn=false;
    List<Cell> pathToTake;
    float lerpTimer;
    public float lerpTime = 0.1f;
    Vector2 currentPos;
    Vector2 newPos;
    int idx = 0;
    bool isMoving=false;
    //bool isLerping=false;
    private void Start()
    {
        pf = GameObject.FindGameObjectWithTag("GameManager").GetComponent<Pathfinding>();
    }
    public void MoveEnemy()
    {
        int r = Random.Range(1, 3);
        
        currentPos = gameObject.transform.position;
        if (r == 1)
        {
            pf.FindPath(currentPos, target1.transform.position);
        }
        else
        {
            pf.FindPath(currentPos, target2.transform.position);
        }

        pathToTake = pf.path;
        while (pathToTake!=null)
        {
            Cell c = pathToTake[0];
            newPos = c.worldPosition;
            isMoving = true;
            LerpMovement();
            currentPos = newPos;
            pathToTake.Remove(pathToTake[0]);
        }
    }
    private void Update()
    {
        if (!isMoving) currentPos = gameObject.transform.position;
    }
    void LerpMovement()
    {
        lerpTimer += Time.deltaTime;

        float percent = lerpTimer / lerpTime;
        if (lerpTimer > lerpTime)
        {
            lerpTimer = 0;
            if (pathToTake != null)
            {
                isMoving = false;
                MoveEnemy();
            }
            
        }
        transform.position = Vector2.Lerp(currentPos, newPos, percent);
    }
}
