using Cysharp.Threading.Tasks;
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

    async public void Interact()
    {
        UIManager.Instance.HideButtonPromptUI();
        UIManager.Instance.ShowDialogueComponents();
        dManager.LoadNode(shopDialogue);
        GameStateManager.Instance.state = State.Menu;

        await UniTask.WaitUntil(() => Input.GetKeyDown(KeyCode.Z));
        shop.DisplayShop();

    }
}
