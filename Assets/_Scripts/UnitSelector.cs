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
    public GameObject EnemyGO;
    public GameObject PlayerGO;
    public GameObject GOSelected;
    Vector2 selectedGOPickupPos;
    //public UnitSelectorIsHovering unitHovered;

    private void Awake()
    {
        gridSize = EditorSnapSettings.gridSize;
    }
    private void Update()
    {
        CheckForInputs();
    }

    void CheckForInputs()
    {
        ConfirmKey();
        CancelKey();
    }

    void ConfirmKey()
    {
        if (GOHovered != null)
        {
            if (GOHovered.CompareTag("Player")) PlayerGO = GOHovered;
            else if (GOHovered.CompareTag("Enemy")) EnemyGO = GOHovered;
        }
        //Pickup unit hovered
        if (Input.GetKeyDown(KeyCode.Z) && GOHovered != null && PlayerGO!=null&&GOHovered.CompareTag("Player"))
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
            bool valid=CheckIfValid();
            if (valid)
            {
                DropSelected(GOSelected);
                GOHovered = GOSelected;
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
            Debug.Log("Selected Enemy");
            //diplay more info on Selection?
            SpriteRenderer eSprite = GOHovered.GetComponent<SpriteRenderer>();
            eSprite.color = new Color(0.2f, 0.7f, 0.9f,.9f);
        }
    }

    private bool CheckIfValid()
    {
        if (GOHovered==null) return true;
        else return false;
        
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
