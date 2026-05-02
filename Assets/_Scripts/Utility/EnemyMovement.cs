using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public GameObject target1;
    public GameObject target2;
    Pathfinding pf;
    public bool enemyTurn=false;
    public List<Cell> pathToTake;
    float lerpTimer=0f;
    public float lerpTime = 0.1f;
    Vector2 currentPos;
    Vector2 newPos;
    bool isMoving=false;
    //bool isLerping=false;
    private void Start()
    {
        pf = gameObject.GetComponent<Pathfinding>();
    }
    public void MoveEnemy()
    {
        int r = Random.Range(1, 3);
        
        currentPos = gameObject.transform.position;
        if (r == 1)
        {
            pf.FindPath(currentPos, (Vector2)target1.transform.position);
        }
        else
        {
            pf.FindPath(currentPos, target2.transform.position);
        }
        pathToTake = pf.path;
        GetFoundPath();
        
        
    }
    private void GetFoundPath()
    {
        print(pf.path);
        while (pathToTake != null)
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
    {//convert to coroutine or implement back into update
        print("trying to lerp");
        StartCoroutine(LerpRoutine());
        
    }
    IEnumerator LerpRoutine()
    {
        print("in coroutine");
        while (lerpTimer < lerpTime)
        {
            print("In loop");
            lerpTimer += Time.deltaTime;
            float percent = lerpTimer / lerpTime;
            transform.position = Vector2.Lerp(currentPos, newPos, percent);
            yield return null;
        }
        transform.position = newPos;
        if (pathToTake != null)
        {
            isMoving = false;
            GetFoundPath();
        }
    }
}
