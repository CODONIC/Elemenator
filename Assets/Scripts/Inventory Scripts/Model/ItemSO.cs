using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    [System.Serializable]
    public class ItemSO : ScriptableObject
    {
        public int ID; // Unique identifier for the item

        [field: SerializeField]
        public bool IsStackable { get; set; }

        [field: SerializeField]
        public int MaxStackSize { get; set; } = 1;

        [field: SerializeField]
        public string Name { get; set; }

        [field: SerializeField]
        [field: TextArea]
        public string Description { get; set; }

        [field: SerializeField]
        public Sprite ItemImage { get; set; }
    }
}
