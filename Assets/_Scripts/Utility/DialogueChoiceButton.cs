using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueChoiceButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label;
    [SerializeField] private Button button;

  public void Setup(string text, DialogueNodeSO nextNode)
    {
        label.text = text;
        button.onClick.AddListener(() =>
        {
            DialogueManager.Instance.LoadNode(nextNode);
        });
    }
}
