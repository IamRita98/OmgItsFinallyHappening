using UnityEngine;
using UnityEngine.UI;

public class NPCSelectable : MonoBehaviour, IInteractable
{
    public DialogueNodeSO npcDialogue;

    public void Interact()
    {
        DialogueManager dManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        UIManager.Instance.ShowDialogueComponents();
        dManager.LoadNode(npcDialogue);
    }
}
