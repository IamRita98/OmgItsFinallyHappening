using JetBrains.Annotations;
using System.Collections;
using UnityEditor;
using UnityEngine;

public enum DirectionFacing
{
    left,
    right,
    up,
    down
}
public class GridMovement : MonoBehaviour
{
    float gridSizeSide = 1;
    Vector3 gridSize;
    public float lerpTime;
    bool playerWantsToMove = false;
    public bool inCombat = false;
    UnitSelector unitSelector;
    public bool canMove = true;
    public Vector2 currentPos;
    Vector2 newPos;
    public FMODUnity.EventReference moveSFXRef;
    Lerping lerp;
    public DirectionFacing dirFacing;

   
    private void Awake()
    {
        gridSize = new Vector3(gridSizeSide, gridSizeSide, gridSizeSide);
        unitSelector = this.gameObject.GetComponent<UnitSelector>();
    }
    private void Start()
    {
        lerp = gameObject.GetComponent<Lerping>();
        
    }
    void Update()
    {
        if (!canMove) return;//just remember actual movement happens a frame later
        if (playerWantsToMove) return;
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
                RemoveGOHoveredOnMovement();
                FMODUnity.RuntimeManager.PlayOneShotAttached(moveSFXRef, gameObject);
                dirFacing = DirectionFacing.up;
                StartCoroutine(LerpMovement());
            }
            else if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
            {
                newPos = currentPos + Vector2.down * gridSize;
                playerWantsToMove = true;
                RemoveGOHoveredOnMovement();
                FMODUnity.RuntimeManager.PlayOneShotAttached(moveSFXRef, gameObject);
                dirFacing = DirectionFacing.down;
                StartCoroutine(LerpMovement());
            }
            else if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
            {
                newPos = currentPos + Vector2.right * gridSize;
                playerWantsToMove = true;
                RemoveGOHoveredOnMovement();
                dirFacing = DirectionFacing.right;
                FMODUnity.RuntimeManager.PlayOneShotAttached(moveSFXRef, gameObject);
                StartCoroutine(LerpMovement());
            }
            else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
            {
                newPos = currentPos + Vector2.left * gridSize;
                playerWantsToMove = true;
                RemoveGOHoveredOnMovement();
                dirFacing = DirectionFacing.left;
                FMODUnity.RuntimeManager.PlayOneShotAttached(moveSFXRef, gameObject);
                StartCoroutine(LerpMovement());
            }
        }
        /* else checks for terrain and other things perhaps
         {
             
         }*/
    }
    void RemoveGOHoveredOnMovement()
    {
        unitSelector.GOHovered = null;
    }

    IEnumerator LerpMovement()
    {
        lerp.SetValues(newPos);
        yield return StartCoroutine(lerp.LerpRoutine());
        playerWantsToMove = false;
    }
}
