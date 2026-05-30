using UnityEngine;

public class PlayerExplorationController : MonoBehaviour
{
    SpriteRenderer playerSRend;
    public Sprite playerExplorationSprite;
    public Vector2 rayDir;
    GridMovement gM;
    float maxRayDist = 2f;
    RaycastHit2D ray;
    int mask;
    private void Start()
    {
        //playerSRend = gameObject.GetComponent<SpriteRenderer>();
        mask = LayerMask.GetMask("Interactable");
    }
    private void OnEnable()
    {
        playerSRend = gameObject.GetComponent<SpriteRenderer>();
        gM = gameObject.GetComponent<GridMovement>();
    }
    private void Update()
    {
        CheckRayCast();
    }
    public void SetExplorationPlayer()
    {
        playerSRend.sprite = playerExplorationSprite;
    }
    void CheckRayCast()
    {
        CheckDirFacing();
        ray = Physics2D.Raycast(transform.position, rayDir,maxRayDist,mask);
        CheckRayHits();
        //check enum state and set direction
    }
    private void CheckRayHits()
    { 
        if (ray)
        {
            //if (ray.collider.TryGetComponent<ISelectable>(out ISelectable sel))
            //{
            //    Debug.Log($"hit {sel}, OWNER: {sel.}");
            //    //do stuff
            //}
            Debug.Log($"Hit this {ray.collider.gameObject}");
            if (ray.collider.TryGetComponent<ISelectable>(out ISelectable sel))
            {
                
                Debug.Log($"hit {sel}, OWNER: {ray.collider.gameObject}");
                //do stuff
            }
        }
    }
    private void CheckDirFacing()
    {
        if (gM.dirFacing == DirectionFacing.up)
        {
            rayDir = Vector2.up;
            Debug.DrawRay(transform.position, rayDir*maxRayDist, Color.green,1f);
        }
        else if (gM.dirFacing == DirectionFacing.down)
        {
            rayDir = Vector2.down;
            Debug.DrawRay(transform.position, rayDir*maxRayDist, Color.green,1f);
        }
        else if (gM.dirFacing == DirectionFacing.right)
        {
            rayDir = Vector2.right;
            Debug.DrawRay(transform.position, rayDir*maxRayDist, Color.green,1f);
        }
        else if(gM.dirFacing==DirectionFacing.left)
        {
            rayDir = Vector2.left;
            Debug.DrawRay(transform.position, rayDir*maxRayDist, Color.green,1f);
        }
    }

}
