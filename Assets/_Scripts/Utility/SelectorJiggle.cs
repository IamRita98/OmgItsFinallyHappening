using System.Collections;
using UnityEngine;

public class SelectorJiggle : MonoBehaviour
{
    private float waitTime = .5f;
    private float lerpTimer=0f;
    private float lerpTime = .50f;
    private float lifeTime = 1 / 5;
    Vector2 leftJiggle;
    Vector2 rightJiggle;
    Vector2 starPos;
    bool lerp = false;
    public void Jiggle()
    {
        starPos = gameObject.transform.position;
        leftJiggle = new Vector2((float)gameObject.transform.position.x - .3f,gameObject.transform.position.y);
        rightJiggle = new Vector2(gameObject.transform.position.x + .3f, gameObject.transform.position.y);
        this.gameObject.GetComponent<UnitSelector>().StopSelectorControl();
        lerp = true;
        Debug.Log("once");
        StartCoroutine(JiggleRoutine());
        this.gameObject.GetComponent<UnitSelector>().ResumeSelectorControl();
    }
    private IEnumerator JiggleRoutine()
    {
        while (lerpTimer < lerpTime&&lerp)
        {
            lerpTimer += Time.deltaTime;
            float percent = lerpTimer / lerpTime;
            transform.position = Vector2.Lerp(starPos, leftJiggle, .5f);
            if (lerpTimer >= lerpTime)
            {
                lerp = false;
            }
            Debug.Log("left loop");
           
        }
        yield return null;
    }
    private void Update()
    {
        
        //lerp = true;
    }
    public void JiggleLeft()
    {
        lerpTimer += Time.deltaTime;
        float percent = lerpTimer / lerpTime;
        if (lerpTimer > lerpTime)
        {
            lerpTimer = 0;
            lerp = false;
        }
        transform.position = Vector2.Lerp(starPos, leftJiggle, percent);
        Debug.Log("left loop");
        //JiggleRight();
    }
    public void JiggleRight()
    {
        lerp = true;
        while (lerp)
        {
            lerpTimer += Time.deltaTime;
            float percent = lerpTimer / lerpTime;
            if (lerpTimer > lerpTime)
            {
                lerpTimer = 0;
                lerp = false;
            }
            transform.position = Vector2.Lerp(leftJiggle, rightJiggle, percent);
            Debug.Log("Right loop");
        }
        lerp = true;
        //JiggleCenter();
    }
    public void JiggleCenter()
    {
        lerp = true;
        while (lerp)
        {
            lerpTimer += Time.deltaTime;
            float percent = lerpTimer / lerpTime;
            if (lerpTimer > lerpTime)
            {
                lerpTimer = 0;
                lerp = false;
            }
            transform.position = Vector2.Lerp(rightJiggle, starPos, percent);
            Debug.Log("back to center");
        }
    }

}
