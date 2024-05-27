using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Inventory.Model
{
    public class CraftingSystem : MonoBehaviour
    {
        [System.Serializable]
        public class Crafting
        {
            public string craftName;
            public ItemSO craftObject;
            public List<CraftingRecipe> itemRecipe;
        }

        [System.Serializable]
        public class CraftingRecipe
        {
            public string name;
            public RecipeIngredient[] recipe;
           
        }

        [System.Serializable]
        public class RecipeIngredient
        {
            public int ID;
            public int quantity;
            public ItemSO item;
        }

        public List<Crafting> craftItems;

        public class CraftingResult
        {
            public ItemSO CraftedItem { get; set; }
            public List<InventoryItem> UsedIngredients { get; set; }
            public int CraftingSlotIndex { get; set; } // Add this property
        }

        public CraftingResult Craft(InventoryItem[] inputItems)
        {
            CraftingResult bestResult = null;
            int maxIngredientsUsed = 0;

            foreach (var crafting in craftItems)
            {
                foreach (var recipe in crafting.itemRecipe)
                {
                    if (IsRecipeMatch(recipe.recipe, inputItems, out List<InventoryItem> usedIngredients))
                    {
                        // Calculate the total number of ingredients used in this recipe
                        int totalIngredientsUsed = usedIngredients.Sum(item => item.quantity);

                        // Check if this recipe uses more ingredients than the previous best result
                        if (totalIngredientsUsed > maxIngredientsUsed)
                        {
                            maxIngredientsUsed = totalIngredientsUsed;
                            bestResult = new CraftingResult
                            {
                                CraftedItem = crafting.craftObject,
                                UsedIngredients = usedIngredients
                            };
                        }
                    }
                }
            }

            return bestResult; // Return the best result found
        }


        private bool IsRecipeMatch(RecipeIngredient[] recipe, InventoryItem[] inputItems, out List<InventoryItem> usedIngredients)
        {
            Dictionary<int, int> inputItemQuantities = new Dictionary<int, int>();
            usedIngredients = new List<InventoryItem>();

            // Populate the dictionary with input item quantities
            foreach (var item in inputItems)
            {
                if (inputItemQuantities.ContainsKey(item.ID))
                {
                    inputItemQuantities[item.ID] += item.quantity;
                }
                else
                {
                    inputItemQuantities[item.ID] = item.quantity;
                }
            }

            bool hasEnoughIngredients = true; // Assume there are enough ingredients initially

            // Check if all ingredients in the recipe are present in the input items with sufficient quantity
            foreach (var ingredient in recipe)
            {
                if (!inputItemQuantities.ContainsKey(ingredient.ID) || inputItemQuantities[ingredient.ID] < ingredient.quantity)
                {
                    // If any ingredient is not present in sufficient quantity, set hasEnoughIngredients to false
                    hasEnoughIngredients = false;
                    break;
                }
            }

            if (hasEnoughIngredients)
            {
                // If there are enough ingredients, deduct their quantities from the input items
                foreach (var ingredient in recipe)
                {
                    int remainingQuantity = inputItemQuantities[ingredient.ID] - ingredient.quantity;
                    inputItemQuantities[ingredient.ID] = remainingQuantity;

                    // Add the used ingredient to the list
                    usedIngredients.Add(new InventoryItem
                    {
                        ID = ingredient.ID,
                        item = ingredient.item,
                        quantity = ingredient.quantity
                    });
                }
            }

            return hasEnoughIngredients;
        }







    }
}
