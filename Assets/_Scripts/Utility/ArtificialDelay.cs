using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ArtificialDelay : MonoBehaviour
{
    float f;
    float fTimer;
    bool startTimer;

    private void Update()
    {
        /*if (!startTimer) return;
        fTimer++;*/
    }

    /// <summary>
    /// Pass the frames to wait and the string of the method you want to call after x frames have passed
    /// </summary>
    /// <param name="framesToWait"></param>
    /// <param name="methodToCall"></param>
    public void WaitXFrames(float framesToWait, string methodToCall)
    {
        Invoke(nameof(methodToCall), 0);
/*        f = framesToWait;
        startTimer = true;
        StartCoroutine(WaitForFrames(framesToWait, methodToCall));*/
    }
    IEnumerator WaitForFrames(float framesToWait, string methodToCall)
    {
        yield return new WaitUntil(() => fTimer >= f);
        startTimer = false;
        fTimer = 0;
    }
}
