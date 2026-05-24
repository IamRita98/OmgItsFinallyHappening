using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections.Generic;
using Unity.Collections;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    public InventoryList iList;

    public GameObject combatOptions;
    public GameObject combatCalcUI;
    public Button attackButton;
    Selectable firstSelected;
    public GameObject undoButton;

    public GameObject inventoryUI;
    public TMP_Text invTextBoxGO;
    public GameObject InvItemSpriteParent;
    public GameObject InvItemNameParent;
    public GameObject InvItemAmountParent;

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

    private void Start()
    {
        iList = GameObject.FindGameObjectWithTag("PersistentGameManager").GetComponent<InventoryList>();
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

    public void DisplayInventory()
    {
        inventoryUI.SetActive(true);
        Dictionary<string, InventoryItem> pInv = iList.GetPartyInv();
        foreach (var item in pInv)
        {
            //TMP_Text invTextBoxSprite = Instantiate(invTextBoxGO, InvItemSpriteParent.transform);
            TMP_Text invTextBoxName = Instantiate(invTextBoxGO, InvItemNameParent.transform);
            TMP_Text invTextBoxAmount = Instantiate(invTextBoxGO, InvItemAmountParent.transform);

            //invTextboxSprite.sprite = whateverthefk
            invTextBoxName.text = item.Key;
            invTextBoxAmount.text = item.Value.GetStacks().ToString();
        }
        //go through list-- For each element create a new text box
        //Fill textbox w/ Inventory item info
        //Later we will probably want to filter the in-combat inventory to only display Consumable itemtypes
        //Outside of combat(in towns) we will probably display all inventory items
        //When we get to this we will also want a way to be able to filter through items while in town so that we only display one of weapons/armor/consumables at a time
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
