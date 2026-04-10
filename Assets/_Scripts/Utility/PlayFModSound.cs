using UnityEngine;

public class PlayFModSound : MonoBehaviour
{
    public FMODUnity.EventReference sfx;

    public void PlaySFX()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached(sfx, gameObject);
    }
}
