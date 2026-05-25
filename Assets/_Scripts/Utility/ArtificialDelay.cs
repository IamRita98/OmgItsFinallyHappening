using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtificialDelay : MonoBehaviour
{
    /// <summary>
    /// Pass in frames you want to wait. Used to add delay for race conditions + maybe loading screen
    /// </summary>
    public void WaitXFrames(float framesToWait)
    {
        StartCoroutine(WaitForFrames(framesToWait));
    }

    IEnumerator WaitForFrames(float framesToWait)
    {
        yield return WaitForFrames(framesToWait);
    }
}
