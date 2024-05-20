using System.Collections;
using System.Collections.Generic;
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
            public ItemSO[] recipe;
            public int required;
        }


        public List<Crafting> craftItems;


        public void Craft()
        {

            //if (slotItem.Equal recipe)
            //{

            //}

            for(int i = 0; i < craftItems.Count; i++)
            {
                if (craftItems[i].itemRecipe.Equals(craftItems[i].itemRecipe))
                {

                }
            }
        }
    }
}