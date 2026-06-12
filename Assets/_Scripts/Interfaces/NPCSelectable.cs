using UnityEngine;
using UnityEngine.UI;

public class NPCSelectable : MonoBehaviour, IInteractable
{
    public DialogueNodeSO npcDialogue;
    DialogueManager dManager;

    private void Start()
    {
        dManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
    }

    public void Interact()
    {
        UIManager.Instance.HideButtonPromptUI();
        UIManager.Instance.ShowDialogueComponents();
        dManager.LoadNode(npcDialogue);
    }
}
