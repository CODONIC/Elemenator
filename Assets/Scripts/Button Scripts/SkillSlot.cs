using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    public int index; // Index of this skill slot
    public Image skillIcon; // Reference to the child image representing the skill icon
    private bool isEquipped = false; // Flag to track if this slot is currently equipped with a skill

    // Method to equip a skill into this slot
    public void EquipSkill(Sprite skillSprite)
    {
        if (!isEquipped)
        {
            skillIcon.sprite = skillSprite; // Set the skill icon to the provided sprite
            isEquipped = true;
        }
        else
        {
            Debug.LogWarning("This slot is already equipped with a skill!");
        }
    }

    // Method to unequip the skill from this slot
    public void UnequipSkill()
    {
        if (isEquipped)
        {
            skillIcon.sprite = null; // Remove the skill icon
            isEquipped = false;
        }
        else
        {
            Debug.LogWarning("No skill is equipped in this slot!");
        }
    }

    // Method to check if this slot is currently equipped with a skill
    public bool IsEquipped()
    {
        return isEquipped;
    }
}
