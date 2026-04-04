using UnityEditor;
using UnityEngine;

public class GridMovement : MonoBehaviour
{
    Vector3 gridSize;
    public bool inCombat = false;
    UnitSelector unitSelector;
    private void Awake()
    {
        gridSize = EditorSnapSettings.gridSize;
    }

    void Update()
    {
        Move();
    }
    void Move()
    {
        Vector2 currentPos = transform.position;
        Vector2 newPos = transform.position;
        if (!inCombat)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) newPos = currentPos + Vector2.up * gridSize;
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) newPos = currentPos + Vector2.down * gridSize;
            if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) newPos = currentPos + Vector2.right * gridSize;
            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) newPos = currentPos + Vector2.left * gridSize;
            
        }
        else
        {
            bool valid=unitSelector.CheckForMovement();
            if (valid)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W)) newPos = currentPos + Vector2.up * gridSize;
                if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S)) newPos = currentPos + Vector2.down * gridSize;
                if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D)) newPos = currentPos + Vector2.right * gridSize;
                if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A)) newPos = currentPos + Vector2.left * gridSize;
            }
            
        }
           

        gameObject.transform.position = newPos;
    }
}
