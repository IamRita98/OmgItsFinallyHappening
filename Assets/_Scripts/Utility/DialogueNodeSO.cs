using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(fileName = "NewNode", menuName = "Dialogue/Node")]
public class DialogueNodeSO : ScriptableObject
{
    [TextArea(2,4)]//for inspector only
    public string dialogueText;
    public List<DialogueChoiceSO> choices;
    public string charName;
    //Next Node if no choice
    public DialogueNodeSO d;
}
