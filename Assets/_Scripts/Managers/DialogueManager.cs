using UnityEngine;
using TMPro;
using System.Collections;
using NUnit.Framework;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    public TextMeshProUGUI textComponent;
    public Transform choiceContainer;
    public GameObject choiceButtonPrefab;
    
    [Header("Settings")]
    public float textSpeed;
    public DialogueNodeSO startNode;

    private DialogueNodeSO currentNode;
    private bool isTyping = false;
    private List<GameObject> activeChoiceButtons = new List<GameObject>();

    private int index;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
    }
    void Start()
    {
        textComponent.text = string.Empty;
        LoadNode(startNode);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {//skip animating text
                StopAllCoroutines();
                textComponent.text = currentNode.dialogueText;
                isTyping = false;
                if (currentNode.choices.Count > 0)
                {
                    ShowDialogueChoices();
                }
            }else if (currentNode.choices.Count == 0)
            {
                gameObject.SetActive(false);
            }

        }
    }
    public void LoadNode(DialogueNodeSO node)
    {
        if (node == null)
        {//no dialogue
            gameObject.SetActive(false);
            return;
        }
        ClearDialogueChoices();
        currentNode = node;
        textComponent.text = string.Empty;
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }
    
    IEnumerator TypeLine()
    {
        isTyping = true;
        foreach (char c in currentNode.dialogueText)
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
        isTyping = false;
        if (currentNode.choices.Count > 0) ShowDialogueChoices();
    }
    void ShowDialogueChoices()
    {
        foreach (DialogueChoiceSO choice in currentNode.choices)
        {
            GameObject buttonObj = Instantiate(choiceButtonPrefab, choiceContainer);
            buttonObj.GetComponent<DialogueChoiceButton>().Setup(choice.choiceText, choice.nextNode);
            activeChoiceButtons.Add(buttonObj);
        }
    }
    void ClearDialogueChoices()
    {
        foreach(GameObject button in activeChoiceButtons)
        {
            Destroy(button);
        }
        activeChoiceButtons.Clear();
    }
}
