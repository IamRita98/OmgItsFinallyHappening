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
    int unitsTakenTurn = 0;
    UIManager uiManager;
    Vector3 gridSize;
    GameStateManager GSM;
    UnitStatSheet unitStatSheet;
    public GameObject GOHovered;
    public GameObject EnemyGO;
    public GameObject PlayerGO;
    public GameObject playerUnitSelected;
    public int movementRange;
    List<Vector2> moveableTiles=new List<Vector2>();
    List<GameObject> moveableTilePlacements = new List<GameObject>();
    public float startingX;
    public float startingY;
    Vector2 selectedGOPickupPos;
    public GameObject moveableTileMarker;
    public bool canMoveSelector = true;
    GridMovement gridMovement;
    public FMODUnity.EventReference selectSFXRef;
    public FMODUnity.EventReference invalidMoveSFXRef;
    CombatHandler combatHandler;
    public bool selectorCanSelect = true;
    //public UnitSelectorIsHovering unitHovered;

    private void Awake()
    {
        GSM = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameStateManager>();
        gridMovement = GetComponent<GridMovement>();
        uiManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<UIManager>();
        combatHandler = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CombatHandler>();
    }

    private void Start()
    {
        
        unitsTakenTurn = GameObject.FindGameObjectsWithTag("Player").Count();
    }

    private void Update()
    {
        CheckForTurnPhase();
        CheckForInputs();
        CheckHoveredUnit();
    }
    public void CheckForTurnPhase()
    {
        if (unitsTakenTurn <= 0)
        {
            unitsTakenTurn = GameObject.FindGameObjectsWithTag("Player").Count();
            GSM.EndPlayerPhase();
        }
    }
    void CheckForInputs()
    {
        CancelKey();
        ConfirmKey();
    }
    
    void ConfirmKey()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //Pickup unit hovered
            if(HoveringReadyToActUnit()) PickupPlayerUnit();
            else if (playerUnitSelected != null) //Confirm Selected Units Movement
            {
                if (CheckIfValidMove()) MoveUnit();
                else Debug.Log("Can't drop here");
            }

            if (HoveringEnemyUnit()) ViewEnemyDetails();
        }
    }

    void CancelKey()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            //Press X w/ unit held
            if (playerUnitSelected != null) ReturnToPickLocationAndCancel();

            //Press X while in Menu
            if (!canMoveSelector)
            {
                ResumeSelectorControl();
                CancelSelection();
                uiManager.DisableCombatUI();
            }

            //Press X while enemies are in attack range to clear tiles
            if (unitStatSheet.attackTiles.Count > 0)
            {
                unitStatSheet.attackTiles.Clear();
                DrawTiles dt = combatHandler.GetComponent<DrawTiles>();
                dt.ClearTiles();
            }
        }
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
        movementRange = ((int)unitStatSheet.Movement.Value);
        playerUnitSelected = GOHovered.GetComponent<ISelectable>().Select();
        selectedGOPickupPos = playerUnitSelected.transform.position;
        playerUnitSelected.transform.parent = this.gameObject.transform;
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
        UIManager.Instance.EnableCombatUI();
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
        transform.position = selectedGOPickupPos;
        playerUnitSelected.transform.position = selectedGOPickupPos;
        playerUnitSelected.transform.parent = null;
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
    }

    private void GetValidMovementTiles()
    {
        Debug.Log("Trying to get tiles to move to");
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
        GOHovered = collision.gameObject;
    }
     
    private void OnTriggerExit2D(Collider2D collision)
    {
        //if(playerUnitSelected==null) GOHovered = null;
    }


    ///States
    public bool HoveringReadyToActUnit()
    {
        if (GOHovered == true && GOHovered.CompareTag("Player") && GOHovered.GetComponent<UnitStatSheet>().hasActionThisTurn && selectorCanSelect) return true;
        else return false;
    }

    bool HoveringEnemyUnit()
    {
        if (GOHovered != null && GOHovered.CompareTag("Enemy")) return true;
        else return false;
    }
}
