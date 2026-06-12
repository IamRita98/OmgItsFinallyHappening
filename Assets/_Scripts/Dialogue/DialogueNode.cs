using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DialogueNode
{
    public string DialogueText { get; set; }
    public List<DialogueChoice> DialogueChoices { get; set; }
    public DialogueNode(string dialogueText)
    {
        DialogueText = dialogueText;
        DialogueChoices = new List<DialogueChoice>();
    }
}
