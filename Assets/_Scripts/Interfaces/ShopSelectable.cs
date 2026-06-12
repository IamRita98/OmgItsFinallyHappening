using UnityEngine;

public class ShopSelectable : MonoBehaviour, IInteractable
{
    public DialogueNodeSO shopDialogue;
    DialogueManager dManager;
    Shop shop;

    private void Start()
    {
        dManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
        shop = GetComponent<Shop>();
    }

    public void Interact()
    {
        UIManager.Instance.HideButtonPromptUI();
        UIManager.Instance.ShowDialogueComponents();
        dManager.LoadNode(shopDialogue);
        shop.DisplayShop();
    }
}
