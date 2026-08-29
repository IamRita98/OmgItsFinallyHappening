using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

/*public enum UnitSelectorIsHovering
{
    Player,
    Enemy,
    None,
}*/
/*enum TerrainSelectorIsHovering
{
    Grass,
    Forest,
    Water,
}*/
public class UnitSelector : MonoBehaviour
{
    public static UnitSelector Instance;
    int unitsTakenTurn = 0;
    UnitStatSheet unitStatSheet;
    PlayerExplorationController explorationController;
    public GameObject GOHovered;
    public GameObject EnemyGO;
    public GameObject PlayerGO;
    public GameObject playerUnitSelected;
    public int movementRange;
    List<Vector2> moveableTiles=new List<Vector2>();
    List<GameObject> moveableTilePlacements = new List<GameObject>();
    public float startingX;
    public float startingY;
    public Vector2 selectedGOPickupPos;
    public GameObject moveableTileMarker;
    public bool canMoveSelector = true;
    GridMovement gridMovement;
    public FMODUnity.EventReference selectSFXRef;
    public FMODUnity.EventReference invalidMoveSFXRef;
    public bool selectorCanSelect = true;
    Rigidbody2D rb;
    private uint playerGold = 500;
    public Sprite playerCombatSprite;
    SpriteRenderer playerSRend;
    private InputSystem_Actions actions;
    InputAction confirmKey;
    InputAction cancelKey;
    //public UnitSelectorIsHovering unitHovered;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);
        gridMovement = GetComponent<GridMovement>();
        rb = GetComponent<Rigidbody2D>();
        playerSRend = gameObject.GetComponent<SpriteRenderer>();
        actions = new InputSystem_Actions();

