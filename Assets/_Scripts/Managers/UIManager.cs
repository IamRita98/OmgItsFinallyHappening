using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject combatOptions;
    public GameObject combatCalcUI;
    public Button attackButton;
    Selectable firstSelected;
    public GameObject undoButton;

    public TMP_Text playerHPText;
    public TMP_Text playerDamageText;
    public TMP_Text playerHitChanceText;
    public TMP_Text playerCritChanceText;
    public TMP_Text enemyHPText;
    public TMP_Text enemyDamageText;
    public TMP_Text enemyHitChanceText;
    public TMP_Text enemyCritChanceText;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

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
    public void EnableUndo()
    {
        undoButton.SetActive(true);
    }
    public void DisableUndo()
    {
        undoButton.SetActive(false);
    }
}
