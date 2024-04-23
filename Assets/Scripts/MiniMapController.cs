using UnityEngine;
using UnityEngine.EventSystems;

public class MiniMapController : MonoBehaviour, IPointerClickHandler
{
    // Reference to the render texture Raw Image component
    public UnityEngine.UI.RawImage minimapRenderTexture;

    // Reference to the full map Raw Image component
    public UnityEngine.UI.RawImage fullMapRenderTexture;

    // Function to handle tap event
    public void OnPointerClick(PointerEventData eventData)
    {
        // Check if the minimap is tapped
        if (eventData.pointerPress == minimapRenderTexture.gameObject)
        {
            // Assign the minimap's render texture to the full map's texture
            fullMapRenderTexture.texture = minimapRenderTexture.texture;
        }
    }
}
