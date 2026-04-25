using UnityEngine;

public class DialogueChoice
{
    public string ChoiceText { get; set; }
    public DialogueNode NextNode { get; set; }
    public DialogueChoice(string choiceText,DialogueNode nextNode)
    {
        ChoiceText = choiceText;
        NextNode = nextNode;
    }
}
