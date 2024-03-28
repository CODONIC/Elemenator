using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NPC : MonoBehaviour
{
    public GameObject dialoguePanel;
    public Text dialogueText;
    public Image npcImage; // Reference to NPC Image component
    public Text npcNameText; // Reference to NPC Name text component
    public string[] dialogue;
    public Sprite npcSprite; // NPC Image sprite
    public string npcName; // NPC Name
    private int index;

    public float wordSpeed = 0.001f; // Adjust this value to control the speed of the typing animation

    public bool playerIsClose;

    public Button touchButton;

    private Coroutine typingCoroutine; // Reference to the typing coroutine
    private bool inDialogue = false; // Flag to track if NPC is engaged in dialogue

    void Start()
    {
        touchButton.onClick.AddListener(OnTouchButtonClicked);
        dialoguePanel.SetActive(false);
    }

    void OnTouchButtonClicked()
    {
        if (inDialogue)
        {
            if (dialoguePanel.activeInHierarchy)
            {
                if (typingCoroutine != null)
                {
                    StopCoroutine(typingCoroutine);
                }
                NextLine();
            }
            else
            {
                EndDialogue();
            }
        }
        else if (playerIsClose) // Trigger dialogue only if the player is close and NPC is not in dialogue
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        // Set inDialogue flag to true
        inDialogue = true;
        // Reset index to start from the beginning
        index = 0;
        // Clear previous dialogue
        dialogueText.text = "";
        // Set NPC image
        npcImage.sprite = npcSprite;
        // Set NPC name
        npcNameText.text = npcName;
        // Show dialogue panel
        dialoguePanel.SetActive(true);
        // Start typing coroutine for new dialogue
        typingCoroutine = StartCoroutine(Typing());
    }

    void EndDialogue()
    {
        // Set inDialogue flag to false
        inDialogue = false;
        // Hide dialogue panel
        dialoguePanel.SetActive(false);
        // Stop typing coroutine if it's running
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }
    }

    IEnumerator Typing()
    {
        foreach (char letter in dialogue[index].ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(wordSpeed * Time.deltaTime);
        }
    }

    void NextLine()
    {
        index++;
        if (index < dialogue.Length)
        {
            // Clear previous dialogue
            dialogueText.text = "";
            // Start typing new line
            typingCoroutine = StartCoroutine(Typing());
        }
        else
        {
            EndDialogue(); // End dialogue when all lines are displayed
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsClose = false;
            // If NPC is in dialogue when the player exits, end the dialogue
            if (inDialogue)
            {
                EndDialogue();
            }
        }
    }
}
