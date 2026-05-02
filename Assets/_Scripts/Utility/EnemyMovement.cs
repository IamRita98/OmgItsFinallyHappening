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
    private void Start()
    {
        pf = gameObject.GetComponent<Pathfinding>();
    }
    public void MoveEnemy()
    {
        int r = Random.Range(1, 3);
        print(r);
        
        currentPos = gameObject.transform.position;
        if (r == 1)
        {
            Vector2 t = new Vector2(Mathf.Round(target1.transform.position.x), Mathf.Round(target1.transform.position.y));
            print( (double)target1.transform.position.x +" ,"+ (double)target1.transform.position.y);
            pf.FindPath(currentPos, t);
        }
        else
        {
            Vector2 t = new Vector2(Mathf.Round(target2.transform.position.x), Mathf.Round(target2.transform.position.y));
            print((double)target2.transform.position.x + " ," + (double)target2.transform.position.y);
            pf.FindPath(currentPos, t);
        }
        pathToTake = pf.path;
        GetFoundPath();
        
        
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
