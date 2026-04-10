using UnityEditor;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    Vector3 gridSize;
    public float lerpTime;
    float lerpTimer;
    bool playerWantsToMove = false;
    public bool inCombat = false;
    UnitSelector unitSelector;
    public bool canMove = true;
    public Vector2 currentPos;
    Vector2 newPos;

    private void Awake()
    {
        gridSize = EditorSnapSettings.gridSize;
    }

    void Update()
    {
        if (!canMove) return;
        if (playerWantsToMove) LerpMovement();
        else Move();
    }
    void Move()
    {
        currentPos = transform.position;

        if (!inCombat)
        {
            if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
            {
                newPos = currentPos + Vector2.up * gridSize;
                playerWantsToMove = true;
            }
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                newPos = currentPos + Vector2.down * gridSize;
                playerWantsToMove = true;
            }
            if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                newPos = currentPos + Vector2.right * gridSize;
                playerWantsToMove = true;
            }
            if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                newPos = currentPos + Vector2.left * gridSize;
                playerWantsToMove = true;
            }
        }
        else
        {
            bool valid=unitSelector.CheckForMovement();
            if (valid)
            {
                if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
                {
                    newPos = currentPos + Vector2.up * gridSize;
                    playerWantsToMove = true;
                }
                if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
                {
                    newPos = currentPos + Vector2.down * gridSize;
                    playerWantsToMove = true;
                }
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                {
                    newPos = currentPos + Vector2.right * gridSize;
                    playerWantsToMove = true;
                }
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                {
                    newPos = currentPos + Vector2.left * gridSize;
                    playerWantsToMove = true;
                }
            }
        }
    }

    void LerpMovement()
    {
        lerpTimer += Time.deltaTime;

        float percent = lerpTimer / lerpTime;
        if (lerpTimer > lerpTime)
        {
            lerpTimer = 0;
            playerWantsToMove = false;
        }
        transform.position = Vector2.Lerp(currentPos, newPos, percent);
    }
}
