using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
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
    Rigidbody2D rb;
    public float speed = 1f;
    public Vector2 dir;
    public DirectionFacing dirFacing;
    Transform interactionCone;
    public GameObject GOInInteractionCone;
    public InputActionReference moveAction;
    private InputSystem_Actions actions;
    
    private void Awake()
    {
        actions = new InputSystem_Actions();
        actions.Player.Move.performed+=context =>SendMessage();
    }

    void SendMessage()
    {
        Debug.Log("PlayerMoved");
    }
    private void Start()
    {
        mask = LayerMask.GetMask("Interactable");
    }
    private void OnEnable()
    {
        playerSRend = gameObject.GetComponent<SpriteRenderer>();
        gM = gameObject.GetComponent<GridMovement>();
        rb = GetComponent<Rigidbody2D>();
        actions.Player.Enable();
        SetExplorationPlayer();
    }

    private void OnDisable()
    {
        actions.Player.Disable();
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = dir * speed;
    }

    private void Update()
    {
        if (GameStateManager.Instance.state != State.Exploration) return;
        Movement();
        CheckDirFacing();
        CheckForInputs();
        
    }

    private void Movement()
    {
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.y = Input.GetAxisRaw("Vertical");
        dir.Normalize();
    }

    public void SetExplorationPlayer()
    {
        gM.enabled = false;
        interactionCone = transform.GetChild(0);
        interactionCone.gameObject.SetActive(true);
        interactionCone.transform.localPosition = Vector3.zero;
        playerSRend.sprite = playerExplorationSprite;
    }

    void CheckForInputs()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (GOInInteractionCone != null)
            {
/*                if (GOInInteractionCone.gameObject.GetComponent<LootSelectable>() != null) Check if Chest item, else run dialogue
                {
                    GOInInteractionCone.gameObject.GetComponent<IInteractable>().Interact();

                }*/
                //This will be else instead of if once we have a lootSelectable
                if(GOInInteractionCone.gameObject.GetComponent<IInteractable>() != null)
                {
                    GameStateManager.Instance.state = State.Dialogue;
                    GOInInteractionCone.gameObject.GetComponent<IInteractable>().Interact();
                }

                //else chest/items logic later
                
            }
        }
    }

    /*    void CheckRayCast()
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
        }*/
    /*    private void NattyDir()
        {
            if (fM.dir == Vector2.zero)
            {
                //maintain last dir
            }
            else {
                rayDir = fM.dir;

            }
            Debug.DrawRay(transform.position, rayDir * maxRayDist, Color.green, 1f);

        }*/
    /*    private void CheckRayHits()
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
        }*/
    private void CheckDirFacing()
    {
        if (dir.y > 0) interactionCone.transform.rotation = Quaternion.Euler(0, 0, 0);
        else if (dir.y < 0) interactionCone.transform.rotation = Quaternion.Euler(0,0, 180);
        else if (dir.x > 0) interactionCone.transform.rotation = Quaternion.Euler(0, 0, 270);
        else if (dir.x < 0) interactionCone.transform.rotation = Quaternion.Euler(0, 0, 90);

    }
}
