using System.Collections;
using UnityEngine;

public class SelectorJiggle : MonoBehaviour
{
    Vector2 startPos;
    Vector2 leftJigglePos;
    Vector2 rightJigglePos;
    float jiggleDistance = .15f;
    bool wantToJiggleLeft = false;
    bool wantToJiggleRight = false;
    bool wantToJiggleCenter = false;

    float lerpTimer = 0f;
    float lerpTime = .05f;
    float longLerpTime = .1f;

    public void Jiggle()
    {
        gameObject.GetComponent<UnitSelector>().StopSelectorControl();
        startPos = transform.position;
        leftJigglePos = new Vector2(startPos.x - jiggleDistance, transform.position.y);
        rightJigglePos = new Vector2(startPos.x + jiggleDistance, transform.position.y);
        wantToJiggleLeft = true;
    }

    private void Update()
    {
        if (!wantToJiggleLeft && !wantToJiggleRight && !wantToJiggleCenter) return;
        else if (wantToJiggleLeft) StartJiggle();
        else if (wantToJiggleRight) RightJiggle();
        else if (wantToJiggleCenter) CenterJiggle();
    }

    void StartJiggle()
    {
        print("Move Left");
        lerpTimer += Time.deltaTime;
        float p = lerpTimer / lerpTime;
        if (lerpTimer > lerpTime)
        {
            lerpTimer = 0f;
            wantToJiggleLeft = false;
            wantToJiggleRight = true;
        }
        transform.position = Vector2.Lerp(startPos, leftJigglePos, p);
    }

    void RightJiggle()
    {
        print("Move Right");
        lerpTimer += Time.deltaTime;
        float p = lerpTimer / longLerpTime;
        if (lerpTimer > longLerpTime)
        {
            lerpTimer = 0f;
            wantToJiggleRight = false;
            wantToJiggleCenter = true;
        }
        transform.position = Vector2.Lerp(leftJigglePos, rightJigglePos, p);
    }

    void CenterJiggle()
    {
        print("Move Center");
        lerpTimer += Time.deltaTime;
        float p = lerpTimer / lerpTime;
        if (lerpTimer > lerpTime)
        {
            lerpTimer = 0f;
            wantToJiggleCenter = false;
            gameObject.GetComponent<UnitSelector>().ResumeSelectorControl();
        }
        transform.position = Vector2.Lerp(rightJigglePos, startPos, p);
    }
}
