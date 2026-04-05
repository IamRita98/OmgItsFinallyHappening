using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject combatOptions;


    public void EnableCombatUI()
    {
        combatOptions.SetActive(true);
    }
    // Update is called once per frame
   
}
