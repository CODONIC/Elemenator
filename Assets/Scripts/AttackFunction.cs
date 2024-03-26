using UnityEngine;

public class AttackFunction : MonoBehaviour
{
    private bool isInteracting;
    public NPC npc; // Reference to the NPC script

    // This method will be called when the button is clicked
    public void OnButtonClick()
    {
        // Implement your basic attack logic here
        Debug.Log("Basic attack executed!");

        // Check if interacting with an NPC
        if (isInteracting && npc != null)
        {
            npc.TriggerDialogue(); // Call TriggerDialogue on the NPC script
        }
        else
        {
            // If not interacting with an NPC, perform a basic attack
            PerformBasicAttack();
        }
    }

    void InteractWithNearestNPC(NPC npc)
    {
        // Implement logic to interact with the nearby NPC or interactable object
        Debug.Log("Interacting with nearest NPC...");
        this.npc = npc; // Set the NPC reference
        isInteracting = true; // Set interacting flag
    }

    void PerformBasicAttack()
    {
        // Implement your basic attack logic here
        Debug.Log("Basic attack executed!");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            // Get reference to the NPC script
            NPC npc = other.GetComponent<NPC>();
            if (npc != null)
            {
                InteractWithNearestNPC(npc);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
        {
            isInteracting = false; // Reset interacting flag
            npc = null; // Reset NPC reference
        }
    }
}
