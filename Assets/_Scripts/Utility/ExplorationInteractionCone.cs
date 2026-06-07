using UnityEngine;

public class ExplorationInteractionCone : MonoBehaviour
{
    public PlayerExplorationController controller;
    private void Start()
    {
        controller = gameObject.GetComponentInParent<PlayerExplorationController>(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<IInteractable>() != null)
        {
            controller.GOInInteractionCone = collision.gameObject;
            Debug.Log(controller.GOInInteractionCone);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        controller.GOInInteractionCone = null;
    }
}