        confirmKey = InputSystem.actions.FindAction("Confirm");
        cancelKey = InputSystem.actions.FindAction("Cancel");
        //actions.Player.Confirm.performed += ctx => ConfirmKey;
        //actions.Player.Cancel.performed += ctx => CancelKey();
    }
    private void OnEnable()
    {
        actions.Enable();
    }
    private void OnDisable()
    {
        actions.Disable();
    }
    private void Start()
    {
        unitsTakenTurn = GameObject.FindGameObjectsWithTag("Player").Count();
        
        explorationController = GetComponent<PlayerExplorationController>();
    }
    
    private void Update()
    {
        if (GameStateManager.Instance.state != State.Combat) return;
        CheckForTurnPhase();
        //CheckForInputs();
        CheckHoveredUnit();
        if (confirmKey.WasPressedThisFrame()) ConfirmKey();
        if (cancelKey.WasPressedThisFrame()) CancelKey();
    }

    public void DisplayGold()
    {
        print("You have " + playerGold + " Gold");
    }

    public void AddGold(uint amt)
    {
        playerGold = (uint)Mathf.Clamp(playerGold + amt, uint.MinValue, uint.MaxValue);
        DisplayGold();
    }

    public void SubtractGold(uint amt)
    {
        playerGold = (uint)Mathf.Clamp(playerGold - amt, uint.MinValue, uint.MaxValue);

        /*        if (playerGold - amt < 0)
                {
                    playerGold =0;
                }
                else
                {
                    playerGold -= amt;    
                }*/
        DisplayGold();
    }

    public uint GetPlayerGold()
    {
        return playerGold;
    }

    public void CheckForTurnPhase()
    {
        if (unitsTakenTurn <= 0)
        {
            unitsTakenTurn = GameObject.FindGameObjectsWithTag("Player").Count();
            GameStateManager.Instance.EndPlayerPhase();
        }
    }
    /*    void CheckForInputs()
        {
            CancelKey();
            ConfirmKey();
        }*/

    public void ConfirmKey()
    {
        if (GameStateManager.Instance.state != State.Combat) return;
        //Pickup unit hovered
        if (HoveringReadyToActUnit()) PickupPlayerUnit();
        else if (playerUnitSelected != null) //Confirm Selected Units Movement
        {
            if (CheckIfValidMove()) MoveUnit();
            else Debug.Log("Can't drop here");
        }
        if (HoveringEnemyUnit()) ViewEnemyDetails();
    }

    void CancelKey()
    {
        if (GameStateManager.Instance.state != State.Combat) return;
        //Press X w/ unit held
        if (HasPlayerUnitSelected())//&& !HoveringEnemyUnit()<--- used to be in the if statement
            ReturnToPickLocationAndCancel();
    }

    void CheckHoveredUnit()
    {
        if (GOHovered != null)
        {
            unitStatSheet = GOHovered.GetComponent<UnitStatSheet>();
            if (GOHovered.CompareTag("Player")) PlayerGO = GOHovered;
            else if (GOHovered.CompareTag("Enemy")) EnemyGO = GOHovered;
        }
    }

    void PickupPlayerUnit()
    {//Should begin unit session here, will need to add logic for unit switching or just hardlock the player
        selectorCanSelect = false;
        playerUnitSelected = GOHovered.GetComponent<ISelectable>().Select();
        transform.position = playerUnitSelected.transform.position;
        movementRange = ((int)unitStatSheet.Movement.Value);
        selectedGOPickupPos = playerUnitSelected.transform.position;
        playerUnitSelected.transform.parent = this.gameObject.transform;
        //playerUnitSelected.transform.localPosition = Vector2.zero;
        GetValidMovementTiles();
        GOHovered = null;
        FMODUnity.RuntimeManager.PlayOneShotAttached(selectSFXRef, gameObject);
        return;
    }

    void MoveUnit()
    {
        Vector2 endPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
        //for testing purposes I will leave this here, but this should be moved somewhere else so it is only called once
        CommandManager.Instance.BeginUnitTurn(playerUnitSelected.GetInstanceID().ToString());
        MoveCommand moveCommand = new MoveCommand(playerUnitSelected, selectedGOPickupPos, endPos);
        CommandManager.Instance.Execute(moveCommand);
        DropSelected();
        GOHovered = playerUnitSelected;
        GameStateManager.Instance.state = State.Menu;
        UIManager.Instance.SetCombatOptionsStates();
        StopSelectorControl();
        FMODUnity.RuntimeManager.PlayOneShotAttached(selectSFXRef, gameObject);
    }

    void ViewEnemyDetails()
    {
        //This is to see more detailed obj hovered stats(could be enemy, terrain, or something else)
        Debug.Log("Selected Enemy:\nStr: " + unitStatSheet.Strength.Value + "\n" + "Def: " + unitStatSheet.Defense.Value);
        SpriteRenderer eSprite = GOHovered.GetComponent<SpriteRenderer>();
        eSprite.color = new Color(0.2f, 0.7f, 0.9f, .9f);
    }

    private bool CheckIfValidMove()
    {
        if (GOHovered == null && moveableTiles.Contains(new Vector2(transform.position.x, transform.position.y))) return true;
        else return false;
    }

    public void ReturnToPickLocationAndCancel()
    {
        playerUnitSelected.transform.position = selectedGOPickupPos;
        CancelSelection();
    }

    void DropSelected()
    {
        ClearMoveableTiles();
        GOHovered = null;
        playerUnitSelected.transform.parent = null;
        selectorCanSelect = false;
    }

    public void CancelSelection()
    {
        ClearMoveableTiles();
        playerUnitSelected.transform.parent = null;
        transform.position = selectedGOPickupPos;
        //playerUnitSelected.transform.position = selectedGOPickupPos;
        
        playerUnitSelected = null;
        GOHovered = PlayerGO;
        selectorCanSelect = true;
    }

    void ClearMoveableTiles()
    {
        moveableTiles.Clear();
        foreach (GameObject tile in moveableTilePlacements) Destroy(tile);
        moveableTilePlacements.Clear();
    }

    public void EndUnitTurn()
    {
        selectorCanSelect = true;
        UIManager.Instance.EnableUndo();
        PlayerGO.GetComponent<UnitStatSheet>().UnitTookTurn();
        CommandManager.Instance.ConfirmUnitTurn();
        playerUnitSelected = null;
        ResumeSelectorControl();
        unitsTakenTurn--;
        UIManager.Instance.ClearUI();
        GameStateManager.Instance.state = State.Combat;
    }

    private void GetValidMovementTiles()
    {
        
        for (int i = 0; i <= movementRange; i++)
        {
            for (int j = 0; j <= movementRange - i; j++)
            {
                if (!moveableTiles.Contains(new Vector2(this.gameObject.transform.position.x + i, this.gameObject.transform.position.y + j)))
                {
                    moveableTiles.Add(new Vector2(this.gameObject.transform.position.x + i, this.gameObject.transform.position.y + j));
                }
                if (!moveableTiles.Contains(new Vector2(this.gameObject.transform.position.x - i, this.gameObject.transform.position.y + j)))
                {
                    moveableTiles.Add(new Vector2(this.gameObject.transform.position.x - i, this.gameObject.transform.position.y + j));
                }
                if (!moveableTiles.Contains(new Vector2(this.gameObject.transform.position.x - i, this.gameObject.transform.position.y - j)))
                {
                    moveableTiles.Add(new Vector2(this.gameObject.transform.position.x - i, this.gameObject.transform.position.y - j));
                }
                if (!moveableTiles.Contains(new Vector2(this.gameObject.transform.position.x + i, this.gameObject.transform.position.y - j)))
                {
                    moveableTiles.Add(new Vector2(this.gameObject.transform.position.x + i, this.gameObject.transform.position.y - j));
                }
            }
        }
        foreach (var tile in moveableTiles)
        {
            GameObject tileMarker = Instantiate(moveableTileMarker, tile, Quaternion.identity);
            moveableTilePlacements.Add(tileMarker);
        }

    }

    public void StopSelectorControl()
    {
        canMoveSelector = false;
        gridMovement.canMove = false;
    }

    public void ResumeSelectorControl()
    {
        canMoveSelector = true;
        gridMovement.canMove = true;
    }

    public void InvalidMove()
    {
        GetComponent<SelectorJiggle>().Jiggle();
        FMODUnity.RuntimeManager.PlayOneShotAttached(invalidMoveSFXRef, gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Tilemap")) return;
        GOHovered = collision.gameObject;
    }
     
    private void OnTriggerExit2D(Collider2D collision)
    {
        //if(playerUnitSelected==null) GOHovered = null;
    }

    public void SetCombatPlayer()
    {
        playerSRend.sprite = playerCombatSprite;
        //explorationController.enabled = false; already done in Scene change
        gridMovement.enabled = true;
        rb.linearVelocity = Vector2.zero;
    }

    ///States
    bool HasPlayerUnitSelected()
    {
        return(playerUnitSelected.CompareTag("Player") && canMoveSelector);
    }

    public bool HoveringReadyToActUnit()
    {
        return(GOHovered == true && GOHovered.CompareTag("Player") && GOHovered.GetComponent<UnitStatSheet>().hasActionThisTurn && selectorCanSelect && canMoveSelector);
    }

    bool HoveringEnemyUnit()
    {
        return (GOHovered != null && GOHovered.CompareTag("Enemy"));
    }
}
