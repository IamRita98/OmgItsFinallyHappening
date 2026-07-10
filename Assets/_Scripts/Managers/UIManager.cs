using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public enum SubMenuStates
    {
        //Combat UI
        CombatOptions,
        SelectTarget,
        Inventory,
        Shop,
        //Non-Combat UI

    }

    public SubMenuStates subMenuStates;
    Canvas canvas;

    public static UIManager Instance;
    public InventoryList iList;
    private InventoryItem inventoryItem;
    UnitSelector unitSelector;

    public GameObject combatOptions;
    public GameObject combatCalcUI;
    public Button attackButton;
    Selectable firstSelected;
    public GameObject undoButton;
    public List<Tuple<string, Item,int>>shopStock=new List<Tuple<string, Item, int>>();
    public GameObject inventoryUI;
    public GameObject InvButtonPrefab;
    public GameObject shopButtonPrefab;
    public GameObject shopUI;
    
    public TMP_Text playerHPText;
    public TMP_Text playerDamageText;
    public TMP_Text playerHitChanceText;
    public TMP_Text playerCritChanceText;
    public TMP_Text enemyHPText;
    public TMP_Text enemyDamageText;
    public TMP_Text enemyHitChanceText;
    public TMP_Text enemyCritChanceText;

    public TMP_Text promptTextBox;
    public GameObject dBox;
    public Image dBoxImg;
    Transform[] dBoxChildren;

    List<string> UIInvList = new List<string>();
    List<string> shopList = new List<string>();
    public static event Action PlayerOpenedInventory;//signal not currently being used
    private InputSystem_Actions actions;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
        actions = new InputSystem_Actions();
        actions.UI.Submit.performed += ctx => UIConfirmKey();
        actions.UI.Cancel.performed += ctx => UICancelKey();

    }

    private void Start()
    {
        iList = GameObject.FindGameObjectWithTag("PersistentGameManager").GetComponent<InventoryList>();
        canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
        unitSelector = GameObject.FindGameObjectWithTag("UnitSelector").GetComponent<UnitSelector>();
        GetDialogueBoxReferences();
    }
    private void OnEnable()
    {
        DialogueManager.FinishedDialogue += HideDialogueComponents;
        CombatHandler.UsedItem += DecrementInvItem;
        actions.Enable();
    }

    private void OnDisable()
    {
        CombatHandler.UsedItem -= DecrementInvItem;
        DialogueManager.FinishedDialogue -= HideDialogueComponents;
        actions.Disable();
    }

    private void Update()
    {
        if (GameStateManager.Instance.state != State.Menu) return;
        //ProcessInputs();
    }

    private void DecrementInvItem()
    {
        inventoryItem.RemoveFromStack();
    }

    private void UIConfirmKey()
    {
        if (!CheckGameState()) return;
        //confirm I dont think we are doing anything with this because it is all done through button functionality
    }

    private void UICancelKey()
    {
        if (!CheckGameState()) return;
        switch (subMenuStates)
        {
            case (SubMenuStates.CombatOptions):
                Debug.Log("in combat options");
                GameStateManager.Instance.state = State.Combat;
                ClearUI();
                unitSelector.ReturnToPickLocationAndCancel();
                unitSelector.ResumeSelectorControl();
                break;
            case (SubMenuStates.SelectTarget):
                Debug.Log("in select target");
                SetCombatOptionsStates();
                break;
            case (SubMenuStates.Inventory):
                SetCombatOptionsStates();
                break;
            case (SubMenuStates.Shop):
                DestroyShop();
                GameStateManager.Instance.state = State.Exploration;
                break;
        }
    }
    public void ShowCombatCalcs(int playerHP, int enemyHP, int playerDamage, int enemyDamage, int playerHit, int enemyHit, int playerCrit, int enemyCrit)
    {
        combatCalcUI.SetActive(true); 
        foreach (Transform child in combatCalcUI.GetComponentsInChildren<Transform>(true).ToList())
        {
            child.gameObject.SetActive(true);
        }
        playerHPText.text = playerHP.ToString();
        playerDamageText.text = playerDamage.ToString();
        playerHitChanceText.text = playerHit.ToString();
        playerCritChanceText.text = playerCrit.ToString();

        enemyHPText.text = enemyHP.ToString();
        enemyDamageText.text = enemyDamage.ToString();
        enemyHitChanceText.text = enemyHit.ToString();
        enemyCritChanceText.text = playerCrit.ToString();
    }

    public void DestroyShop()
    {
        foreach (Transform child in shopUI.GetComponentsInChildren<Transform>(true).ToList())
        {
            if (child == shopUI.transform) continue;
            Destroy(child.gameObject);
            shopStock.Clear();
        }
    }
    
    async public void DisplayInventory(bool isShop=false,GameObject npcShop=null)
    {
        Shop shop = null;

        if (npcShop != null)
        {
            shop = npcShop.gameObject.GetComponent<Shop>();
        }

        /*
         * make shop script
         * setup separate function for shop
         */
        if (npcShop != null && isShop)
        {
            subMenuStates = SubMenuStates.Shop;
            //shop stuff
            shopUI.SetActive(true);
            foreach (Transform child in shopUI.GetComponentsInChildren<Transform>(true).ToList())
            {
                child.gameObject.SetActive(true);
            }

            foreach (var item in shopStock)
            {
                GameObject itemButton = Instantiate(shopButtonPrefab, shopUI.transform);
                List<Transform> itemButtonChildren = itemButton.GetComponentsInChildren<Transform>(true).ToList(); //0 parent, 1 sprite, 2 name, 3 amount
                //itemButtonChildren[1].sprite = itemSpriteSO
                itemButtonChildren[2].GetComponent<TMP_Text>().text = item.Item1 + "   -";
                itemButtonChildren[3].GetComponent<TMP_Text>().text = item.Item3.ToString();

                itemButton.GetComponent<Button>().onClick.AddListener(() => { shop.ShopItemButtonSetup(item.Item2); });
            }
        }
        else
        {
            //player inv stuff
            inventoryUI.SetActive(true);
            foreach (Transform child in inventoryUI.GetComponentsInChildren<Transform>(true).ToList())
            {
                child.gameObject.SetActive(true);
            }
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
                GameObject itemButton = Instantiate(InvButtonPrefab, inventoryUI.transform);
                List<Transform> itemButtonChildren = itemButton.GetComponentsInChildren<Transform>(true).ToList(); //0 parent, 1 sprite, 2 name, 3 amount
                //itemButtonChildren[1].sprite = itemSpriteSO
                itemButtonChildren[2].GetComponent<TMP_Text>().text = item.Key + "   -";
                itemButtonChildren[3].GetComponent<TMP_Text>().text = item.Value.GetStacks().ToString();

                itemButton.GetComponent<Button>().onClick.AddListener(() => { UseItem(item.Value.item,item.Value); });
            

                UIInvList.Add(item.Key);
            }
            PlayerOpenedInventory?.Invoke();
            firstSelected = inventoryUI.GetComponentsInChildren<Button>().ToList()[0];
            firstSelected.Select();
        }
        
        await UniTask.DelayFrame(1);
        

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



    public void UseItem(Item item,InventoryItem invItem)
    {
        item.GOTarget = unitSelector.GOHovered;
        item.Use();
        inventoryItem = invItem;
        CombatHandler.Instance.item = item;
/*        if (item.itemTargets == ItemTargets.NoTarget)
        {
            //Do Something
        }
        else
        {
            CombatHandler.Instance.SelectTarget(item.itemTargets);
        }*/

        //CALL TO UIMANAGER TO PULL UP LIST OF ENEMIES/ALLIES IN RANGE FOR THING
        //get target ->pulls up list of allies or enemies in range to use
        //once player picks target->target=target
        //then run itemEffects

        ClearUI();
        DecrementInvItem();
        unitSelector.EndUnitTurn();
    }

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
        if (!CheckGameState()) return;
        subMenuStates = SubMenuStates.SelectTarget;
        ClearUI();
        CombatHandler.Instance.SelectTarget();
        
    }

    public void SetInventoryStates()
    {
        if (!CheckGameState()) return;
        ClearUI();
        subMenuStates = SubMenuStates.Inventory;
        DisplayInventory();
    }

    void GetDialogueBoxReferences()
    {
        dBoxChildren = dBox.GetComponentsInChildren<Transform>(true);
    }

    public void HideDialogueComponents()
    {
        dBoxImg.enabled = false;
        foreach (Transform dBoxChild in dBoxChildren)
        {
            print("Interacting throguh dBoxChildren");
            dBoxChild.gameObject.SetActive(false);
        }
    }

    public void ShowDialogueComponents()
    {
        dBoxImg.enabled = true;
        foreach (Transform dBoxChild in dBoxChildren)
        {
            print("Interacting throguh dBoxChildren");
            dBoxChild.gameObject.SetActive(true);
        }
    }

    public void FillAndDisplayButtonPromptUI(string text)
    {
        promptTextBox.GetComponent<TMP_Text>().enabled = true;
        promptTextBox.text = text;
    }

    public void HideButtonPromptUI()
    {
        promptTextBox.GetComponent<TMP_Text>().enabled = false;
    }

    bool CheckGameState()
    {
        if (GameStateManager.Instance.state == State.Menu) return true;
        else return false;
    }
}
