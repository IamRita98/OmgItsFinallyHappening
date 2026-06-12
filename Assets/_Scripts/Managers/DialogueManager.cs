using System;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{
    enum DialogueType
    {
        Dialogue,
        Shop,
        Quest
    }

    DialogueType dType;
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
    public static event Action FinishedDialogue;
    public static event Action OpenShop;

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

    // add signal when dialogue ends for any subscribers to run their logic on end of dialogue
    void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Z))
        {
            if (isTyping)
            {//skip animating text
                StopAllCoroutines();
                textComponent.text = currentNode.dialogueText;
                isTyping = false;
                CheckDialogueState();
            }
            else
            {
                CheckDialogueState();
            }
            /*else //if (currentNode.choices.Count == 0) UNCOMMENT BLOCK IF ABOVE SETUP IS NOT WORKING PROPERLY
            {
                if (currentNode.nextNode)
                {
                    LoadNode(currentNode.nextNode);
                }

                return;
                //gameObject.SetActive(false);
            }*/

        }
    }

    void CheckDialogueState()
    {
        if (currentNode.choices.Count > 0)
        {
            ShowDialogueChoices();
        }
        else if (currentNode.nextNode)
        {
            LoadNode(currentNode.nextNode);
        }
        else
        {
            switch (dType)
            {
                case (DialogueType.Dialogue): break;
                case (DialogueType.Shop):
                    OpenShop?.Invoke();
                    break;
                case (DialogueType.Quest): break;
            }

            FinishedDialogue?.Invoke();
        }
    }


    public void LoadNode(DialogueNodeSO node)
    {
        if (!node)
        {//no dialogue
            //gameObject.SetActive(false);
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
