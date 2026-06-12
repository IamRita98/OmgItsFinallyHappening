using UnityEngine;

[CreateAssetMenu(fileName = "NewChoice", menuName = "Dialogue/Choice")]
public class DialogueChoiceSO : ScriptableObject
{
    public string choiceText;
    public DialogueNodeSO nextNode;
}
