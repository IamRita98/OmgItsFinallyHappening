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
    Vector3 gridSize;
    public GameObject GOHovered;
    public GameObject GOSelected;
    Vector2 selectedGOPickupPos;
    //public UnitSelectorIsHovering unitHovered;

    private void Awake()
    {
        gridSize = EditorSnapSettings.gridSize;
    }
    private void Update()
    {
        Move();
        CheckForInputs();
    }

    void CheckForInputs()
    {
        ConfirmKey();
        CancelKey();
    }

    void ConfirmKey()
    {
        //Pickup unit hovered
        if (Input.GetKeyDown(KeyCode.Z) && GOHovered != null)
        {
            GOSelected = GOHovered.GetComponent<ISelectable>().Select();
            GOSelected.transform.parent = this.gameObject.transform;
            selectedGOPickupPos = GOSelected.transform.position;
            GOHovered = null;
            return;
        }
        //Confirm selected units placement
        if (Input.GetKeyDown(KeyCode.Z) && GOSelected != null)
        {
            DropSelected(GOSelected);
            GOHovered = GOSelected;
        }
    }

    void CancelKey()
    {
        //Return selected unit to its starting pos
        if (GOSelected != null && Input.GetKeyDown(KeyCode.X))
        {
            GOSelected.transform.position = selectedGOPickupPos;
            DropSelected(GOSelected);
        }
    }

    void DropSelected(GameObject goSelected)
    {
        GOHovered = null;
        goSelected.transform.parent = null;
        GOSelected = null;
    }

    void Move()
    {
        Vector2 currentPos = transform.position;
        Vector2 newPos = transform.position;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) newPos = currentPos + Vector2.up * gridSize;
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) newPos = currentPos + Vector2.down * gridSize;
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) newPos = currentPos + Vector2.right * gridSize;
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) newPos = currentPos + Vector2.left * gridSize;

        gameObject.transform.position = newPos;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GOHovered = collision.gameObject;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        GOHovered = null;
    }
}
