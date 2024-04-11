using UnityEngine;
using UnityEngine.UI;

public class ResetButton : MonoBehaviour
{
    public void OnResetHealthButtonClick()
    {
        SaveManager.Instance.DeletePlayerPreference("PlayerHealth");
        Debug.Log("Player health preference deleted!");
    }
}
