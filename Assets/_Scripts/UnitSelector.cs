using System.Collections;
using System.Collections.Generic;
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
    //public UnitSelectorIsHovering unitHovered;

    private void Awake()
    {
        gridMovement = GetComponent<GridMovement>();
        gridSize = EditorSnapSettings.gridSize;
        uiManager = GameObject.FindGameObjectWithTag("UIManager").GetComponent<UIManager>();
    }
    private void Update()
    {
        CheckForInputs();
    }

    void CheckForInputs()
    {
        CancelKey();
        if (!canMoveSelector) return;
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
        if (Input.GetKeyDown(KeyCode.Z) && GOHovered != null && PlayerGO != null && GOHovered.CompareTag("Player"))
        {
            movementRange = ((int)unitStatSheet.Movement.Value);
            GOSelected = GOHovered.GetComponent<ISelectable>().Select();
            GOSelected.transform.parent = this.gameObject.transform;
            selectedGOPickupPos = GOSelected.transform.position;
            GetValidMovementTiles();
            GOHovered = null;
            return;
        }

        //Confirm selected units placement
        if (Input.GetKeyDown(KeyCode.Z) && GOSelected != null)
        {
            bool valid=CheckIfValid();
            if (valid)
            {
                DropSelected(GOSelected);
                GOHovered = GOSelected;
                unitStatSheet.GetAttackRange();
                uiManager.EnableCombatUI();
                StopSelectorControl();
            }
            else
            {
                Debug.Log("Can't drop here");
            }
            
        }
        //if(GOHovered!=null && EnemyGO != null)
        //{
        //    SpriteRenderer eSprite = EnemyGO.GetComponent<SpriteRenderer>();
        //    eSprite.color = new Color(0.4f, 0.7f, 0.1f, .2f);
        //}
        if(Input.GetKeyDown(KeyCode.Z)&&GOHovered!=null && EnemyGO != null&&GOHovered.CompareTag("Enemy"))
        {
            Debug.Log("Selected Enemy:\nStr: " + unitStatSheet.Strength.Value + "\n" + "Def: " + unitStatSheet.Defense.Value);
            //diplay more info on Selection?
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
        }
    }

    void DropSelected(GameObject goSelected)
    {
        ClearMoveableTiles();
        
        GOHovered = null;
        goSelected.transform.parent = null;
        GOSelected = null;
    }
    void CancelSelection(GameObject goSelected)
    {
        ClearMoveableTiles();
        transform.position = selectedGOPickupPos;
        goSelected.transform.position = selectedGOPickupPos;
        goSelected.transform.parent = null;
        GOHovered = PlayerGO;
    }

    void ClearMoveableTiles()
    {

        moveableTiles.Clear();
        foreach (GameObject tile in moveableTilePlacements) Destroy(tile);
        moveableTilePlacements.Clear();
    }

    /*    void Select()
        {
            switch (unitHovered)
            {
                case UnitSelectorIsHovering.Player:
                    PickUpUnit();
                    break;
                case UnitSelectorIsHovering.Enemy:
                    break;
            }
        }*/

    //void PickUpUnit()

    private void GetValidMovementTiles()
    {
        Debug.Log("Trying to get tiles to move to");
        for (int i = 0; i <= movementRange; i++)
        {
            for (int j = 0; j <= movementRange-i; j++)
            {
                moveableTiles.Add(new Vector2(this.gameObject.transform.position.x+i, this.gameObject.transform.position.y+j));
                moveableTiles.Add(new Vector2(this.gameObject.transform.position.x - i, this.gameObject.transform.position.y + j));
                moveableTiles.Add(new Vector2(this.gameObject.transform.position.x-i, this.gameObject.transform.position.y-j));
                moveableTiles.Add(new Vector2(this.gameObject.transform.position.x + i, this.gameObject.transform.position.y - j));
            }

        }
        foreach (var tile in moveableTiles)
        {
            GameObject tileMarker = Instantiate(moveableTileMarker, tile, Quaternion.identity);
            moveableTilePlacements.Add(tileMarker);
        }
        //for(int i = (int)this.gameObject.transform.position.x;i<this.gameObject.transform.position.x+maxMovement; i++)
        //{
        //    for(int j = (int)this.gameObject.transform.position.y; j < this.gameObject.transform.position.y-i; j ++)
        //    {
        //        moveableTiles.Add(new Vector2(i, j));
        //        moveableTiles.Add(new Vector2(-1*i, -1*j));
        //    }

        //}
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
        GOHovered = null;
    }
}
