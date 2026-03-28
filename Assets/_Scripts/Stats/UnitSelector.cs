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
    
    //public UnitSelectorIsHovering unitHovered;

    private void Awake()
    {
        gridSize = EditorSnapSettings.gridSize;
    }
    private void Update()
    {
        Move();
        //CheckForInputs();
    }

/*    void CheckForInputs()
    {
        if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Mouse0)) Select();
    }*/

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if (collision.collider.tag == "Player") unitHovered = UnitSelectorIsHovering.Player;
    }
}
