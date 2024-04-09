using UnityEngine;

[CreateAssetMenu(fileName = "New Element", menuName = "Inventory/Element")]
public class Element : ScriptableObject
{
    public Sprite image;
    public string elementName;
    public int atomicNumber;
    public string symbol;
    public int quantity;
    public float electronegativity; // Electronegativity of the element
}
