using System.Collections;
using UnityEditor;
using UnityEngine;
using static EnemyMovement;

public class Lerping : MonoBehaviour
{
    Vector2 goalPosition;
    public float lerpTime;
    float lerpTimer = 0;
    Vector2 currentPos;
    public void LerpMovement(Vector2 goalPos)
    {
        goalPosition = goalPos;
        currentPos= gameObject.transform.position;
        StartCoroutine(LerpRoutine());
    }
    public void SetEnemyValues(Vector2 goalPos)
    {
        goalPosition = goalPos;
        currentPos = gameObject.transform.position;
    }
    public IEnumerator LerpRoutine()
    {
        while (lerpTimer < lerpTime)
        {
            lerpTimer += Time.deltaTime;
            float percent = lerpTimer / lerpTime;
            transform.position = Vector2.Lerp(currentPos, goalPosition, percent);
            yield return null;
        }
        lerpTimer = 0;
        transform.position = goalPosition;
    }
}
