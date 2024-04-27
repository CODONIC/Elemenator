using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    public void OnResetHealthButtonClick()
    {
        SaveManager.Instance.DeletePlayerPreference("PlayerHealth");
        Debug.Log("Player health preference deleted!");
         SaveManager.Instance.DeletePlayerPreference("PlayerPositionX");
        SaveManager.Instance.DeletePlayerPreference("PlayerPositionY");
        SaveManager.Instance.DeletePlayerPreference("PlayerPositionZ");
        Debug.Log("Player position preferences deleted!");
      
        Debug.Log("Inventory cleared!");
    }
}
