using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    [System.Serializable]
    public class ItemSO : ScriptableObject
    {
        public int ID; // Unique identifier for the item

        [SerializeField]
        private string itemImageBase64; // Serialized sprite as Base64 string

        [SerializeField] // Exclude from serialization
        public Sprite itemImage; // Direct field to store the sprite
        public string itemImagePath;
        public bool IsStackable;
        public int MaxStackSize = 1;
        public string Name;
        public string Description;
        public ItemCategory category;

        // Property to get the sprite
        public Sprite ItemImage
        {
            get
            {
                if (itemImage != null)
                    return itemImage;

                if (string.IsNullOrEmpty(itemImageBase64))
                    return null;

                return LoadSpriteFromBase64(itemImageBase64);
            }
        }

        // Method to set the sprite
        public void SetItemImage(Sprite sprite)
        {
            if (sprite == null)
            {
                itemImageBase64 = null;
                itemImage = null;
                return;
            }

            itemImage = sprite;

            // Serialize the sprite as Base64 string
            itemImageBase64 = SpriteToBase64(sprite);
        }
        // Define the ItemCategory enum
        public enum ItemCategory
        {
            Weapon,
            Armor,
            Consumable,
            // Add more categories as needed, including Skill if applicable
            Skill,
        }


        // Convert Sprite to Base64 string
        private string SpriteToBase64(Sprite sprite)
        {
            Texture2D texture = sprite.texture;
            byte[] textureData = texture.EncodeToPNG();
            return System.Convert.ToBase64String(textureData);
        }

        // Convert Base64 string to Sprite
        private Sprite LoadSpriteFromBase64(string base64)
        {
            byte[] textureData = System.Convert.FromBase64String(base64);
            Texture2D texture = new Texture2D(1, 1);
            texture.LoadImage(textureData);
            return Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        }
    }
}
