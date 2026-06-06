using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public enum SubMenuStates
    {
        //Combat UI
        CombatOptions,
        SelectTarget,
        Inventory,
        //Non-Combat UI

    }

    public SubMenuStates subMenuStates;
    Canvas canvas;

    public static UIManager Instance;
    public InventoryList iList;

    UnitSelector unitSelector;

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

    public GameObject dBox;
    public Image dBoxImg;
    Transform[] dBoxChildren;

    List<string> UIInvList = new List<string>();

    public static event Action PlayerOpenedInventory;//signal not currently being used

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
    }

    private void Start()
    {
        iList = GameObject.FindGameObjectWithTag("PersistentGameManager").GetComponent<InventoryList>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        unitSelector = GameObject.FindGameObjectWithTag("UnitSelector").GetComponent<UnitSelector>();
        GetDialogueBoxReferences();
    }

    private void Update()
    {
        if (GameStateManager.Instance.state != State.Menu) return;
        ProcessInputs();
    }

    private void ProcessInputs()
    {
        if (Input.GetKeyDown(KeyCode.Z)) 
        { 
            
        }//confirm I dont think we are doing anything with this

        if (Input.GetKeyDown(KeyCode.X))
        {
            switch (subMenuStates)
            {
                case (SubMenuStates.CombatOptions):
                    GameStateManager.Instance.state = State.Combat;
                    ClearUI();
                    unitSelector.ResumeSelectorControl();
                    break;
                case (SubMenuStates.SelectTarget):
                    SetCombatOptionsStates();
                    break;
                case (SubMenuStates.Inventory):
                    SetCombatOptionsStates();
                    break;
            }
        }
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

    async public void DisplayInventory()
    {
        inventoryUI.SetActive(true);
        Dictionary<string, InventoryItem> pInv = iList.GetPartyInv();
        foreach (var item in pInv)
        {
            if (UIInvList.Count > 0)
            {
                if (UIInvList.Contains(item.Key))
                {
                    continue;
                }
            }
            //TMP_Text invTextBoxSprite = Instantiate(invTextBoxGO, InvItemSpriteParent.transform);
            TMP_Text invTextBoxName = Instantiate(invTextBoxGO, InvItemNameParent.transform);
            TMP_Text invTextBoxAmount = Instantiate(invTextBoxGO, InvItemAmountParent.transform);

            //invTextboxSprite.sprite = whateverthefk
            invTextBoxName.text = item.Key;
            invTextBoxAmount.text = item.Value.GetStacks().ToString();

            UIInvList.Add(item.Key);
        }
        await UniTask.DelayFrame(1);
        PlayerOpenedInventory?.Invoke();

        //go through list-- For each element create a new text box
        //Fill textbox w/ Inventory item info
        //Later we will probably want to filter the in-combat inventory to only display Consumable itemtypes
        //Outside of combat(in towns) we will probably display all inventory items
        //When we get to this we will also want a way to be able to filter through items while in town so that we only display one of weapons/armor/consumables at a time
    }


/*    public void ConfirmCombatItemUse()
    {
        HideInv();
        ShowCombatCalcs(int playerHP, int enemyHP, int playerDamage, int enemyDamage, int playerHit, int enemyHit, int playerCrit, int enemyCrit)
    }*/


    public void EnableUndo()
    {
        undoButton.SetActive(true);
    }
    public void DisableUndo()
    {
        undoButton.SetActive(false);
    }

    public void ClearUI()
    {
        List<Transform> childrenOfCanvas = canvas.GetComponentsInChildren<Transform>().ToList();
        if (childrenOfCanvas.Count <= 0) return;
        for (int i = 1; i < childrenOfCanvas.Count; i++)
        {
            childrenOfCanvas[i].gameObject.SetActive(false);
        }
/*        foreach (Transform child in childrenOfCanvas)
        {
            child.gameObject.SetActive(false);
        }*/
    }

    public void SetCombatOptionsStates()
    {
        ClearUI();
        subMenuStates = SubMenuStates.CombatOptions;
        firstSelected = attackButton;
        combatOptions.SetActive(true);
        foreach (Transform child in combatOptions.GetComponentsInChildren<Transform>(true).ToList())
        {
            child.gameObject.SetActive(true);
        }
        firstSelected.Select();
    }

    public void SetSelectTargetStates()
    {
        ClearUI();
        CombatHandler.Instance.AttackSelected();
        subMenuStates = SubMenuStates.SelectTarget;
    }

    public void SetInventoryStates()
    {
        ClearUI();
        DisplayInventory();
        subMenuStates = SubMenuStates.Inventory;
    }

    void GetDialogueBoxReferences()
    {
        dBoxChildren = dBox.GetComponentsInChildren<Transform>(true);
    }

    public void HideDialogueComponents()
    {

    }

    public void ShowDialogueComponents()
    {
        dBoxImg.enabled = true;
        foreach (Transform dBoxChild in dBoxChildren)
        {
            print("Interating throguh dBoxChildren");
            dBoxChild.gameObject.SetActive(true);
        }
    }
}
