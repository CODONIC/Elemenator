using System.Collections.Generic;
using UnityEngine;
using static Inventory.Model.CraftingSystem;

public class CraftingRecipeDatabase : MonoBehaviour
{
    // Define a list to store crafting recipes
    public List<CraftingRecipe> recipes = new List<CraftingRecipe>();

    // Method to add a new crafting recipe to the database
    public void AddRecipe(CraftingRecipe recipe)
    {
        recipes.Add(recipe);
        Debug.Log("Added recipe: " + recipe.itemID1 + " + " + recipe.itemID2);
    }

    // Method to get a crafting recipe based on the IDs of two items
    public CraftingRecipe GetRecipe(int itemID1, int itemID2)
    {
        foreach (var recipe in recipes)
        {
            // Check if the recipe matches the combination of item IDs
            if ((recipe.itemID1 == itemID1 && recipe.itemID2 == itemID2) ||
                (recipe.itemID1 == itemID2 && recipe.itemID2 == itemID1))
            {
                Debug.Log("Found recipe: " + recipe.itemID1 + " + " + recipe.itemID2);
                return recipe;
            }
        }
        // No matching recipe found
        Debug.Log("No recipe found for combination: " + itemID1 + " + " + itemID2);
        return null;
    }
}
