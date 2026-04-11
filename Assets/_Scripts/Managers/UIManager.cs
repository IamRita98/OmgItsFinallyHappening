using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject combatOptions;
    public GameObject combatCalcUI;
    public Button attackButton;
    Selectable firstSelected;

    public TMP_Text playerHPText;
    public TMP_Text playerDamageText;
    public TMP_Text playerHitChanceText;
    public TMP_Text playerCritChanceText;
    public TMP_Text enemyHPText;
    public TMP_Text enemyDamageText;
    public TMP_Text enemyHitChanceText;
    public TMP_Text enemyCritChanceText;

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

    public void ShowCombatCalcs(int playerHP, int enemyHP, int playerDamage, int enemyDamage, int playerHit, int enemyHit, int playerCrit, int enemyCrit)
    {
        combatCalcUI.SetActive(true); 
        playerHPText.text = playerHP.ToString();
        playerDamageText.text = playerDamage.ToString();
        playerHitChanceText.text = playerHit.ToString();
        playerCritChanceText.text = playerCrit.ToString();

        enemyHPText.text = enemyHP.ToString();
        enemyDamageText.text = enemyDamage.ToString();
        enemyHitChanceText.text = enemyHit.ToString();
        enemyCritChanceText.text = playerCrit.ToString();
    }

    public void HideCombatCalcs()
    {
        combatCalcUI.SetActive(false);
    }
}
