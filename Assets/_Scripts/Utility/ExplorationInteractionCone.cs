using UnityEngine;

public class ExplorationInteractionCone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Cumb");
        if (collision.gameObject.GetComponent<IInteractable>() != null)
        {
            //IInteractable interact = collision.gameObject.GetComponent<IInteractable>();
            //interact.Interact();
            collision.gameObject.GetComponent<IInteractable>().Interact();
        }
    }
}
