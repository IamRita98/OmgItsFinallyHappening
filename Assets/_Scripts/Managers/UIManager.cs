using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject combatOptions;
    public Button attackButton;
    Selectable firstSelected;

    public void EnableCombatUI()
    {
        firstSelected = attackButton;
        combatOptions.SetActive(true);
        firstSelected.Select();
    }

    public void DisableCombatUI()
    {
        combatOptions.SetActive(false);
    }
    // Update is called once per frame
   
}
