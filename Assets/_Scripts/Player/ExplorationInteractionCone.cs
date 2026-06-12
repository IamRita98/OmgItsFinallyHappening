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
            if (collision.gameObject.GetComponent<NPCSelectable>())
            {
                UIManager.Instance.FillAndDisplayButtonPromptUI("Z Talk");
            }
            else if (collision.gameObject.GetComponent<ShopSelectable>())
            {
                UIManager.Instance.FillAndDisplayButtonPromptUI("Z Shop");
            }

            controller.GOInInteractionCone = collision.gameObject;
            Debug.Log(controller.GOInInteractionCone);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        controller.GOInInteractionCone = null;
        UIManager.Instance.HideButtonPromptUI();
    }
}
