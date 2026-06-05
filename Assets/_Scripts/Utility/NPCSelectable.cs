using UnityEngine;

public class NPCSelectable : MonoBehaviour,IInteractable
{
    public DialogueNodeSO npcDialogue;
    public void Interact()
    {
        GameObject dBox = GameObject.FindGameObjectWithTag("DialogueManager");
        dBox.SetActive(true);
        DialogueManager dManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        dManager.LoadNode(npcDialogue);
    }
}
