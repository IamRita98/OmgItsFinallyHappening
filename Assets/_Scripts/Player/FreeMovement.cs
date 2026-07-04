using UnityEngine;
using UnityEngine.InputSystem;

public class FreeMovement : MonoBehaviour
{

    Rigidbody2D rb;
    public float speed = 1f;
    public Vector2 dir;
    public DirectionFacing dirFacing;
    public InputActionReference moveAction;
    private void OnEnable()
    {
        rb = GetComponent<Rigidbody2D>();
        
    }
    private void FixedUpdate()
    {
        rb.linearVelocity = dir * speed;
    }
    // Update is called once per frame
    void Update()
    {
        Movement();
        RayCastDir();
    }
    private void RayCastDir()
    {

    }
    private void Movement()
    {
        dir.x = Input.GetAxisRaw("Horizontal");
        dir.y = Input.GetAxisRaw("Vertical");
        dir.Normalize();
    }
}
