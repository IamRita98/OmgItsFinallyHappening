using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerExplorationController : MonoBehaviour
{
    SpriteRenderer playerSRend;
    public Sprite playerExplorationSprite;
    public Vector2 rayDir;
    GridMovement gM;
    float maxRayDist = 1.3f;
    RaycastHit2D ray;
    int mask;
    FreeMovement fM;
    private void Start()
    {
        //playerSRend = gameObject.GetComponent<SpriteRenderer>();
        mask = LayerMask.GetMask("Interactable");
    }
    private void OnEnable()
    {
        playerSRend = gameObject.GetComponent<SpriteRenderer>();
        gM = gameObject.GetComponent<GridMovement>();
        fM = gameObject.GetComponent<FreeMovement>();
        gM.enabled=false;
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
        //CheckDirFacing();
        NattyDir();
        ray = Physics2D.Raycast(transform.position, rayDir,maxRayDist,mask);
        Quaternion dir2=transform.rotation = new Quaternion(gameObject.transform.rotation.x, transform.rotation.y, transform.rotation.z+15,0);
        Vector3 test = transform.position;
        transform.eulerAngles = new Vector3(0, 0, 15);
        //RaycastHit2D ray2 = Physics2D.Raycast(transform.rotation.z+15, rayDir, maxRayDist, mask);
        Quaternion dir3 = transform.rotation = new Quaternion(gameObject.transform.rotation.x, transform.rotation.y, transform.rotation.z - 15, 0);
        CheckRayHits();
        //check enum state and set direction
    }
    private void NattyDir()
    {
        if (fM.dir == Vector2.zero)
        {
            //maintain last dir
        }
        else {
            rayDir = fM.dir;
            
        }
        Debug.DrawRay(transform.position, rayDir * maxRayDist, Color.green, 1f);

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
