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
    CommandManager commandManager;
    UIManager uiManager;
    Vector3 gridSize;
    UnitStatSheet unitStatSheet;
    public GameObject GOHovered;
    public GameObject EnemyGO;
    public GameObject PlayerGO;
    public GameObject GOSelected;
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
    CombatHandler combatHandler;
    bool wasDropped = false;
    //public UnitSelectorIsHovering unitHovered;

    private void Awake()
    {
        
        gridMovement = GetComponent<GridMovement>();
        //gridSize = EditorSnapSettings.gridSize;
        uiManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<UIManager>();
        combatHandler = GameObject.FindGameObjectWithTag("GameManager").GetComponent<CombatHandler>();
    }
    private void Start()
    {
        commandManager = CommandManager.Instance;
        unitsTakenTurn = GameObject.FindGameObjectsWithTag("Player").Count();
    }
    private void Update()
    {
        CheckForInputs();
    }

    void CheckForInputs()
    {
        CancelKey();
        ConfirmKey();
    }

    void ConfirmKey()
    {
        if (GOHovered != null)
        {
            unitStatSheet = GOHovered.GetComponent<UnitStatSheet>();
            if (GOHovered.CompareTag("Player")) PlayerGO = GOHovered;
            else if (GOHovered.CompareTag("Enemy")) EnemyGO = GOHovered;
        }
        //Pickup unit hovered
        if (Input.GetKeyDown(KeyCode.Z) && GOHovered != null && PlayerGO != null && GOHovered.CompareTag("Player") && unitStatSheet.hasActionThisTurn && wasDropped == false)
        {
            movementRange = ((int)unitStatSheet.Movement.Value);
            GOSelected = GOHovered.GetComponent<ISelectable>().Select();
            selectedGOPickupPos = GOSelected.transform.position;
            GOSelected.transform.parent = this.gameObject.transform;
            GetValidMovementTiles();
            GOHovered = null;
            FMODUnity.RuntimeManager.PlayOneShotAttached(selectSFXRef, gameObject);
            return;
        }

        //Confirm selected units placement
        if (Input.GetKeyDown(KeyCode.Z) && GOSelected != null)
        {
            bool valid=CheckIfValid();
            if (valid)
            {
                Vector2 endPos = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
                MoveCommand moveCommand = new MoveCommand(GOSelected,selectedGOPickupPos, endPos);
                commandManager.Execute(moveCommand);
                DropSelected(GOSelected);
                GOHovered = GOSelected;
                UIManager.Instance.EnableCombatUI();
                StopSelectorControl();
                FMODUnity.RuntimeManager.PlayOneShotAttached(selectSFXRef, gameObject);
                
            }
            else
            {
                Debug.Log("Can't drop here");
            }
        }
        //Attack hovered Enemy
        if (Input.GetKeyDown(KeyCode.Z) && GOHovered != null && PlayerGO != null && GOHovered.CompareTag("Enemy"))
        {
            combatHandler.RunCombatCalc();
            UIManager.Instance.HideCombatCalcs();
            GameObject manager = GameObject.FindGameObjectWithTag("GameManager");
            manager.GetComponent<DrawTiles>().ClearTiles();
            EndUnitTurn();
            //end turn after
        }

        if (Input.GetKeyDown(KeyCode.Z)&&GOHovered!=null && EnemyGO != null&&GOHovered.CompareTag("Enemy"))
        {
            //This is to see more detailed obj hovered stats(could be enemy, terrain, or something else)
            Debug.Log("Selected Enemy:\nStr: " + unitStatSheet.Strength.Value + "\n" + "Def: " + unitStatSheet.Defense.Value);
            SpriteRenderer eSprite = GOHovered.GetComponent<SpriteRenderer>();
            eSprite.color = new Color(0.2f, 0.7f, 0.9f,.9f);
        }
    }

    private bool CheckIfValid()
    {
        if (GOHovered == null && moveableTiles.Contains(new Vector2(transform.position.x, transform.position.y))) return true;
        else return false;
        
    }
    void CancelKey()
    {
        //Return selected unit to its starting pos
        if (Input.GetKeyDown(KeyCode.X))
        {
            if(GOSelected != null)
            {
                GOSelected.transform.position = selectedGOPickupPos;
                CancelSelection(GOSelected);
            }
            if (!canMoveSelector)
            {
                ResumeSelectorControl();
                CancelSelection(PlayerGO);
                uiManager.DisableCombatUI();
            }
            if (unitStatSheet.attackTiles.Count > 0)
            {
                unitStatSheet.attackTiles.Clear();
                DrawTiles dt = combatHandler.GetComponent<DrawTiles>();
                dt.ClearTiles();

            }
        }
    }

    void DropSelected(GameObject goSelected)
    {
        ClearMoveableTiles();
        
        GOHovered = null;
        goSelected.transform.parent = null;
        GOSelected = null;
        wasDropped = true;
    }
    void CancelSelection(GameObject goSelected)
    {
        ClearMoveableTiles();
        transform.position = selectedGOPickupPos;
        goSelected.transform.position = selectedGOPickupPos;
        goSelected.transform.parent = null;
        GOHovered = PlayerGO;
        wasDropped = false;
    }

    void ClearMoveableTiles()
    {

        moveableTiles.Clear();
        foreach (GameObject tile in moveableTilePlacements) Destroy(tile);
        moveableTilePlacements.Clear();
    }

    public void EndUnitTurn()
    {
        wasDropped = false;
        UIManager.Instance.EnableUndo();
        PlayerGO.GetComponent<UnitStatSheet>().UnitTookTurn();
        ResumeSelectorControl();
    }

    private void GetValidMovementTiles()
    {
        Debug.Log("Trying to get tiles to move to");
        for (int i = 0; i <= movementRange; i++)
        {
            for (int j = 0; j <= movementRange-i; j++)
            {
                if(!moveableTiles.Contains(new Vector2(this.gameObject.transform.position.x + i, this.gameObject.transform.position.y + j)))
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
   public bool CheckForMovement()
    {
       
        return false;
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GOHovered = collision.gameObject;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        //if(GOSelected==null) GOHovered = null;

    }
}
